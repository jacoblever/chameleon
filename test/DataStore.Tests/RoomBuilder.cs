namespace DataStore.Tests
{
    public class RoomBuilder
    {
        private readonly Room _room = new Room();
        public RoomBuilder WithPlayer(string id, string character, string votedFor = null)
        {
            _room.AddPerson(id, id);
            _room.SetCharacter(id, character);
            if (votedFor != null)
            {
                _room.SetVotedFor(id, votedFor);
            }
            return this;
        }

        public Room Build()
        {
            return _room;
        }
    }
}