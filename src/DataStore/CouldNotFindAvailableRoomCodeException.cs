using System;

namespace DataStore
{
    public class CouldNotFindAvailableRoomCodeException : Exception
    {
        public CouldNotFindAvailableRoomCodeException(int attempts)
            : base($"Failed to generate unique room code after {attempts} attempts")
        {
        }
    }
}
