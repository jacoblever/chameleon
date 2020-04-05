using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GameLogic
{
    public class RoomStatus
    {
        public RoomStatus(string code,
            string name,
            IReadOnlyCollection<Person> peopleInRoom,
            int peopleCount,
            int chameleonCount,
            string state,
            string character,
            bool everyoneVoted,
            bool showStartGameButton,
            string firstPersonName)
        {
            Code = code;
            Name = name;
            PeopleInRoom = peopleInRoom;
            PeopleCount = peopleCount;
            ChameleonCount = chameleonCount;
            State = state;
            Character = character;
            EveryoneVoted = everyoneVoted;
            ShowStartGameButton = showStartGameButton;
            FirstPersonName = firstPersonName;
            TimeToPollMillisecond = new Random().Next(600, 1000);
        }

        public string Code { get; }
        public string Name { get; }
        public IReadOnlyCollection<Person> PeopleInRoom { get; }
        public int PeopleCount { get; }
        public int ChameleonCount { get; }
        public string State { get; }
        public string Character { get; }
        public bool EveryoneVoted { get; }
        public bool ShowStartGameButton { get; }
        public string FirstPersonName { get; }

        // TODO: This is a very UI thing and so does not belong in Game Logic
        public int TimeToPollMillisecond { get; }
        public string Hash => GetMd5Hash();

        private string GetMd5Hash()
        {
            var fields = string.Concat(
                Code,
                PeopleCount.ToString(),
                ChameleonCount.ToString(),
                State,
                Character,
                TimeToPollMillisecond.ToString()
            );
            var inputBytes = Encoding.ASCII.GetBytes(fields);
            return MD5.Create()
                .ComputeHash(inputBytes)
                .Select(x => x.ToString("X2"))
                .Concat();
        }

        public class Person
        {
            public Person(string id, string name, int votes, int score)
            {
                Id = id;
                Name = name;
                Votes = votes;
                Score = score;
            }

            public string Id { get; }
            public string Name { get; }
            public int Votes { get; }
            public int Score { get; }
        }
    }
}
