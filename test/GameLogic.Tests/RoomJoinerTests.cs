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
        
        [Test]
        public void TestCanCreateRoomWithNullCode()
        {
            var chameleonGame = new Mock<IChameleonGame>();
            var roomJoiner = new RoomJoiner(chameleonGame.Object);

            const string requestBody = "{\"PersonName\": \"Anita\", \"RoomCode\": null}";
            
            roomJoiner.Join(requestBody);
            
            chameleonGame.Verify(x => x.CreateRoom("Anita"), Times.Once);
        }
        
        [Test]
        public void TestErrorsIfRoomCodeIsEmpty()
        {
            var chameleonGame = new Mock<IChameleonGame>();
            var roomJoiner = new RoomJoiner(chameleonGame.Object);

            const string requestBody = "{\"PersonName\": \"Anita\", \"RoomCode\": \"\"}";
            
            Assert.Throws<RoomCodeMustBeValidException>(() => { roomJoiner.Join(requestBody); });
            
            chameleonGame.Verify(x => x.CreateRoom(It.IsAny<string>()), Times.Never);
            chameleonGame.Verify(x => x.JoinRoom(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
        
        [Test]
        public void TestErrorsIfPersonNameIsEmpty()
        {
            var chameleonGame = new Mock<IChameleonGame>();
            var roomJoiner = new RoomJoiner(chameleonGame.Object);

            const string requestBody = "{\"PersonName\": \"\", \"RoomCode\": \"ABCD\"}";
            
            Assert.Throws<PersonNameMustBeSpecifiedException>(() => { roomJoiner.Join(requestBody); });
            
            chameleonGame.Verify(x => x.CreateRoom(It.IsAny<string>()), Times.Never);
            chameleonGame.Verify(x => x.JoinRoom(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
