using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UKnack;
using UnityEngine;
using UnityEngine.Events;

public class RunWhenAllClientsReady : NetworkBehaviour, ICommand
{
    [SerializeField]
    private UnityEvent _whenAllReady;

    Dictionary<int, bool> _clientsReady = new Dictionary<int, bool>();
    public string Description => nameof(RunWhenAllClientsReady);

    private bool _ServerRunning = false;
    private bool _ClientRunning = false;

    public void Execute()
    {
        ExecuteOnServer();
    }

    [Command(requiresAuthority = false)]
    public void ExecuteOnServer(NetworkConnectionToClient sender = null)
    {
        if (sender == null)
            throw new System.NullReferenceException(nameof(sender));

        _clientsReady[sender.connectionId] = true;
        TryRunCommand();
    }

    public void OnServerWhenClientConnect(NetworkConnectionToClient conn)
    {
        _clientsReady[conn.connectionId] = false;
    }

    public void OnServerWhenClientDisconnect(NetworkConnectionToClient conn)
    {
        _clientsReady.Remove(conn.connectionId);
        TryRunCommand();
    }

    private bool TryRunCommand()
    {
        if (_clientsReady.Count > 0)
        {
            if (_clientsReady.All(pair => pair.Value == true) )
            {
                foreach (var pair in _clientsReady)
                {
                    _clientsReady[pair.Key] = false;
                }
                return true;
            }
        }
        return false;
    }

    public override void OnStartServer()
    {
        NetworkManagerCallbacks.OnServerWhenClientConnect += OnServerWhenClientConnect;
        NetworkManagerCallbacks.OnServerWhenClientDisconnect += OnServerWhenClientDisconnect;
        _ServerRunning = true;
    }

    public override void OnStopServer() 
    {
        if (_ServerRunning == false)
            return;

        NetworkManagerCallbacks.OnServerWhenClientConnect -= OnServerWhenClientConnect;
        NetworkManagerCallbacks.OnServerWhenClientDisconnect -= OnServerWhenClientDisconnect;
        _ServerRunning = false;
    }

    public override void OnStartClient()
    {
        throw new System.NotImplementedException();
        _ClientRunning = true;
    }
    public override void OnStopClient()
    {
        if (_ClientRunning == false)
            return;

        throw new System.NotImplementedException();

        _ClientRunning = false;
    }
    public void OnDisable() 
    {
        OnStopServer();
    }


}
