using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class EventsOnNetworkBehaviour : NetworkBehaviour
{
    [SerializeField]
    private UnityEvent _onStartServer;
    /// <summary>Like Start(), but only called on server and host.</summary>
    public override void OnStartServer() => _onStartServer?.Invoke();

    [SerializeField]
    private UnityEvent _onStopServer;
    /// <summary>Stop event, only called on server and host.</summary>
    public override void OnStopServer() => _onStopServer?.Invoke();
    
}
