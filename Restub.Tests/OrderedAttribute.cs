using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace Exprest.Tests
{
    /// <summary>
    /// Implicit Order attribute that uses the source line number as an order.
    /// </summary>
    public class OrderedAttribute : OrderAttribute
    {
        public OrderedAttribute([CallerLineNumber] int lineNumber = 0)
            : base(lineNumber)
        {
        }
    }
}
