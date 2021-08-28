using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Math parser
    /// </summary>
    public class MathParser
    {
        #region Variables        
        private bool _isGridData;
        private StringBuilder _buffer = new StringBuilder();
        private Stack<string> _symbolStack = new Stack<string>();
        private Queue<IExpression> _expressionQueue = new Queue<IExpression>();
        private Dictionary<string, IExpression> _expressionCache = new Dictionary<string, IExpression>();
        private Stack<object> _calculationStack = new Stack<object>();
        private Stack<object> _parameters = new Stack<object>();
        //private List<string> _innerFunctions;
        //private ReadOnlyCollection<string> _functions;

        readonly List<string> _variables = new List<string>();
        readonly List<string> _operators = new List<string>();
        readonly List<string> _functions = new List<string>();

        private StringReader _expressionReader;
        private MeteoDataInfo _meteoDataInfo = null;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MathParser()
        {
            _operators = new List<string>(new string[] { "+", "-", "*", "/", "%", "\\", "^" });
            _functions = new List<string>(new string[] {"Abs","Atn","Atan","Cos","Exp","Ln","Log","Rnd","Sgn","Sin","Sqr","Tan",
              "Acos","Asin"});
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MathParser(MeteoDataInfo aDataInfo)
        {
            _meteoDataInfo = aDataInfo;
            _isGridData = aDataInfo.IsGridData;
            _variables = aDataInfo.GetVariableNames();            

            _operators = new List<string>(new string[] { "+", "-", "*", "/", "%", "\\", "^" });
            _functions = new List<string>(new string[] {"Abs","Atn","Atan","Cos","Exp","Ln","Log","Rnd","Sgn","Sin","Sqr","Tan",
              "Acos","Asin"});

        }

        #endregion

        #region Parser methods

        /// <summary>Evaluates the specified expression.</summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns>The result of the evaluated expression.</returns>
        /// <exception cref="ArgumentNullException">When expression is null or empty.</exception>
        /// <exception cref="ParseException">When there is an error parsing the expression.</exception>
        public object Evaluate(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                throw new ArgumentNullException("expression");

            _expressionReader = new StringReader(expression);
            _symbolStack.Clear();
            _expressionQueue.Clear();

            ParseExpressionToQueue();

            object result = CalculateFromQueue();

            //_variables[AnswerVariable] = result;
            return result;
        }

        private void ParseExpressionToQueue()
        {
            do
            {
                char c = (char)_expressionReader.Read();

                if (char.IsWhiteSpace(c))
                    continue;

                if (TryNumber(c))
                    continue;

                if (TryString(c))
                    continue;

                if (TryStartGroup(c))
                    continue;

                if (TryOperator(c))
                    continue;

                if (TryEndGroup(c))
                    continue;

                //if (TryConvert(c))
                //    continue;

                throw new ParseException("Invalid character encountered" + c);
            } while (_expressionReader.Peek() != -1);

            ProcessSymbolStack();
        }

        //public void Parse(string expr)
        //{
        //    expr = expr.Trim();
        //    _expressionReader = new StringReader(expr);
        //    while (_expressionReader.Peek() >= 0)
        //    {
        //        char c = (char)_expressionReader.Read();

        //        if (Char.IsWhiteSpace(c))
        //            continue;

        //        if (TryNumber(c))
        //            continue;

        //        if (TryString(c))
        //            continue;

        //        if (TryStartGroup(c))
        //            continue;

        //        if (TryOperator(c))
        //            continue;

        //        if (TryEndGroup(c))
        //            continue;

        //        throw new ParseException("Invalid character encountered: " + c);
        //    }
        //}

        private bool TryNumber(char c)
        {
            bool isNumber = NumberExpression.IsNumber(c);
            bool isNegative = false;
            if (NumberExpression.IsNegativeSign(c))
            {
                if (_expressionQueue.Count == 0)
                    isNegative = true;
                else if (_expressionQueue.Count > 0 && _symbolStack.Count > 0)
                {
                    if ((string)_symbolStack.Peek() == "(")
                        isNegative = true;
                }
            }

            if (!isNumber && !isNegative)
                return false;

            _buffer.Length = 0;
            _buffer.Append(c);

            char p = (char)_expressionReader.Peek();
            while (NumberExpression.IsNumber(p))
            {
                _buffer.Append((char)_expressionReader.Read());
                p = (char)_expressionReader.Peek();
            }

            double value;
            if (!(double.TryParse(_buffer.ToString(), out value)))
                throw new ParseException("Invalid number format: " + _buffer);

            NumberExpression expression = new NumberExpression(value);
            _expressionQueue.Enqueue(expression);

            return true;
        }

        private bool TryString(char c)
        {
            if (!char.IsLetter(c))
                return false;

            _buffer.Length = 0;
            _buffer.Append(c);

            char p = (char)_expressionReader.Peek();
            while (char.IsLetterOrDigit(p) || p.Equals('_') || p.Equals('@'))
            {
                _buffer.Append((char)_expressionReader.Read());
                p = (char)_expressionReader.Peek();
            }

            if (_variables.Contains(_buffer.ToString()))
            {
                object value = GetVariableValue(_buffer.ToString());
                NumberExpression expression = new NumberExpression(value);
                _expressionQueue.Enqueue(expression);

                return true;
            }

            if (FunctionExpression.IsFunction(_buffer.ToString()))
            {
                _symbolStack.Push(_buffer.ToString());
                return true;
            }

            throw new ParseException("Invalid variable: " + _buffer);
        }

        private bool TryStartGroup(char c)
        {
            if (c != '(')
                return false;

            _symbolStack.Push(c.ToString());
            return true;
        }

        private bool TryOperator(char c)
        {
            if (!OperatorExpression.IsSymbol(c))
                return false;

            bool repeat;
            string s = c.ToString();

            do
            {
                string p = _symbolStack.Count == 0 ? string.Empty : _symbolStack.Peek();
                repeat = false;
                if (_symbolStack.Count == 0)
                    _symbolStack.Push(s);
                else if (p == "(")
                    _symbolStack.Push(s);
                else if (Precedence(s) > Precedence(p))
                    _symbolStack.Push(s);
                else
                {
                    IExpression e = GetExpressionFromSymbol(_symbolStack.Pop());
                    _expressionQueue.Enqueue(e);
                    repeat = true;
                }
            } while (repeat);

            return true;
        }

        private bool TryEndGroup(char c)
        {
            if (c != ')')
                return false;

            bool ok = false;

            while (_symbolStack.Count > 0)
            {
                string p = _symbolStack.Pop();
                if (p == "(")
                {
                    ok = true;
                    break;
                }

                IExpression e = GetExpressionFromSymbol(p);
                _expressionQueue.Enqueue(e);
            }

            if (!ok)
                throw new ParseException("Unbalance parenthese");

            return true;
        }

        private void ProcessSymbolStack()
        {
            while (_symbolStack.Count > 0)
            {
                string p = _symbolStack.Pop();
                if (p.Length == 1 && p == "(")
                    throw new ParseException("Unbalance parenthese");

                IExpression e = GetExpressionFromSymbol(p);
                _expressionQueue.Enqueue(e);
            }
        }        

        private static int Precedence(string c)
        {
            if (c.Length == 1 && (c[0] == '*' || c[0] == '/' || c[0] == '%'))
                return 2;

            return 1;
        }

        private IExpression GetExpressionFromSymbol(string p)
        {
            IExpression e;

            if (_expressionCache.ContainsKey(p))
                e = _expressionCache[p];
            else if (OperatorExpression.IsSymbol(p))
            {
                e = new OperatorExpression(p);
                _expressionCache.Add(p, e);
            }
            else if (FunctionExpression.IsFunction(p))
            {
                e = new FunctionExpression(p, false);
                _expressionCache.Add(p, e);
            }
            //else if (ConvertExpression.IsConvertExpression(p))
            //{
            //    e = new ConvertExpression(p);
            //    _expressionCache.Add(p, e);
            //}
            else
                throw new ParseException("Invalid symbol on stack" + p);

            return e;
        }

        private object CalculateFromQueue()
        {
            object result;
            _calculationStack.Clear();

            foreach (IExpression expression in _expressionQueue)
            {
                if (_calculationStack.Count < expression.ArgumentCount)
                    throw new ParseException("Not enough numbers" + expression);

                _parameters.Clear();
                for (int i = 0; i < expression.ArgumentCount; i++)
                    _parameters.Push(_calculationStack.Pop());

                _calculationStack.Push(expression.Evaluate.Invoke(_parameters.ToArray()));
            }

            result = _calculationStack.Pop();
            return result;
        }

        private object GetVariableValue(string varName)
        {
            if (_meteoDataInfo == null)
                return 100;
            else
            {
                if (_isGridData)
                    return _meteoDataInfo.GetGridData(varName);
                else
                    return _meteoDataInfo.GetStationData(varName);
            }
        }
        
        #endregion

        #region Methods        

        //break the variable string into the name and its sign (if any). Es -x
        private void Catch_Sign(ref string str, ref int Sign)
        {
            string s = null;
            Sign = 1;
            s = str.Substring(0, 1);
            if (s == "-" || s == "+")
            {
                str = str.Substring(1);
                if (s == "-")
                    Sign = -Sign;
            }
        }

        #endregion
    }
}
