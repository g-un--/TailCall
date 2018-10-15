using System.Threading.Tasks;
using Xunit;
using Shouldly;
using TailRecursive;
using System.Diagnostics;

namespace TailCall.Tests
{
    public class AsyncTailFuncTests
    {
        [Fact]
        public async Task Iterate()
        {
            var nrOfCalls = 0;
            var iterate = AsyncTailFunc<int, int>.As(async (me, val) =>
            {
                await Task.Delay(1);
                if (val < 42)
                    return await me(val + 1);
                else
                    return val;
            })
            .InterceptWith(_ => nrOfCalls += 1)
            .ToFunc();

            var result = await iterate(1);

            result.ShouldBe(42);
            nrOfCalls.ShouldBe(42);
        }

        [Fact]
        public async Task StackSizeIsConstant()
        {
            int? previousStackFrameCount = null;
            await AsyncTailFunc<int, int>.As(async (me, val) =>
            {
                await Task.Delay(1);

                var currentStackFrameCount = new StackTrace().FrameCount;
                if (previousStackFrameCount.HasValue)
                    currentStackFrameCount.ShouldBeLessThanOrEqualTo(previousStackFrameCount.Value);
                previousStackFrameCount = currentStackFrameCount;

                if (val < 42)
                    return await me(val + 1);
                else
                    return val;
            }).Run(1);
        }
    }
}
