using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>Delegate used by an expression to do the math evaluation.</summary>
    /// <param name="numbers">The numbers to evaluate.</param>
    /// <returns>The result of the evaluated numbers.</returns>
    public delegate object MathEvaluate(object[] numbers);
}
