using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Sample
{
    [TestClass]
    public class IAsyncEnumerableT
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public Task Run()
        {
            IAsyncEnumerable<int> xs = Get();
            var cancellation = new CancellationTokenSource();
            cancellation.Cancel();
            return PrintValues(xs, cancellation.Token);
        }

        public async IAsyncEnumerable<int> Get([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            TestContext.WriteLine($"Canceled: {cancellationToken.IsCancellationRequested}");

            await Task.Delay(millisecondsDelay: 100, cancellationToken);
            yield return 1;

            await Task.Delay(millisecondsDelay: 100, cancellationToken);
            yield return 2;
        }

        private async Task PrintValues(IAsyncEnumerable<int> xs, CancellationToken cancellationToken)
        {
            await foreach (var x in xs.WithCancellation(cancellationToken))
            {
                TestContext.WriteLine($"{x}");
            }
        }
    }
}
