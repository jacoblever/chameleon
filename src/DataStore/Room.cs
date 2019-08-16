using System;
using System.Collections.Generic;

namespace DataStore
{
    public class Room
    {
        private Dictionary<string, string> CharacterByPersonId { get; set; } = new Dictionary<string, string>();
        public string RoomCode { get; set; }

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
    }
}
