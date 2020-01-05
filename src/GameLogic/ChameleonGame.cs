using System;
using System.Collections.Generic;
using System.Linq;
using DataStore;

namespace GameLogic
{
    public class ChameleonGame : IChameleonGame
    {
        private readonly IRoomStore _roomStore;
        
        // TODO: Move this to some dependency manager
        public static IChameleonGame Create(IRoomStore roomStore = null)
        {
            return new ChameleonGame(roomStore);
        }
        
        private ChameleonGame(IRoomStore roomStore)
        {
            _roomStore = roomStore ?? RoomStore.Create();
        }

        public RoomAndPerson CreateRoom(string personName)
        {
            var room = _roomStore.CreateRoom();
            var personId = _roomStore.CreatePersonInRoom(room.RoomCode, personName);
            return new RoomAndPerson(room.RoomCode, personId);
        }

        public RoomAndPerson JoinRoom(string roomCode, string personName)
        {
            if (!_roomStore.DoesRoomExist(roomCode))
            {
                throw new RoomDoesNotExistException(roomCode);
            }

            var personId = _roomStore.CreatePersonInRoom(roomCode, personName);
            return new RoomAndPerson(roomCode, personId);
        }

        public RoomStatus GetRoomStatus(string roomCode, string personId)
        {
            var room = _roomStore.GetRoom(roomCode);
            if (room == null)
            {
                throw new RoomDoesNotExistException(roomCode);
            }
            ThrowUnlessPersonInRoom(personId, room);

            var name = room.GetNameFor(personId);
            var peopleCount = room.PersonIds.Count;
            var myCharacter = room.GetCharacterFor(personId);
            var votesByPerson = room.PersonIds
                .Where(x => room.GetVotedFor(x) != null)   
                .GroupBy(x => room.GetVotedFor(x))
                .ToDictionary(x => x.Key, x => x.Count());
            var people = room.PersonIds
                .Select(x =>
                {
                    votesByPerson.TryGetValue(x, out var votes);
                    return new RoomStatus.Person(x, room.GetNameFor(x), votes);
                })
                .ToList();
            var everyoneVoted = room.PersonIds.All(x => room.GetVotedFor(x) != null || room.GetCharacterFor(x) == "chameleon");

            return new RoomStatus(
                code: roomCode,
                name: name,
                peopleInRoom: people,
                peopleCount: peopleCount,
                chameleonCount: GetChameleonCount(peopleCount),
                state: myCharacter == null ? RoomState.PreGame.ToString() : RoomState.InGame.ToString(),
                character: myCharacter,
                everyoneVoted: everyoneVoted,
                showStartGameButton: room.OldestPersonId == personId,
                firstPersonName: myCharacter == null ? null : room.WhoGoesFirstByName());
        }

        public void StartGame(string roomCode, string personId)
        {
            var room = _roomStore.GetRoom(roomCode);
            ThrowUnlessPersonInRoom(personId, room);
            var word = new Words().GetRandomWord();

            var random = new Random();
            var chameleons = room.PersonIds.OrderBy(x => random.Next())
                .ToArray()
                .Take(GetChameleonCount(room.PersonIds.Count))
                .ToHashSet();

            var goesFirst = PickFirstPlayer(room.PersonIds, chameleons);
            _roomStore.StartGame(roomCode, word, chameleons, goesFirst);
        }

        private static string PickFirstPlayer(IReadOnlyCollection<string> personIds, ISet<string> chameleons)
        {
            var weightedPersonIds = new List<string>();
            
            foreach (var personId in personIds)
            {
                var weight = chameleons.Contains(personId) ? 1 : 7;
                for (var i = 0; i < weight; i++)
                {
                    weightedPersonIds.Add(personId);
                }
            }
            
            var randomIndex = new Random().Next(weightedPersonIds.Count);
            return weightedPersonIds[randomIndex];
        }

        public void Vote(string roomCode, string personId, string vote)
        {
            var room = _roomStore.GetRoom(roomCode);
            ThrowUnlessPersonInRoom(personId, room);
            if (room.GetCharacterFor(personId) == "chameleon")
            {
                throw new ChameleonCannotVoteException();
            }
            _roomStore.Vote(roomCode, personId, vote);
        }

        public void LeaveRoom(string roomCode, string personId)
        {
            var room = _roomStore.GetRoom(roomCode);
            ThrowUnlessPersonInRoom(personId, room);
            _roomStore.RemovePersonFromRoom(roomCode, personId);
        }

        private static void ThrowUnlessPersonInRoom(string personId, Room room)
        {
            if (!room.PersonIds.Contains(personId))
            {
                throw new PersonNotInRoomException(
                    $"Person {personId} not in room {room.RoomCode}, people in room {string.Join(',', room.PersonIds)}");
            }
        }

        private static int GetChameleonCount(int peopleCount) => (int)Math.Ceiling((double)peopleCount / 6);
    }
}
