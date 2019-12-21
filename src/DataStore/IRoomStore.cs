using System.Collections.Generic;

namespace DataStore
{
    public interface IRoomStore
    {
        Room CreateRoom();
        bool DoesRoomExist(string roomCode);
        string CreatePersonInRoom(string roomCode, string personName);
        void StartGame(string roomCode, string word, IEnumerable<string> chameleons);
        Room GetRoom(string roomCode);
        void RemovePersonFromRoom(string roomCode, string personId);
    }
}
