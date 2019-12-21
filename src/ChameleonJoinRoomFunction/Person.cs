namespace ChameleonJoinRoomFunction
{
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
