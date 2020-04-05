using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;

namespace DataStore
{
    public class RoomStore : IRoomStore
    {
        private readonly IDynamoTable _dynamoTable;
        private readonly IRandomRoomCode _randomRoomCode;

        // TODO: Move this to some dependency manager
        public static IRoomStore Create(IDynamoTable dynamoTable = null, IRandomRoomCode randomRoomCode = null)
        {
            return new RoomStore(dynamoTable, randomRoomCode);
        }
        
        private RoomStore(IDynamoTable dynamoTable, IRandomRoomCode randomRoomCode)
        {
            _dynamoTable = dynamoTable ?? new DynamoTable();
            _randomRoomCode = randomRoomCode ?? new RandomRoomCode();
        }

        public Room CreateRoom()
        {
            var roomCode = FindAvailableRoomCode();
            var room = new Room
            {
                RoomCode = roomCode
            };
            Save(room);
            return room;
        }

        private void Save(Room room)
        {
            const int nineHours = 9 * 60 * 60;
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();
            room.LastModified = now;
            room.TimeToLive = now + nineHours;
            _dynamoTable.SaveRoom(room);
        }

        private string FindAvailableRoomCode()
        {
            const int maximumAttempts = 10;
            for (var i = 0; i < maximumAttempts; i++)
            {
                var roomCode = _randomRoomCode.Generate();
                if (!DoesRoomExist(roomCode))
                {
                    return roomCode;
                }
            }

            throw new CouldNotFindAvailableRoomCodeException(maximumAttempts);
        }

        public bool DoesRoomExist(string roomCode)
        {
            var room = GetRoom(roomCode);
            return room != null;
        }
        
        public string CreatePersonInRoom(string roomCode, string personName)
        {
            var room = GetRoom(roomCode);
            ThrowIfPersonNameNotUnique(room, personName);
            var personId = Guid.NewGuid().ToString();
            room.AddPerson(personId, personName);
            Save(room);
            return personId;
        }

        private static void ThrowIfPersonNameNotUnique(Room room, string personName)
        {
            var nameClash = room.PersonIds
                .Any(x => string.Equals(
                    room.GetNameFor(x),
                    personName,
                    StringComparison.InvariantCultureIgnoreCase));
            if (nameClash)
            {
                throw new PersonNameNotUniqueException(personName);
            }
        }

        public void StartGame(string roomCode, string word, ISet<string> chameleons, string firstPersonId)
        {
            var room = GetRoom(roomCode);
            foreach (var id in room.PersonIds)
            {
                room.SetVotedFor(id, null);
            }
            foreach (var personId in room.PersonIds.Select(x => x).ToList())
            {
                room.SetCharacter(personId,
                    chameleons.Contains(personId)
                        ? "chameleon"
                        : word);
            }
            room.SetGoesFirst(firstPersonId);
            Save(room);
        }

        public Room GetRoom(string roomCode)
        {
            return _dynamoTable.GetRoom(roomCode);
        }

        public void RemovePersonFromRoom(string roomCode, string personId)
        {
            var room = GetRoom(roomCode);
            room.RemovePerson(personId);
            Save(room);
        }

        public void Vote(string roomCode, string personId, string vote)
        {
            var room = GetRoom(roomCode);
            room.SetVotedFor(personId, vote);
            int numberLeftToVote = room.PersonIds
                .Where(x => room.GetCharacterFor(x) != "chameleon")
                .Count(x => room.GetVotedFor(x) == null);
            if (numberLeftToVote == 0)
            {
                Score(room);
            }
            Save(room);
        }

        
        private void Score(Room room)
        {
            var myScorer = new Scorer(room);
            Dictionary<string, int> scoreByPersonId = myScorer.CalculateScore();

            foreach (var id in scoreByPersonId.Keys)
            {
                room.AddScore(id, scoreByPersonId[id]);
            }
        }
    }
}
