using System.Linq;
using Moq;
using NUnit.Framework;

namespace DataStore.Tests
{
    public class RoomStoreTests
    {
        [Test]
        public void TestCanStartGame()
        {
            const string roomCode = "AAAA";
            
            var room = new Room();
            room.AddPerson("person-1");
            room.AddPerson("person-2");
            room.AddPerson("person-3");
            var dynamoTable = new Mock<IDynamoTable>();
            dynamoTable.Setup(foo => foo.GetRoom(roomCode)).Returns(room);
            
            var roomStore = RoomStore.Create(dynamoTable.Object);
            
            roomStore.StartGame(roomCode);
            
            var expectedRoom = new Room();
            expectedRoom.AddPerson("person-1");
            expectedRoom.SetCharacter("person-1", "Pizza");
            expectedRoom.AddPerson("person-2");
            expectedRoom.SetCharacter("person-2", "Pizza");
            expectedRoom.AddPerson("person-3");
            expectedRoom.SetCharacter("person-3", "Pizza");
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
