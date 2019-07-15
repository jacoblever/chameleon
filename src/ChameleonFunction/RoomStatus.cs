﻿using System;

namespace ChameleonFunction
{
    public class RoomStatus
    {
        public RoomStatus(string code, int peopleCount, int chameleonCount, RoomState state, string character)
        {
            Code = code;
            PeopleCount = peopleCount;
            ChameleonCount = chameleonCount;
            State = state;
            Character = character;
            TimeToPollMillisecond = 5000;
        }

        public string Code { get; }
        public int PeopleCount { get; }
        public int ChameleonCount { get; }
        public RoomState State { get; }
        public string Character { get; }
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