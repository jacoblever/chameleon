using System;

namespace ChameleonJoinRoomFunction
{
    public class RoomCodeMustBeValidException : Exception
    {
        public RoomCodeMustBeValidException()
            : base("The Room Code must be 4 capital letters")
        {
        }
    }
}
