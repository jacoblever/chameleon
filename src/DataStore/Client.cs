using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;

namespace DataStore
{
    public class Client
    {
        public static Room CreateRoom()
        {
            var roomCode = "ABCD";
            var room = new Room()
            {
                RoomCode = roomCode
            };
            SaveRoom(room);
            return room;
        }

        public static string CreatePersonInRoom(string roomCode)
        {
            var room = GetRoom(roomCode);
            var personId = Guid.NewGuid().ToString();
            room.PersonIds.Add(personId);
            SaveRoom(room);
            return personId;
        }

        public static bool DoesRoomExist(string roomCode)
        {
            var room = GetRoom(roomCode);
            return room != null;
        }

        private static string SaveRoom(Room room)
        {
            try
            {
                var client = CreateClient();
                var table = Table.LoadTable(client, "ChameleonData");
                var roomJson = JsonConvert.SerializeObject(room);
                Document doc = Document.FromJson(roomJson);
                Task putItem = table.PutItemAsync(doc);
                putItem.Wait();
                return "Done!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static Room GetRoom(string roomCode)
        {
            var client = CreateClient();
            var table = Table.LoadTable(client, "ChameleonData");
            var getTask = table.GetItemAsync(roomCode);
            getTask.Wait();
            var doc = getTask.Result;
            var roomJson = doc.ToJson();
            var room = JsonConvert.DeserializeObject<Room>(roomJson);
            return room;
        }

        private static AmazonDynamoDBClient CreateClient()
        {
            AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
            clientConfig.RegionEndpoint = RegionEndpoint.EUWest1;
            return new AmazonDynamoDBClient();
        }
    }
}
