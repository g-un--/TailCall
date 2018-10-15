using Shouldly;
using System.Diagnostics;
using TailRecursive;
using Xunit;

namespace TailCall.Tests
{
    public class SyncTailActionTests
    {
        [Fact]
        public void Iterate()
        {
            var nrOfCalls = 0;
            var iterate = SyncTailAction<int>.As((me, val) =>
            {
                if (val < 42)
                    me(val + 1);
            })
            .InterceptWith(_ => nrOfCalls += 1)
            .ToAction();

            iterate(1);

            nrOfCalls.ShouldBe(42);
        }

        [Fact]
        public void StackSizeIsConstant()
        {
            int? previousStackFrameCount = null;
            SyncTailAction<int>.As((me, val) =>
            {
                var currentStackFrameCount = new StackTrace().FrameCount;
                if (previousStackFrameCount.HasValue)
                    currentStackFrameCount.ShouldBe(previousStackFrameCount.Value);
                previousStackFrameCount = currentStackFrameCount;

                if (val < 42)
                    me(val + 1);
            }).Run(1);
        }
    }
}
