using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace DataStore.Tests
{
    public class RoomStoreTests
    {
        [Test]
        public void TestCanCreateRoom()
        {
            var existingRoom = new Room{RoomCode = "ABCD"};
            var dynamoTable = new Mock<IDynamoTable>();
            dynamoTable.Setup(foo => foo.GetRoom(existingRoom.RoomCode)).Returns(existingRoom);
            
            var randomRoomCode = new Mock<IRandomRoomCode>();
            randomRoomCode.Setup(x => x.Generate())
                .Returns("WXYZ");

            var roomStore = RoomStore.Create(dynamoTable.Object, randomRoomCode.Object);

            roomStore.CreateRoom();

            dynamoTable.Verify(x => x.SaveRoom(It.Is<Room>(y => y.RoomCode == "WXYZ")), Times.Once);
        }
        
        [Test]
        public void TestCanCreateRoomWhenRandomClashesWithExistingRoom()
        {
            var existingRoom = new Room{RoomCode = "ABCD"};
            var dynamoTable = new Mock<IDynamoTable>();
            dynamoTable.Setup(foo => foo.GetRoom(existingRoom.RoomCode)).Returns(existingRoom);
            
            var randomRoomCode = new Mock<IRandomRoomCode>();
            randomRoomCode.SetupSequence(x => x.Generate())
                .Returns(existingRoom.RoomCode)
                .Returns("WXYZ");

            var roomStore = RoomStore.Create(dynamoTable.Object, randomRoomCode.Object);

            roomStore.CreateRoom();

            dynamoTable.Verify(x => x.SaveRoom(It.Is<Room>(y => y.RoomCode == "WXYZ")), Times.Once);
        }
        
        [Test]
        public void TestCreateRoomDoesNotCauseInfiniteLoop()
        {
            var existingRoom = new Room{RoomCode = "ABCD"};
            var dynamoTable = new Mock<IDynamoTable>();
            dynamoTable.Setup(foo => foo.GetRoom(existingRoom.RoomCode)).Returns(existingRoom);
            
            var randomRoomCode = new Mock<IRandomRoomCode>();
            randomRoomCode.Setup(x => x.Generate())
                .Returns(existingRoom.RoomCode);

            var roomStore = RoomStore.Create(dynamoTable.Object, randomRoomCode.Object);

            Assert.Throws<CouldNotFindAvailableRoomCodeException>(() => roomStore.CreateRoom());
            dynamoTable.Verify(
                x => x.SaveRoom(It.IsAny<Room>()),
                Times.Never);
        }
    
        [Test]
        public void TestCanStartGame()
        {
            const string roomCode = "AAAA";
            const string word = "Thing";
            
            var room = new Room();
            room.AddPerson("person-1");
            room.AddPerson("person-2");
            room.AddPerson("person-3");
            var dynamoTable = new Mock<IDynamoTable>();
            dynamoTable.Setup(foo => foo.GetRoom(roomCode)).Returns(room);
            
            var roomStore = RoomStore.Create(dynamoTable.Object);

            roomStore.StartGame(roomCode, word, new List<string> {"person-1"});
            
            var expectedRoom = new Room();
            expectedRoom.AddPerson("person-1");
            expectedRoom.SetCharacter("person-1", "chameleon");
            expectedRoom.AddPerson("person-2");
            expectedRoom.SetCharacter("person-2", word);
            expectedRoom.AddPerson("person-3");
            expectedRoom.SetCharacter("person-3", word);
            dynamoTable.Verify(x => x.SaveRoom(It.Is<Room>(y => y.IsTheSameAs(expectedRoom))), Times.Once);
        }
    }

    internal static class RoomTestExtensions
    {
        public static bool IsTheSameAs(this Room me, Room other)
        {
            return me.RoomCode == other.RoomCode
                   && me.PersonIds.SequenceEqual(other.PersonIds)
                   && me.PersonIds.All(personId => me.GetCharacterFor(personId) == other.GetCharacterFor(personId));
        }
    }
}
