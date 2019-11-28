namespace GameLogic
{
    public interface IChameleonGame
    {
        RoomAndPerson CreateRoom();
        RoomAndPerson JoinRoom(string roomCode);
        RoomStatus GetRoomStatus(string roomCode, string personId);
        void StartGame(string roomCode, string personId);
    }
}
