namespace GameLogic
{
    public interface IChameleonGame
    {
        RoomAndPerson JoinOrCreateRoom(string roomCode);
        RoomStatus GetRoomStatus(string roomCode, string personId);
        void StartGame(string roomCode, string personId);
    }
}
