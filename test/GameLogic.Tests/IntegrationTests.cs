using DataStore;
using NUnit.Framework;

namespace GameLogic.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public void CanPlayGame()
        {
            var chameleonGame = ChameleonGame.Create(RoomStore.Create(new FakeDynamoTable()));

            // Creating and joining the room
            var personAndRoom = chameleonGame.CreateRoom("Anita");
            var roomCode = personAndRoom.RoomCode;
            
            var anita = personAndRoom.PersonId;
            var jacob = chameleonGame.JoinRoom(roomCode, "Jacob").PersonId;
            var paul = chameleonGame.JoinRoom(roomCode, "Paul").PersonId;
            
            // Getting the status
            var status = chameleonGame.GetRoomStatus(roomCode, anita);
            Assert.That(status.Name, Is.EqualTo("Anita"));
            Assert.That(status.PeopleCount, Is.EqualTo(3));
            Assert.That(status.State, Is.EqualTo(RoomState.PreGame.ToString()));
            Assert.That(status.Character, Is.Null);
            Assert.That(status.FirstPersonName, Is.Null);
            
            // Starting a game
            chameleonGame.StartGame(roomCode, jacob);
            
            var inGameStatus = chameleonGame.GetRoomStatus(roomCode, paul);
            Assert.That(inGameStatus.State, Is.EqualTo(RoomState.InGame.ToString()));
            Assert.That(inGameStatus.Character, Is.Not.Null);
            Assert.That(inGameStatus.FirstPersonName, Is.AnyOf("Anita", "Jacob", "Paul"));
            
            // Leaving a room
            chameleonGame.LeaveRoom(roomCode, paul);
            
            var afterLeavingStatus = chameleonGame.GetRoomStatus(roomCode, anita);
            Assert.That(afterLeavingStatus.PeopleCount, Is.EqualTo(2));
            
            // A new player joining a room with an existing game
            var anna = chameleonGame.JoinRoom(roomCode, "Anna").PersonId;

            var newPlayerStatus = chameleonGame.GetRoomStatus(roomCode, anna);
            Assert.That(newPlayerStatus.State, Is.EqualTo(RoomState.PreGame.ToString()));
            Assert.That(newPlayerStatus.Character, Is.Null);
            Assert.That(newPlayerStatus.FirstPersonName, Is.Null);
        }
    }
}
