using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Sample
{
    [TestClass]
    public class LinqAsync
    {
        public TestContext TestContext { get; set; }

        private async IAsyncEnumerable<int> Get()
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(millisecondsDelay: 100);
                yield return i;
            }
        }

        [TestMethod]
        public async Task UseSelect()
        {
            int result = await Get().Select(Square).SumAsync();
            TestContext.WriteLine($"Result: {result}");
        }

        private int Square(int x) => x * x;

        [TestMethod]
        public async Task UseSelectAwait()
        {
            int result = await Get().SelectAwait(SquareAsync).SumAsync();
            TestContext.WriteLine($"Result: {result}");
        }

        private async ValueTask<int> SquareAsync(int x)
        {
            await Task.Delay(millisecondsDelay: 100);
            return x * x;
        }

        [TestMethod]
        public async Task Parallelism()
        {
            List<Task<int>> tasks = await Get().Select(SquareAsync)
                                               .Select(valueTask => valueTask.AsTask())
                                               .ToListAsync();
            int[] squares = await Task.WhenAll(tasks);
            int result = squares.Sum();
            TestContext.WriteLine($"Result: {result}");
        }
    }
}
