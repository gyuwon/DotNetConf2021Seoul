#nullable enable

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sample
{
    [TestClass]
    public class NullableReference
    {
        private static void CallClone(string value) => value.Clone();

        [TestMethod]
        public void ReferenceVariable()
        {
            string value = null;

            CallClone(value);

            if (value == null)
            {
                return;
            }

            CallClone(value);
        }

        [TestMethod]
        [DataRow("foo")]
        [DataRow(null)]
        public void ReferenceArgument(string value)
        {
            CallClone(value);
        }

        [TestMethod]
        [DataRow("foo")]
        [DataRow(null)]
        public void NullableReferenceArgument(string? value)
        {
            CallClone(value);

            if (value != null)
            {
                CallClone(value);
            }
        }
    }
}
