using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sample
{
    [TestClass]
    public class SumType
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Just()
        {
            Maybe<int> maybe = Maybe.Just(1);
            maybe.DoIfJust(value => TestContext.WriteLine($"Just {value}"));
        }

        [TestMethod]
        public void Nothing()
        {
            Maybe<int> maybe = Maybe<int>.Nothing;
            maybe.Do(forJust: _ => { }, forNothing: () => TestContext.WriteLine("Nothing"));
        }

        [TestMethod]
        [DataRow("foo", "bar")]
        [DataRow(null, "bar")]
        [DataRow("foo", null)]
        [DataRow(null, null)]
        public void Concat(string a, string b)
        {
            Maybe<string> ma = Maybe.Create(a);
            Maybe<string> mb = Maybe.Create(b);
            Maybe<string> result = ma.Bind(x => mb.Map(y => $"{x}{y}"));

            result.Do(forJust: value => TestContext.WriteLine($"Just {value}"),
                      forNothing: () => TestContext.WriteLine("Nothing"));
        }

        [TestMethod]
        [DataRow("foo", "bar")]
        [DataRow(null, "bar")]
        [DataRow("foo", null)]
        [DataRow(null, null)]
        public void ConcatLinq(string a, string b)
        {
            Maybe<string> result = from x in Maybe.Create(a)
                                   from y in Maybe.Create(b)
                                   select $"{x}{y}";

            result.Do(forJust: value => TestContext.WriteLine($"Just {value}"),
                      forNothing: () => TestContext.WriteLine("Nothing"));
        }
    }
}
 