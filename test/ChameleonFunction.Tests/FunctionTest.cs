using Xunit;
using Amazon.Lambda.TestUtilities;

namespace ChameleonFunction.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void TestToUpperFunction()
        {
            var function = new Function();
            var context = new TestLambdaContext();
            var result = function.FunctionHandler("ABCD", context);

            var expected = new RoomStatus(
                code: "ABCD",
                peopleCount: 4,
                chameleonCount: 2,
                state: RoomState.InGame,
                character: "butterfly");
            Assert.Equal(expected, result);
        }
    }
}
