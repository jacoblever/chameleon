using System.Collections.Generic;
using System.Linq;
using DataStore;
using Moq;
using NUnit.Framework;

namespace GameLogic.Tests
{
    public class ChameleonGameTests
    {
        [Test]
        public void TestCanStartGame()
        {
            const string roomCode = "AAAA";
            
            var mockRoomStore = new Mock<IRoomStore>();
            var chameleonGame = ChameleonGame.Create(mockRoomStore.Object);

            var room = new Room();
            room.AddPerson("person-1");
            room.AddPerson("person-2");
            room.AddPerson("person-3");

            mockRoomStore.Setup(x => x.GetRoom(roomCode)).Returns(room);
            
            chameleonGame.StartGame(roomCode, "person-1");
            
            mockRoomStore.Verify(x => x.StartGame(
                roomCode, 
                It.IsAny<string>(), 
                It.Is<IEnumerable<string>>(y => y.Count() == 1)));
        }
    }
}
