﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// A class representing the System.Math function expressions
    /// </summary>
    public class FunctionExpression : ExpressionBase
    {
        // must be sorted
        /// <summary>The supported math functions by this class.</summary>
        private static readonly string[] mathFunctions = new string[]
            {
                "abs", "acos", "asin", "atan", "cos", "exp",
                "log", "log10", "sin", "sqrt", "tan"                
            };

        /// <summary>Initializes a new instance of the <see cref="FunctionExpression"/> class.</summary>
        /// <param name="function">The function name for this instance.</param>
        public FunctionExpression(string function)
            : this(function, true)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="FunctionExpression"/> class.</summary>
        /// <param name="function">The function.</param>
        /// <param name="validate">if set to <c>true</c> to validate the function name.</param>
        internal FunctionExpression(string function, bool validate)
        {
            function = function.ToLowerInvariant();

            if (validate && !IsFunction(function))
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, "Invalid function name", _function),
                    "function");

            _function = function;
            base.Evaluate = new MathEvaluate(Execute);
        }

        private string _function;

        /// <summary>Gets the name function for this instance.</summary>
        /// <value>The function name.</value>
        public string Function
        {
            get { return _function; }
        }

        /// <summary>Executes the function on specified numbers.</summary>
        /// <param name="numbers">The numbers used in the function.</param>
        /// <returns>The result of the function execution.</returns>
        /// <exception cref="ArgumentNullException">When numbers is null.</exception>
        /// <exception cref="ArgumentException">When the length of numbers do not equal <see cref="ArgumentCount"/>.</exception>
        public object Execute(object[] numbers)
        {
            base.Validate(numbers);

            string function = char.ToUpperInvariant(_function[0]) + _function.Substring(1);
            switch (function)
            {
                case "Abs":
                    return DataMath.Abs(numbers[0]);                    
                case "Acos":
                    return DataMath.Acos(numbers[0]); 
                case "Asin":
                    return DataMath.Asin(numbers[0]);
                case "Atan":
                    return DataMath.Atan(numbers[0]);
                case "Cos":
                    return DataMath.Cos(numbers[0]);
                case "Exp":
                    return DataMath.Exp(numbers[0]);
                case "Log":
                    return DataMath.Log(numbers[0]);
                case "Log10":
                    return DataMath.Log10(numbers[0]);
                case "Sin":
                    return DataMath.Sin(numbers[0]);
                case "Sqrt":
                    return DataMath.Sqrt(numbers[0]);
                case "Tan":
                    return DataMath.Tan(numbers[0]);
                default:
                    return null;
            }
        }

        /// <summary>Executes the function on specified numbers.</summary>
        /// <param name="numbers">The numbers used in the function.</param>
        /// <returns>The result of the function execution.</returns>
        /// <exception cref="ArgumentNullException">When numbers is null.</exception>
        /// <exception cref="ArgumentException">When the length of numbers do not equal <see cref="ArgumentCount"/>.</exception>
        public object Execute_old(object[] numbers)
        {
            base.Validate(numbers);

            string function = char.ToUpperInvariant(_function[0]) + _function.Substring(1);
            MethodInfo method = typeof(Math).GetMethod(
                function,
                BindingFlags.Static | BindingFlags.Public,
                null,
                new Type[] { typeof(double) },
                null);

            if (method == null)
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture,
                        "Invalid function name", _function));

            object[] parameters = new object[numbers.Length];
            Array.Copy(numbers, parameters, numbers.Length);
            return (double)method.Invoke(null, parameters);
        }

        /// <summary>Gets the number of arguments this expression uses.</summary>
        /// <value>The argument count.</value>
        public override int ArgumentCount
        {
            get { return 1; }
        }

        /// <summary>Determines whether the specified function name is a function.</summary>
        /// <param name="function">The function name.</param>
        /// <returns><c>true</c> if the specified name is a function; otherwise, <c>false</c>.</returns>
        public static bool IsFunction(string function)
        {
            return (Array.BinarySearch(
                        mathFunctions, function,
                        StringComparer.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.</returns>
        /// <filterPriority>2</filterPriority>
        public override string ToString()
        {
            return _function;
        }

        /// <summary>
        /// Gets the function names.
        /// </summary>
        /// <returns>An array of function names.</returns>
        public static string[] GetFunctionNames()
        {
            return (string[])mathFunctions.Clone();
        }
    }
}
