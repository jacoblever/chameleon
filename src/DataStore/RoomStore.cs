using System;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;

namespace DataStore
{
    public class RoomStore : IRoomStore
    {
        private readonly DynamoTable _dynamoTable;

        // TODO: Move this to some dependency manager
        public static IRoomStore Create(DynamoTable dynamoTable = null)
        {
            return new RoomStore(dynamoTable);
        }
        
        private RoomStore(DynamoTable dynamoTable = null)
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
            SaveRoom(room);
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
            SaveRoom(room);
            return personId;
        }

        public void StartGame(string roomCode)
        {
            var room = GetRoom(roomCode);
            foreach (var personId in room.PersonIds)
            {
                room.SetCharacter(personId, "Pizza");
            }
            SaveRoom(room);
        }

        public Room GetRoom(string roomCode)
        {
            var table = _dynamoTable.GetTable();
            var getTask = table.GetItemAsync(roomCode);
            getTask.Wait();
            var doc = getTask.Result;
            if (doc == null)
            {
                return null;
            }
            var roomJson = doc.ToJson();
            var dynamoModel = JsonConvert.DeserializeObject<Room.DynamoModel>(roomJson);
            return new Room(dynamoModel);
        }
        
        private string SaveRoom(Room room)
        {
            var table = _dynamoTable.GetTable();
            var roomJson = JsonConvert.SerializeObject(room.GetDynamoModel());
            Document doc = Document.FromJson(roomJson);
            Task putItem = table.PutItemAsync(doc);
            putItem.Wait();
            return "Done!";
        }
    }
}
