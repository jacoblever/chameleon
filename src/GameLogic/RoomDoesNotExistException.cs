using System;

namespace GameLogic
{
    public class RoomDoesNotExistException : Exception
    {
        public RoomDoesNotExistException(string roomCode) : base($"Room '{roomCode}' does not exist")
        {
        }
    }
}
