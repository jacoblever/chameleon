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
    public class DynamoTable : IDynamoTable
    {
        private readonly bool _useLocal;

        public DynamoTable(bool useLocal = false)
        {
            _useLocal = useLocal;
        }
        
        public Room GetRoom(string roomCode)
        {
            var table = GetTable();
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

        public string SaveRoom(Room room)
        {
            var table = GetTable();
            var roomJson = JsonConvert.SerializeObject(room.GetDynamoModel());
            Document doc = Document.FromJson(roomJson);
            Task putItem = table.PutItemAsync(doc);
            putItem.Wait();
            return "Done!";
        }
        
        private Table GetTable()
        {
            var client = CreateClient();
            return GetOrCreateTable(client);
        }
        
        private Table GetOrCreateTable(AmazonDynamoDBClient client)
        {
            var tableName = Environment.GetEnvironmentVariable("DYNAMO_DB_TABLE_NAME");
            if (!Table.TryLoadTable(client, tableName, out var table))
            {
                var task = client.CreateTableAsync(tableName, new List<KeySchemaElement>
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
                table = Table.LoadTable(client, tableName);
            }

            return table;
        }
        
        private AmazonDynamoDBClient CreateClient()
        {
            var clientConfig = new AmazonDynamoDBConfig();
            if (_useLocal)
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
