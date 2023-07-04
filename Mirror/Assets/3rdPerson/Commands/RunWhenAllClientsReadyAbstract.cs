using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UKnack;
using UnityEngine;

public abstract class RunWhenAllClientsReadyAbstract : NetworkBehaviour
{
    
    protected Dictionary<int, bool> _clientsReady = new Dictionary<int, bool>();

    private bool _ServerRunning = false;
    private bool _ClientRunning = false;
    
    protected abstract void RunCommandOnServerWhenAllReady();
    protected abstract void OnClientInitialize();
    protected abstract void OnClientPrepareForDestroy();


    [Command(requiresAuthority = false)]
    protected void CmdOnServer(NetworkConnectionToClient sender = null)
    {
        if (sender == null)
            throw new System.NullReferenceException(nameof(sender));
        Debug.Log($"CmdOnServer called from {sender.connectionId}");

        _clientsReady[sender.connectionId] = true;

        TryRunCommand();

    }

    public virtual void OnServerWhenClientConnect(NetworkConnectionToClient conn)
    {
        _clientsReady[conn.connectionId] = false;
    }

    public virtual void OnServerWhenClientDisconnect(NetworkConnectionToClient conn)
    {
        _clientsReady.Remove(conn.connectionId);
        TryRunCommand();
    }

    protected virtual bool TryRunCommand()
    {
        //Debug.Log("no exception");
        if (_clientsReady.Count > 0)
        {
            //Debug.Log($"still no exception {_clientsReady.Count}");
            if (_clientsReady.All(pair => pair.Value == true) )
            {
                Debug.Log($"{_clientsReady.Count} all are true");

                foreach (var pair in _clientsReady.ToArray()) //baby error
                {
                    //Debug.Log($"still no exception for {pair.Key} {pair.Value}");
                    _clientsReady[pair.Key] = false;
                }
                RunCommandOnServerWhenAllReady();
                //Debug.Log($"no exception before returnning true");
                return true;
            }
        }
        //Debug.Log($"no exception before returnning false");
        return false;
    }

    public override void OnStartServer()
    {
        NetworkManagerCallbacks.OnServerWhenClientConnect += OnServerWhenClientConnect;
        NetworkManagerCallbacks.OnServerWhenClientDisconnect += OnServerWhenClientDisconnect;
        foreach (var key in NetworkServer.connections.Keys)
        {
            _clientsReady[key] = false;
        }
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
        OnClientInitialize();
        _ClientRunning = true;
    }

    public override void OnStopClient()
    {
        if (_ClientRunning == false)
            return;

        OnClientPrepareForDestroy();

        _ClientRunning = false;
    }

    public void OnDisable() 
    {
        OnStopServer();
        OnStopClient();
    }


}
