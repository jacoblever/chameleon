using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ChameleonGetRoomStatusFunction
{
    public class Function
    {
        public RoomStatus FunctionHandler(string input, ILambdaContext context)
        {
            var value = input?.ToUpper();
            return new RoomStatus(
                code: value,
                peopleCount: 4,
                chameleonCount: 2,
                state: RoomState.InGame,
                character: "butterfly");
        }
    }
}
