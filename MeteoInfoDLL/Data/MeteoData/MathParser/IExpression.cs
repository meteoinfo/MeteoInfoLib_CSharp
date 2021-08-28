using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// The interface used when running expressions
    /// </summary>
    public interface IExpression
    {
        /// <summary>Gets the number of arguments this expression uses.</summary>
        /// <value>The argument count.</value>
        int ArgumentCount { get; }
        /// <summary>Gets or sets the evaluate delegate.</summary>
        /// <value>The evaluate delegate.</value>
        MathEvaluate Evaluate { get; set; }
    }
}
