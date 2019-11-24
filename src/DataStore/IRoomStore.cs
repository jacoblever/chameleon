namespace DataStore
{
    public interface IRoomStore
    {
        Room CreateRoom();
        bool DoesRoomExist(string roomCode);
        string CreatePersonInRoom(string roomCode);
        void StartGame(string roomCode);
        Room GetRoom(string roomCode);
    }
}
