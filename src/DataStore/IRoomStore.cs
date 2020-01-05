using System.Collections.Generic;

namespace DataStore
{
    public interface IRoomStore
    {
        Room CreateRoom();
        bool DoesRoomExist(string roomCode);
        string CreatePersonInRoom(string roomCode, string personName);
        void StartGame(string roomCode, string word, ISet<string> chameleons, string firstPersonId);
        Room GetRoom(string roomCode);
        void RemovePersonFromRoom(string roomCode, string personId);
        void Vote(string roomCode, string personId, string vote);
    }
}
