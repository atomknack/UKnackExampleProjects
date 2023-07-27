using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UKnack;
using UnityEngine;

namespace UKnack.Mirror
{

    public abstract class RunWhenAllClientsReadyAbstract : ClientServerCommandAbstract
    {

        protected Dictionary<int, bool> _clientsReady = new Dictionary<int, bool>();

        protected abstract void RunCommandOnServerWhenAllReady();

        protected override void CommandCodeOnServer(NetworkConnectionToClient sender)
        {
            _clientsReady[sender.connectionId] = true;

            TryRunCommand();

        }

        protected override void OnServerWhenClientConnect(NetworkConnectionToClient conn)
        {
            _clientsReady[conn.connectionId] = false;
        }

        protected override void OnServerWhenClientDisconnect(NetworkConnectionToClient conn)
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
                if (_clientsReady.All(pair => pair.Value == true))
                {
                    //Debug.Log($"{_clientsReady.Count} all are true");

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
            foreach (var key in NetworkServer.connections.Keys)
            {
                _clientsReady[key] = false;
            }
            base.OnStartServer();
        }

    }

}