namespace GameLogic
{
    public interface IChameleonGame
    {
        RoomAndPerson CreateRoom(string personName);
        RoomAndPerson JoinRoom(string roomCode, string personName);
        RoomStatus GetRoomStatus(string roomCode, string personId);
        void StartGame(string roomCode, string personId);
        void Vote(string roomCode, string personId, string vote);
        void LeaveRoom(string roomCode, string personId);
    }
}
