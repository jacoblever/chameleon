using System;

namespace ChameleonJoinRoomFunction
{
    public class PersonNameMustBeSpecifiedException : Exception
    {
        public PersonNameMustBeSpecifiedException()
            : base("We need your name to add you to the room")
        {
        }
    }
}
