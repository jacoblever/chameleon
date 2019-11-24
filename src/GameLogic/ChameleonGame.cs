using System;
using System.Linq;
using DataStore;

namespace GameLogic
{
    public class ChameleonGame : IChameleonGame
    {
        private readonly IRoomStore _roomStore;
        
        // TODO: Move this to some dependency manager
        public static IChameleonGame Create()
        {
            return new ChameleonGame();
        }
        
        private ChameleonGame(IRoomStore roomStore = null)
        {
            _roomStore = roomStore ?? RoomStore.Create();
        }

        public RoomAndPerson JoinOrCreateRoom(string roomCode)
        {
            if (roomCode == null)
            {
                var room = _roomStore.CreateRoom();
                var personId = _roomStore.CreatePersonInRoom(room.RoomCode);
                return new RoomAndPerson(room.RoomCode, personId);
            }
            if (_roomStore.DoesRoomExist(roomCode))
            {
                var personId = _roomStore.CreatePersonInRoom(roomCode);
                return new RoomAndPerson(roomCode, personId);
            }
            throw new NotImplementedException();
        }

        public RoomStatus GetRoomStatus(string roomCode, string personId)
        {
            var room = _roomStore.GetRoom(roomCode);
            EnsurePersonInRoom(roomCode, personId, room);

            var peopleCount = room.PersonIds.Count;
            var myCharacter = room.GetCharacterFor(personId);

            return new RoomStatus(
                code: roomCode,
                peopleCount: peopleCount,
                chameleonCount: GetChameleonCount(peopleCount),
                state: myCharacter == null ? RoomState.PreGame.ToString() : RoomState.InGame.ToString(),
                character: myCharacter);
        }

        public void StartGame(string roomCode, string personId)
        {
            var room = _roomStore.GetRoom(roomCode);
            EnsurePersonInRoom(roomCode, personId, room);
            
            _roomStore.StartGame(roomCode);
        }

        private static void EnsurePersonInRoom(string roomCode, string personId, Room room)
        {
            if (!room.PersonIds.Contains(personId))
            {
                throw new PersonNotInRoomException(
                    $"Person {personId} not in room {roomCode}, people in room {string.Join(',', room.PersonIds)}");
            }
        }

        private static int GetChameleonCount(int peopleCount) => (int)Math.Ceiling((double)(peopleCount / 6));
    }
}
