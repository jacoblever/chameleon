using System.Collections.Generic;

namespace DataStore
{
    public class Room
    {
        private DynamoModel _dynamoModel = new DynamoModel();

        public Room() { }

        internal Room(DynamoModel dynamoModel)
        {
            _dynamoModel = dynamoModel;
        }

        private Dictionary<string, string> CharacterByPersonId
        {
            get => _dynamoModel.CharacterByPersonId;
            set => _dynamoModel.CharacterByPersonId = value;
        }

        public string RoomCode
        {
            get => _dynamoModel.RoomCode;
            set => _dynamoModel.RoomCode = value;
        }

        public IReadOnlyCollection<string> PersonIds
        {
            get
            {
                return CharacterByPersonId.Keys;
            }
        }

        public void AddPerson(string personId)
        {
            CharacterByPersonId.Add(personId, null);
        }

        public void SetCharacter(string personId, string character)
        {
            CharacterByPersonId[personId] = character;
        }

        public string GetCharacterFor(string personId)
        {
            return CharacterByPersonId[personId];
        }

        internal DynamoModel GetDynamoModel()
        {
            return _dynamoModel;
        }

        internal class DynamoModel
        {
            internal Dictionary<string, string> CharacterByPersonId { get; set; } = new Dictionary<string, string>();
            internal string RoomCode { get; set; }
        }
    }
}
