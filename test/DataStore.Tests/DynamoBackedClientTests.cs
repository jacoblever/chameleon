using System.Linq;
using NUnit.Framework;

namespace DataStore.Tests
{
    public class DynamoBackedClientTests
    {
        [SetUp]
        public void SetUp() => Client.UseDynamoLocal = true;

        [TearDown]
        public void TearDown() => Client.UseDynamoLocal = false;

        [Test]
        public void TestCanSaveRoom()
        {
            var room = Client.CreateRoom();
            var personAdded = Client.CreatePersonInRoom(room.RoomCode);
            
            var roomFromDb = Client.GetRoom(room.RoomCode);
            Assert.That(roomFromDb.PersonIds.Count, Is.EqualTo(1));
            Assert.That(roomFromDb.PersonIds.First(), Is.EqualTo(personAdded));
        }
        
        [Test]
        public void TestGetNonExistentRoom()
        {
            var roomFromDb = Client.GetRoom("hello");
            Assert.That(roomFromDb, Is.Null);
        }
    }
}
