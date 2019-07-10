using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

namespace DataStore
{
    public class Client
    {
        public static string TestSave()
        {
            try
            {
                var client = CreateClient();
                var table = Table.LoadTable(client, "ChameleonData");
                Document doc = Document.FromJson("{\"RoomCode\": \"ABCD\", \"test\": \"Hello Jacob\"}");
                Task putItem = table.PutItemAsync(doc);
                putItem.Wait();
                return "Done!";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string TestGet()
        {
            try
            {
                var client = CreateClient();
                var table = Table.LoadTable(client, "ChameleonData");
                var getTask = table.GetItemAsync("ABCD");
                getTask.Wait();
                var doc = getTask.Result;
                return doc["test"].AsString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private static AmazonDynamoDBClient CreateClient()
        {
            AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
            clientConfig.RegionEndpoint = RegionEndpoint.EUWest1;
            return new AmazonDynamoDBClient();
        }
    }
}
