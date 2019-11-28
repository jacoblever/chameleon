using Amazon.Lambda.Core;
using GameLogic;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ChameleonJoinRoomFunction
{
    public class Function
    {
        // TODO: Make this a lambda proxy like the others
        public Person FunctionHandler(JoinRoomRequest input, ILambdaContext context)
        {
            try
            {
                var chameleonGame = ChameleonGame.Create();
                var roomAndPerson = input.RoomCode == null
                    ? chameleonGame.CreateRoom()
                    : chameleonGame.JoinRoom(input.RoomCode);
                return new Person(roomAndPerson.RoomCode, roomAndPerson.PersonId);
            }
            catch (RoomDoesNotExistException e)
            {
                return new Person(input.RoomCode, e.Message);
            }
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
