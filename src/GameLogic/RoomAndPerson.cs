namespace GameLogic
{
    public struct RoomAndPerson
    {
        public RoomAndPerson(string roomCode, string personId)
        {
            RoomCode = roomCode;
            PersonId = personId;
        }

        public string RoomCode { get; }
        public string PersonId { get; }
    }
}
