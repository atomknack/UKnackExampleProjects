using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class NetworkManagerCallbacks
{
    public static Action<NetworkConnectionToClient> OnServerWhenClientConnect;
    public static Action<NetworkConnectionToClient> OnServerWhenClientDisconnect;
}

public class NetworkManagerWithCallbacks : NetworkManager
{
    [SerializeField]
    private UnityEvent _rightBeforeClientStop;

    public override void OnStopClient()
    {
        _rightBeforeClientStop?.Invoke();
        base.OnStopClient();
    }


    /// <summary>Called on the server when a new client connects.</summary>
    public override void OnServerConnect(NetworkConnectionToClient conn) 
    {
        base.OnServerDisconnect(conn);
        NetworkManagerCallbacks.OnServerWhenClientConnect?.Invoke(conn);
    }

    /// <summary>Called on the server when a client disconnects.</summary>
    // Called by NetworkServer.OnTransportDisconnect!
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        NetworkManagerCallbacks.OnServerWhenClientDisconnect?.Invoke(conn);
    }
}
