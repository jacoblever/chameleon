using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace DataStore.Tests
{
    public class RandomTests
    {
        [Test]
        public void TestGenerateRoomCode()
        {
            var random = new Random();
            
            var roomCode = random.GenerateRoomCode();
            
            Assert.That(roomCode.Length, Is.EqualTo(4));
        }
    }
}
