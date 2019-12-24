using System;
using System.Linq;
using NUnit.Framework;

namespace DataStore.Tests
{
    [TestFixture]
    [Category("DynamoTests")]
    public class DynamoBackedClientTests
    {
        private const string DynamoDbTableNameEnvVar = "DYNAMO_DB_TABLE_NAME";
        private const string DynamoLocalTableName = "ChameleonData";

        [SetUp]
        public void SetUp()
            => Environment.SetEnvironmentVariable(DynamoDbTableNameEnvVar, DynamoLocalTableName);

        [TearDown]
        public void TearDown()
            => Environment.SetEnvironmentVariable(DynamoDbTableNameEnvVar, null);

        [Test]
        public void TestCanSaveRoom()
        {
            var roomStore = RoomStore.Create(new DynamoTable(true));
            var room = roomStore.CreateRoom();
            var personAdded = roomStore.CreatePersonInRoom(room.RoomCode, "Jacob");
            
            var roomFromDb = roomStore.GetRoom(room.RoomCode);
            Assert.That(roomFromDb.PersonIds.Count, Is.EqualTo(1));
            Assert.That(roomFromDb.PersonIds.First(), Is.EqualTo(personAdded));
        }
        
        [Test]
        public void TestGetNonExistentRoom()
        {
            var roomStore = RoomStore.Create(new DynamoTable(true));
            var roomFromDb = roomStore.GetRoom("not-a-room");
            Assert.That(roomFromDb, Is.Null);
        }
    }
}
