using System.Collections.Generic;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace DataStore
{
    public class DynamoTable
    {
        private readonly bool _useLocal;

        public DynamoTable(bool useLocal = false)
        {
            _useLocal = useLocal;
        }
        
        public Table GetTable()
        {
            var client = CreateClient();
            return GetOrCreateTable(client);
        }
        
        private Table GetOrCreateTable(AmazonDynamoDBClient client)
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
