using System.Collections.Generic;

namespace DataStore
{
    public class Room
    {
        public string RoomCode { get; set; }
        public List<string> PersonIds { get; set; } = new List<string>();
    }
}
