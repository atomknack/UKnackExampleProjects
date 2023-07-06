using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

//https://mirror-networking.gitbook.io/docs/manual/guides/communications/networkbehaviour-callbacks
public class RunEventOnNetworkBehaviourFull : NetworkBehaviour
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
    public override void OnStartClient() => _onStartClient?.Invoke();

    [SerializeField]
    private UnityEvent _onStopClient;
    public override void OnStopClient() => _onStopClient?.Invoke();

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
