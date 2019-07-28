using System;
using Amazon.Lambda.Core;
using DataStore;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ChameleonJoinRoomFunction
{
    public class Function
    {
        public Person FunctionHandler(JoinRoomRequest input, ILambdaContext context)
        {
            if (input.RoomCode == null)
            {
                var room = Client.CreateRoom();
                var personId = Client.CreatePersonInRoom(room.RoomCode);
                return new Person(room.RoomCode, personId);
            }
            if (Client.DoesRoomExist(input.RoomCode))
            {
                var personId = Client.CreatePersonInRoom(input.RoomCode);
                return new Person(input.RoomCode, personId);
            }
            throw new NotImplementedException();
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
