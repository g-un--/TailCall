using TailRecursive;
using Xunit;
using Shouldly;
using System.Diagnostics;

namespace TailCall.Tests
{
    public class SyncTailFuncTests 
    {
        [Fact]
        public void Iterate()
        {
            var nrOfCalls = 0;
            var iterate = SyncTailFunc<int, int>.As((me, val) =>
            {
                if (val < 42)
                    return me(val + 1);
                else
                    return val;
            })
            .InterceptWith(_=> nrOfCalls += 1)
            .ToFunc();

            var result = iterate(1);

            result.ShouldBe(42);
            nrOfCalls.ShouldBe(42);
        }

        [Fact]
        public void StackSizeIsConstant()
        {
            int? previousStackFrameCount = null;
            var iterate = SyncTailFunc<int, int>.As((me, val) =>
            {
                var currentStackFrameCount = new StackTrace().FrameCount;
                if (previousStackFrameCount.HasValue)
                    currentStackFrameCount.ShouldBe(previousStackFrameCount.Value);
                previousStackFrameCount = currentStackFrameCount;

                if (val < 42)
                    return me(val + 1);
                else
                    return val;
            }).Run(1);
        }
    }
}
