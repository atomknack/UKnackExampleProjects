using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UKnack;
using UKnack.Attributes;
using UKnack.Events;
using UnityEngine;
using UnityEngine.Events;

public class RunEventWhenAllClientsReady : RunWhenAllClientsReadyAbstract
{
    [SerializeField]
    [ValidReference]
    private SOEvent _clientReadyOnClientSide;

    [SerializeField]
    private UnityEvent _serverSideEventWhenAllReady;
    private void ReadyOnClient()
    {
        CmdOnServer();
    }
    protected override void OnClientInitialize()
    {
        if (_clientReadyOnClientSide ==  null) 
            throw new System.ArgumentNullException(nameof(_clientReadyOnClientSide));
        _clientReadyOnClientSide.Subscribe(ReadyOnClient);
    }

    protected override void OnClientPrepareForDestroy()
    {
        _clientReadyOnClientSide.UnsubscribeNullSafe(ReadyOnClient);
    }

    protected override void RunCommandOnServerWhenAllReady()
    {
        _serverSideEventWhenAllReady?.Invoke();
    }
}
