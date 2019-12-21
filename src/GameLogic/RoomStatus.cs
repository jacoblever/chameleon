using System;

namespace GameLogic
{
    public class RoomStatus
    {
        public RoomStatus(string code, string name, int peopleCount, int chameleonCount, string state, string character)
        {
            Code = code;
            Name = name;
            PeopleCount = peopleCount;
            ChameleonCount = chameleonCount;
            State = state;
            Character = character;
            TimeToPollMillisecond = 5000;
        }

        public string Name { get; set; }
        public string Code { get; }
        public int PeopleCount { get; }
        public int ChameleonCount { get; }
        public string State { get; }
        public string Character { get; }
        
        // TODO: This is a very UI thing and so does not belong in Game Logic
        public int TimeToPollMillisecond { get; }

        public override bool Equals(object obj)
        {
            return obj is RoomStatus status &&
                   Code == status.Code &&
                   PeopleCount == status.PeopleCount &&
                   ChameleonCount == status.ChameleonCount &&
                   State == status.State &&
                   Character == status.Character &&
                   TimeToPollMillisecond == status.TimeToPollMillisecond;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Code, PeopleCount, ChameleonCount, State, Character, TimeToPollMillisecond);
        }
    }
}
