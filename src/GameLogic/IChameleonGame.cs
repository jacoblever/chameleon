namespace GameLogic
{
    public interface IChameleonGame
    {
        RoomAndPerson CreateRoom(string personName);
        RoomAndPerson JoinRoom(string roomCode, string personName);
        RoomStatus GetRoomStatus(string roomCode, string personId);
        void StartGame(string roomCode, string personId);
        void LeaveRoom(string roomCode, string personId);
    }
}
