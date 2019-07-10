using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ChameleonJoinRoomFunction
{
    public class Function
    {
        public Person FunctionHandler(string input, ILambdaContext context)
        {
            var value = input?.ToUpper();
            return new Person();
        }
    }

    public class Person
    {
        public string RoomCode { get; }
        public string PersonId { get; }
    }
}
