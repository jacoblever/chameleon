using System.Text.RegularExpressions;
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
            var roomCodeToken = body["RoomCode"];
            var personNameToken = body["PersonName"];
            var personName = personNameToken?.ToString().Trim() ?? "";
            if (personName.Length == 0)
            {
                throw new PersonNameMustBeSpecifiedException();
            }

            RoomAndPerson roomAndPerson;
            if (roomCodeToken?.Value<string>() == null)
            {
                roomAndPerson = _chameleonGame.CreateRoom(personName);
            }
            else
            {
                var roomCode = roomCodeToken.ToString().Trim();
                if (!Regex.IsMatch(roomCode, "[A-Z]{4}"))
                {
                    throw new RoomCodeMustBeValidException();
                }
                roomAndPerson = _chameleonGame.JoinRoom(roomCode, personName);
            }
            return new Person(roomAndPerson.RoomCode, roomAndPerson.PersonId);
        }
    }
}
