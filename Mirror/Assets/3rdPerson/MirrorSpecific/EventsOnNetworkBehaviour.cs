using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsOnNetworkBehaviour : NetworkBehaviour
{
    [SerializeField]
    private UnityEvent _onStartServer;

    [SerializeField]
    private UnityEvent _onStopServer;

    /// <summary>Like Start(), but only called on server and host.</summary>
    public override void OnStartServer() 
    {
        _onStartServer?.Invoke();
    }

    /// <summary>Stop event, only called on server and host.</summary>
    public override void OnStopServer() 
    {
        _onStopServer?.Invoke();
    }
}
