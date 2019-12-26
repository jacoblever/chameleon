using System.Collections.Generic;
using Newtonsoft.Json;

namespace DataStore
{
    public class FakeDynamoTable : IDynamoTable
    {
        private readonly Dictionary<string, string> _rooms = new Dictionary<string, string>();

        public Room GetRoom(string roomCode)
        {
            if (!_rooms.TryGetValue(roomCode, out var roomJson))
            {
                return null;
            }

            var dynamoModel = JsonConvert.DeserializeObject<Room.DynamoModel>(roomJson);
            return new Room(dynamoModel);
        }

        public void SaveRoom(Room room)
        {
            var roomJson = JsonConvert.SerializeObject(room.GetDynamoModel());
            _rooms[room.RoomCode] = roomJson;
        }
    }
}
