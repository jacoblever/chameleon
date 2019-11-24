using Amazon.Lambda.Core;
using GameLogic;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ChameleonJoinRoomFunction
{
    public class Function
    {
        public Person FunctionHandler(JoinRoomRequest input, ILambdaContext context)
        {
            var roomAndPerson = ChameleonGame.Create().JoinOrCreateRoom(input.RoomCode);
            return new Person(roomAndPerson.RoomCode, roomAndPerson.PersonId);
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
