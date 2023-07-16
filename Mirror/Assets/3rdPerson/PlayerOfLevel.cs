using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOfLevel : SubjectOfLevel
{
    [SerializeField]
    private UnityEvent _onClientPlayerOutsideOfLevel;

    public override void WentOutside()
    {
        _alreadyKnow = true;
        Debug.Log($" WentOutside called {isServer}");
        if (!isServer)
            return;
        RpcPlayerWentOutsideOfLevel();
    }

    [TargetRpc]
    private void RpcPlayerWentOutsideOfLevel()
    {
        Debug.Log(" RpcPlayerWentOutsideOfLevel called");
        _onClientPlayerOutsideOfLevel?.Invoke();
        CommandAllDone();
    }

    [Command]
    private void CommandAllDone(NetworkConnectionToClient sender = null) 
    {
        Debug.Log(" CommandAllDone called");
        base.WentOutside();
    }
}
