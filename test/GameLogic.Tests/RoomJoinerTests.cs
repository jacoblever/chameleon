using ChameleonJoinRoomFunction;
using Moq;
using NUnit.Framework;

namespace GameLogic.Tests
{
    public class RoomJoinerTests
    {
        [Test]
        public void TestCanCreateRoom()
        {
            var chameleonGame = new Mock<IChameleonGame>();
            var roomJoiner = new RoomJoiner(chameleonGame.Object);

            const string requestBody = "{\"PersonName\": \"Anita\"}";
            
            roomJoiner.Join(requestBody);
            
            chameleonGame.Verify(x => x.CreateRoom("Anita"), Times.Once);
        }
        
        [Test]
        public void TestCanJoinRoom()
        {
            var chameleonGame = new Mock<IChameleonGame>();
            var roomJoiner = new RoomJoiner(chameleonGame.Object);

            const string requestBody = "{\"PersonName\": \"Anita\", \"RoomCode\": \"ABCD\"}";
            
            roomJoiner.Join(requestBody);
            
            chameleonGame.Verify(x => x.JoinRoom("ABCD", "Anita"), Times.Once);
        }
    }
}
