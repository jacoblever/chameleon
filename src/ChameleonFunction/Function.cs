using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ChameleonFunction
{
    public class Function
    {
        public string FunctionHandler(string input, ILambdaContext context)
        {
            var value = input?.ToUpper();
            return value + " YES";
        }   
    }
}
