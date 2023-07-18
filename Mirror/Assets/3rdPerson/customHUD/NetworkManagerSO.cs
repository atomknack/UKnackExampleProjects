using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UKnack.Common;
using UnityEngine;

[Obsolete("not tested")]
[CreateAssetMenu(fileName = "NetworkManagerSO", menuName = "UKnack/Mirror/NetworkManagerSO", order = 990)]
public class NetworkManagerSO : ScriptableObjectWithReadOnlyName
{
    public void StartHost() => NetworkManager.singleton.StartHost();
    public void StarServer() => NetworkManager.singleton.StartServer();
    public void StartClient() => NetworkManager.singleton.StartClient();

    public void StopHost() => NetworkManager.singleton.StopHost();
    public void StopServer() => NetworkManager.singleton.StopServer();
    public void StopClient() => NetworkManager.singleton.StopClient();

    public void StopRunningHostOrServerOrClient()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
            return;
        }

        if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
            return;
        }

        if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
            return;
        }
    }
}
