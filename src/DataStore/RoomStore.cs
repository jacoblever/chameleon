using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStore
{
    public class RoomStore : IRoomStore
    {
        private readonly IDynamoTable _dynamoTable;

        // TODO: Move this to some dependency manager
        public static IRoomStore Create(IDynamoTable dynamoTable = null)
        {
            return new RoomStore(dynamoTable);
        }
        
        private RoomStore(IDynamoTable dynamoTable = null)
        {
            _dynamoTable = dynamoTable ?? new DynamoTable();
        }

        public Room CreateRoom()
        {
            var roomCode = "ABCD";
            var room = new Room
            {
                RoomCode = roomCode
            };
            _dynamoTable.SaveRoom(room);
            return room;
        }

        public bool DoesRoomExist(string roomCode)
        {
            var room = GetRoom(roomCode);
            return room != null;
        }
        
        public string CreatePersonInRoom(string roomCode)
        {
            var room = GetRoom(roomCode);
            var personId = Guid.NewGuid().ToString();
            room.AddPerson(personId);
            _dynamoTable.SaveRoom(room);
            return personId;
        }

        public void StartGame(string roomCode, string word, IEnumerable<string> chameleons)
        {
            chameleons = chameleons.ToList();
            var room = GetRoom(roomCode);
            foreach (var personId in room.PersonIds.Select(x => x).ToList())
            {
                room.SetCharacter(personId,
                    chameleons.Contains(personId)
                        ? "chameleon"
                        : word);
            }
            _dynamoTable.SaveRoom(room);
        }

        public Room GetRoom(string roomCode)
        {
            return _dynamoTable.GetRoom(roomCode);
        }
    }
}
