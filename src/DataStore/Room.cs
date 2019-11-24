using System.Collections.Generic;

namespace DataStore
{
    public class Room
    {
        private readonly DynamoModel _dynamoModel = new DynamoModel();

        public Room() { }

        internal Room(DynamoModel dynamoModel)
        {
            _dynamoModel = dynamoModel;
        }

        public string RoomCode
        {
            get => _dynamoModel.RoomCode;
            set => _dynamoModel.RoomCode = value;
        }

        public IReadOnlyCollection<string> PersonIds => _dynamoModel.CharacterByPersonId.Keys;

        public void AddPerson(string personId)
        {
            _dynamoModel.CharacterByPersonId.Add(personId, null);
        }

        public void SetCharacter(string personId, string character)
        {
            _dynamoModel.CharacterByPersonId[personId] = character;
        }

        public string GetCharacterFor(string personId)
        {
            return _dynamoModel.CharacterByPersonId[personId];
        }

        internal DynamoModel GetDynamoModel()
        {
            return _dynamoModel;
        }

        internal class DynamoModel
        {
            public string RoomCode { get; set; }
            public Dictionary<string, string> CharacterByPersonId { get; set; } = new Dictionary<string, string>();
        }
    }
}
