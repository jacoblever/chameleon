namespace DataStore
{
    public interface IDynamoTable
    {
        Room GetRoom(string roomCode);
        void SaveRoom(Room room);
    }
}
