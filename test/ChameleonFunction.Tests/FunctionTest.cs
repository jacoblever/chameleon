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
            var upperCase = function.FunctionHandler("Hello World", context);

            Assert.Equal("HELLO WORLD YES", upperCase);
        }
    }
}
