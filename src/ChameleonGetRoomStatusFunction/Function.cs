using Amazon.Lambda.Core;
using DataStore;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ChameleonGetRoomStatusFunction
{
    public class Function
    {
        public RoomStatus FunctionHandler(ILambdaContext context)
        {
            return new RoomStatus(
                code: "ABCD",
                peopleCount: 4,
                chameleonCount: 2,
                state: RoomState.InGame,
                character: "");
        }
    }
}
