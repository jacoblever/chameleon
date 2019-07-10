using Amazon.Lambda.Core;
using DataStore;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ChameleonJoinRoomFunction
{
    public class Function
    {
        public Person FunctionHandler(string input, ILambdaContext context)
        {
            var value = input?.ToUpper();
            var a = Client.TestSave();
            return new Person(roomCode: a, personId: "Person1");
        }
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
