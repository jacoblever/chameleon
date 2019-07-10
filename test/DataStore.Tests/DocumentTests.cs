using Amazon.DynamoDBv2.DocumentModel;
using NUnit.Framework;

namespace Tests
{
    public class DocumentTests
    {
        [Test]
        public void TestCanGetValueFromDocument()
        {
			Document doc = Document.FromJson("{\"RoomCode\": \"ABCD\", \"test\": \"Hello Jacob\"}");
			Assert.That(doc["test"].AsString(), Is.EqualTo("Hello Jacob"));
        }
    }
}
