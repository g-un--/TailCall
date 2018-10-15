using System.Threading.Tasks;
using TailRecursive;
using Xunit;
using Shouldly;
using System.Diagnostics;

namespace TailCall.Tests
{
    public class AsyncTailActionTests
    {
        [Fact]
        public async Task Iterate()
        {
            var nrOfCalls = 0;
            var iterate = AsyncTailAction<int>.As(async (me, val) =>
            {
                await Task.Delay(1);
                if (val < 42)
                    await me(val + 1);
            })
            .InterceptWith(_ => nrOfCalls += 1)
            .ToAction();

            await iterate(1);

            nrOfCalls.ShouldBe(42);
        }

        [Fact]
        public async Task StackSizeIsConstant()
        {
            int? previousStackFrameCount = null;
            await AsyncTailAction<int>.As(async (me, val) =>
            {
                await Task.Delay(1);
                
                var currentStackFrameCount = new StackTrace().FrameCount;
                if (previousStackFrameCount.HasValue)
                    currentStackFrameCount.ShouldBeLessThanOrEqualTo(previousStackFrameCount.Value);
                previousStackFrameCount = currentStackFrameCount;

                if (val < 42)
                    await me(val + 1);
            }).Run(1);
        }
    }
}
