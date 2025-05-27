namespace Services.Hubs;

public class BattleSingleton
{
   
    private readonly Dictionary<Guid, bool> _readyClients = new();
    private readonly object _lock = new();

    private readonly HashSet<Guid> RoomsWrittenToHistory = new();

    public bool CheckRoom(Guid roomId)
    {
        lock (_lock)
        {
            return RoomsWrittenToHistory.Contains(roomId);
        }
    }

    public void AddRoomToHistory(Guid roomId)
    {
        lock (_lock)
        {
            RoomsWrittenToHistory.Add(roomId); // HashSet автоматично не додає дублікати
            if (RoomsWrittenToHistory.Count > 1000)
            {
                // Якщо хочете обмежити обʼєм — реалізуйте очищення старих записів іншим способом
            }
        }
    }
    
    public void Add(Guid connectionId)
    {
        lock (_lock)
        {
            _readyClients.Add(connectionId, false);
        }
    }
    public void SetReady(Guid connectionId)
    {
        lock (_lock)
        {
            _readyClients[connectionId] = true;
        }
    }

    public bool IsReady(Guid? connectionId)
    {
        lock (_lock)
        {
            if (connectionId == null) return false;
            return _readyClients.TryGetValue((Guid)connectionId, out var ready) && ready ;
        }
    }
    
    public void Remove(Guid connectionId)
    {
        lock (_lock)
        {
            _readyClients.Remove(connectionId);
        }
    }
}