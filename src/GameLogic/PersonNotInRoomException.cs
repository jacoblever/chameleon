using System;

namespace GameLogic
{
    public class PersonNotInRoomException : Exception
    {
        public PersonNotInRoomException(string message) : base(message)
        {
        }
    }
}
