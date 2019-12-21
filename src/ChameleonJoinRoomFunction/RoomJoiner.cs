using GameLogic;
using Newtonsoft.Json.Linq;

namespace ChameleonJoinRoomFunction
{
    public class RoomJoiner
    {
        private readonly IChameleonGame _chameleonGame;

        public RoomJoiner(IChameleonGame chameleonGame)
        {
            _chameleonGame = chameleonGame;
        }

        public Person Join(string requestBody)
        {
            var body = JObject.Parse(requestBody);
            var roomCode = body["RoomCode"];
            var personName = body["PersonName"];
            
            var roomAndPerson = roomCode == null
                ? _chameleonGame.CreateRoom(personName.ToString())
                : _chameleonGame.JoinRoom(roomCode.ToString(), personName.ToString());
            return new Person(roomAndPerson.RoomCode, roomAndPerson.PersonId);
        }
    }
}
