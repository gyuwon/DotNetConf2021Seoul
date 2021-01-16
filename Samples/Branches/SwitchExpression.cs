using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sample
{
    [TestClass]
    public class SwitchExpression
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [DataRow(null)]
        [DataRow("foo")]
        public void PartialSwitchStatement(string source)
        {
            bool hasValue = default;

            switch (source)
            {
                case null:
                    hasValue = false;
                    break;
            }

            TestContext.WriteLine($"{hasValue}");
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("foo")]
        public void PartialSwitchExpression(string source)
        {
            bool hasValue = source switch
            {
                null => false
            };

            TestContext.WriteLine($"{hasValue}");
        }
    }
}
