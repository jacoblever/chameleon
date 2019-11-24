namespace DataStore
{
    public interface IDynamoTable
    {
        Room GetRoom(string roomCode);
        string SaveRoom(Room room);
    }
}
