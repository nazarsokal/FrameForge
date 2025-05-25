namespace Services.Hubs;

public class BattleSingleton
{
   
    private readonly Dictionary<Guid, bool> _readyClients = new();
    private readonly Queue<Guid> RoomsWrittenToHistory = new();
    private readonly object _lock = new();

    public bool CheckRoom(Guid roomId)
    {
        lock (_lock)
        {
            if (RoomsWrittenToHistory.Contains(roomId)) return false;
            else return true;
        }
    }
    public void AddRoomToHistory(Guid roomId)
    {
        lock (_lock)
        {
            RoomsWrittenToHistory.Enqueue(roomId);
            if (RoomsWrittenToHistory.Count > 10) RoomsWrittenToHistory.Dequeue();
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