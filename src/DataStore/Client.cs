using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;

namespace DataStore
{
    public class Client
    {
        public static bool UseDynamoLocal = false;
        
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
            room.AddPerson(personId);
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
            var client = CreateClient();
            var table = GetOrCreateTable(client);
            var roomJson = JsonConvert.SerializeObject(room.GetDynamoModel());
            Document doc = Document.FromJson(roomJson);
            Task putItem = table.PutItemAsync(doc);
            putItem.Wait();
            return "Done!";
        }

        private static Table GetOrCreateTable(AmazonDynamoDBClient client)
        {
            if (!Table.TryLoadTable(client, "ChameleonData", out var table))
            {
                var task = client.CreateTableAsync("ChameleonData", new List<KeySchemaElement>
                    {
                        new KeySchemaElement("RoomCode", KeyType.HASH),
                    },
                    new List<AttributeDefinition>
                    {
                        new AttributeDefinition("RoomCode", ScalarAttributeType.S)
                    },
                    new ProvisionedThroughput(1, 1)
                );
                task.Wait();
                table = Table.LoadTable(client, "ChameleonData");
            }

            return table;
        }

        public static void StartGame(string roomCode)
        {
            var room = GetRoom(roomCode);
            foreach (var personId in room.PersonIds)
            {
                room.SetCharacter(personId, "Pizza");
            }
            SaveRoom(room);
        }

        public static Room GetRoom(string roomCode)
        {
            var client = CreateClient();
            var table = GetOrCreateTable(client);
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

        private static AmazonDynamoDBClient CreateClient()
        {
            var clientConfig = new AmazonDynamoDBConfig();
            if (UseDynamoLocal)
            {
                clientConfig.ServiceURL = "http://localhost:8000";
            }
            else
            {
                clientConfig.RegionEndpoint = RegionEndpoint.EUWest1;
            }
            return new AmazonDynamoDBClient(clientConfig);
        }
    }
}
