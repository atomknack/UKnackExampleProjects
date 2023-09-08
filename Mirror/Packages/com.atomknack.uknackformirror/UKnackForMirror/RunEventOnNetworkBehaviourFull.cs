using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;

//https://mirror-networking.gitbook.io/docs/manual/guides/communications/networkbehaviour-callbacks
public sealed class RunEventOnNetworkBehaviourFull : NetworkBehaviour
{
    // server

    [SerializeField]
    private UnityEvent _onStartServer;
    public override void OnStartServer() => _onStartServer?.Invoke();

    [SerializeField]
    private UnityEvent _onStopServer;
    public override void OnStopServer() => _onStopServer?.Invoke();

    // client

    [SerializeField]
    private UnityEvent _onStartClient;
    [SerializeField]
    private UnityEvent _onStartHostClient;
    [SerializeField]
    private UnityEvent _onStartNonHostClient;
    public override void OnStartClient()
    {
        _onStartClient?.Invoke();
        if (isServer)
        {
            _onStartHostClient?.Invoke();
        }
        else
        {
            _onStartNonHostClient?.Invoke();
        }
    }

    [SerializeField]
    private UnityEvent _onStopClient;
    [SerializeField]
    private UnityEvent _onStopHostClient;
    [SerializeField]
    private UnityEvent _onStopNonHostClient;
    public override void OnStopClient()
    {
        _onStopClient?.Invoke();
        if (isServer)
        {
            _onStopHostClient?.Invoke();
        }
        else
        {
            _onStopNonHostClient?.Invoke();
        }
    }

    [SerializeField]
    private UnityEvent _onStartLocalPlayer;
    public override void OnStartLocalPlayer() => _onStartLocalPlayer?.Invoke();

    [SerializeField]
    private UnityEvent _onStopLocalPlayer;
    public override void OnStopLocalPlayer() => _onStopLocalPlayer?.Invoke();

    [SerializeField]
    private UnityEvent _onStartAuthority;
    public override void OnStartAuthority() => _onStartAuthority?.Invoke();

    [SerializeField]
    private UnityEvent _onStopAuthority;
    public override void OnStopAuthority() => _onStopAuthority?.Invoke();
}
