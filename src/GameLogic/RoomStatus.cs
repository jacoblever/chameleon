using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GameLogic
{
    public class RoomStatus
    {
        public RoomStatus(
            string code,
            string name,
            int peopleCount,
            int chameleonCount,
            string state,
            string character,
            string firstPersonName)
        {
            Code = code;
            Name = name;
            PeopleCount = peopleCount;
            ChameleonCount = chameleonCount;
            State = state;
            Character = character;
            FirstPersonName = firstPersonName;
            TimeToPollMillisecond = 5000;
        }

        public string Name { get; set; }
        public string Code { get; }
        public int PeopleCount { get; }
        public int ChameleonCount { get; }
        public string State { get; }
        public string Character { get; }
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
    }
}
