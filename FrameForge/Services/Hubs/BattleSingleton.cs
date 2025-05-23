namespace Services.Hubs;

public class BattleSingleton
{
   
    private readonly Dictionary<string, bool> _readyClients = new();
    private readonly object _lock = new();
    public void Add(string connectionId)
    {
        lock (_lock)
        {
            _readyClients.Add(connectionId, false);
        }
    }
    public void SetReady(string connectionId)
    {
        lock (_lock)
        {
            _readyClients[connectionId] = true;
        }
    }

    public bool IsReady(string connectionId)
    {
        lock (_lock)
        {
            return _readyClients.TryGetValue(connectionId, out var ready) && ready;
        }
    }

    public void Remove(string connectionId)
    {
        lock (_lock)
        {
            _readyClients.Remove(connectionId);
        }
    }


}