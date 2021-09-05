using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>The base class for expressions</summary>
    public abstract class ExpressionBase : IExpression
    {
        /// <summary>Gets the number of arguments this expression uses.</summary>
        /// <value>The argument count.</value>
        public abstract int ArgumentCount { get; }

        private MathEvaluate _evaluateDelegate;

        /// <summary>Gets or sets the evaluate delegate.</summary>
        /// <value>The evaluate delegate.</value>
        public virtual MathEvaluate Evaluate
        {
            get { return _evaluateDelegate; }
            set { _evaluateDelegate = value; }
        }

        /// <summary>Validates the specified numbers for the expression.</summary>
        /// <param name="numbers">The numbers to validate.</param>
        /// <exception cref="ArgumentNullException">When numbers is null.</exception>
        /// <exception cref="ArgumentException">When the length of numbers do not equal <see cref="ArgumentCount"/>.</exception>
        protected void Validate(object[] numbers)
        {
            if (numbers == null)
                throw new ArgumentNullException("numbers");
            if (numbers.Length != ArgumentCount)
                throw new ArgumentException("Invalid length of Array", "numbers");
        }
    }
}
