﻿using System.Collections.Generic;
using System.Linq;

namespace DataStore
{
    public class Room
    {
        private readonly DynamoModel _dynamoModel = new DynamoModel();

        public Room()
        {
        }

        internal Room(DynamoModel dynamoModel)
        {
            _dynamoModel = dynamoModel;
        }

        public string RoomCode
        {
            get => _dynamoModel.RoomCode;
            set => _dynamoModel.RoomCode = value;
        }

        public IReadOnlyCollection<string> PersonIds => _dynamoModel.PersonByPersonId.Keys;

        public string GetNameFor(string personId)
        {
            return _dynamoModel.PersonByPersonId[personId].Name;
        }

        public void AddPerson(string personId, string personName)
        {
            _dynamoModel.PersonByPersonId.Add(personId, new DynamoModel.Person {Name = personName, Character = null});
        }

        public void RemovePerson(string personId)
        {
            _dynamoModel.PersonByPersonId.Remove(personId);
        }

        public void SetCharacter(string personId, string character)
        {
            _dynamoModel.PersonByPersonId[personId].Character = character;
        }

        public string GetCharacterFor(string personId)
        {
            return _dynamoModel.PersonByPersonId[personId].Character;
        }

        public void SetGoesFirst(string personId)
        {
            foreach (var id in _dynamoModel.PersonByPersonId.Keys)
            {
                _dynamoModel.PersonByPersonId[personId].GoesFirst = id == personId;
            }
        }

        public string WhoGoesFirst()
        {
            return _dynamoModel.PersonByPersonId.Single(x => x.Value.GoesFirst).Key;
        }

        internal DynamoModel GetDynamoModel()
        {
            return _dynamoModel;
        }

        internal class DynamoModel
        {
            public string RoomCode { get; set; }
            public Dictionary<string, Person> PersonByPersonId { get; set; } = new Dictionary<string, Person>();

            internal class Person
            {
                public string Name { get; set; }
                public string Character { get; set; }
                public bool GoesFirst { get; set; }
            }
        }
    }
}
