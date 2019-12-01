using NUnit.Framework;

namespace DataStore.Tests
{
    public class RandomTests
    {
        [Test]
        public void TestGenerateRoomCode()
        {
            var randomRoomCode = new RandomRoomCode();
            
            var roomCode = randomRoomCode.Generate();
            
            Assert.That(roomCode.Length, Is.EqualTo(4));
        }
    }
}
