using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ChameleonJoinRoomFunction
{
    public class Function
    {
        public Person FunctionHandler(JoinRoomRequest input, ILambdaContext context)
        {
            return new Person(roomCode: input.RoomCode, personId: "Person1");
        }
    }

    public class JoinRoomRequest
    {
        public string RoomCode { get; set; }
    }

    public class Person
    {
        public Person(string roomCode, string personId)
        {
            RoomCode = roomCode;
            PersonId = personId;
        }

        public string RoomCode { get; }
        public string PersonId { get; }
    }
}
