using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UKnack.Mirror;

public class SenderArraySegmentToClients : NetworkBehaviour
{
    private byte[] _data = new byte[500_000_000];
    private uint _dataCount = 0;
    private int _maxArraySegmentSize = 100;

    protected Dictionary<int, uint> _clientsRecievedDataCount = new Dictionary<int, uint>();

    [Command(requiresAuthority = false)]
    protected void CmdClientRecievedTotal(uint clientDataCount, NetworkConnectionToClient sender = null)
    {
        Debug.Log($"MyCommandLOG: CmdClientRecievedTotal called");
        Debug.Log($"MyCommandLOG: CmdClientRecievedTotal called by {sender}");
        Debug.Log($"MyCommandLOG: CmdClientRecievedTotal sender id {sender.connectionId} with data count {clientDataCount}");

        if (sender == null)
            throw new System.NullReferenceException($"{nameof(sender)} should be generated by Mirror and never be null");

        uint wasConfirmedCount = _clientsRecievedDataCount.GetValueOrDefault(sender.connectionId, 0u);
        if (wasConfirmedCount != clientDataCount)
        {
            if(wasConfirmedCount != 0 || clientDataCount!=0)
                throw new System.Exception($"Client before had already {wasConfirmedCount} but new number is {clientDataCount}");
        }
        if (clientDataCount > _dataCount)
            throw new System.Exception($"Client can never have more data {clientDataCount} than server {_dataCount}");

        _clientsRecievedDataCount[sender.connectionId] = clientDataCount;

        if (clientDataCount < _dataCount)
            CheckIfNeedToSendMore(sender, clientDataCount);
    }


    private void CheckIfNeedToSendMore(NetworkConnectionToClient sender, long lastCount)
    {
        var expecting = _clientsRecievedDataCount[sender.connectionId] != lastCount;
        if (expecting)
            throw new System.Exception($"client confirmed total {lastCount}, but server expecting it to have {expecting}");
        if (lastCount < _dataCount)
            SendDataToClient(sender);
    }

    protected void SendDataToClient(NetworkConnectionToClient target)
    {
        Debug.Log($"SendDataToClient called from {target.connectionId}, serverData {_dataCount}, recieved {_clientsRecievedDataCount[target.connectionId]}, maxSegmentSize {_maxArraySegmentSize}");

        uint targetDataSended = _clientsRecievedDataCount[target.connectionId];
        if (targetDataSended > _dataCount)
            throw new System.Exception("this should never happen");
        if (targetDataSended == _dataCount)
            return;

        int needToSend = Math.Min((int)_dataCount - (int)targetDataSended, _maxArraySegmentSize);
        if (needToSend <= 0)
            throw new System.Exception($"this should never happen {needToSend}, check {_maxArraySegmentSize}");

        Debug.Log($"will now call RpcSendedDataToClient {target.connectionId}, sended {(int)targetDataSended}, {needToSend}, {targetDataSended}");
        TargetSendedDataToClient(target, new ArraySegment<byte>(_data, (int)targetDataSended, needToSend), targetDataSended);
        _clientsRecievedDataCount[target.connectionId] = targetDataSended + (uint)needToSend;
    }

    [TargetRpc]
    protected void TargetSendedDataToClient(NetworkConnectionToClient target, ArraySegment<byte> transfer, uint currentClientCountShouldBe)
    {
        Debug.Log($"RpcSendedDataToClient called id null: {target==null}, arraysegment null: {transfer==null}");
        //Debug.Log($"RpcSendedDataToClient called from {target}");
        //Debug.Log($"RpcSendedDataToClient called from id {target.connectionId}, with {transfer}");
        Debug.Log($"RpcSendedDataToClient {_dataCount} and new transfer is: {transfer.Count}, checker: {currentClientCountShouldBe}");
        Debug.Log($"{transfer[0]} {transfer.Count} {transfer.Array.Length}");

        if (currentClientCountShouldBe != _dataCount)
            throw new System.Exception($"client have {_dataCount}, and server expecting it to have {currentClientCountShouldBe}, there is tear somewhere");
        int start = (int)_dataCount;
        for (int i = 0; i< transfer.Count; ++i)
        {
            int dataIndex = start + i;
            _data[dataIndex] = transfer[i];
        }
        _dataCount = (uint)(start + transfer.Count);
        CmdClientRecievedTotal(_dataCount);
    }



    protected void OnServerWhenClientConnect(NetworkConnectionToClient conn)
    {
        Debug.Log($"Client {conn.connectionId} connected to server");
        if (IsConnectionFromHost(conn))
        {
            Debug.Log($"No need to send data to itself for {conn.connectionId}");
            return;
        }
        ZeroClient(conn);
        //SendDataToClient(conn);
    }

    private void ZeroClient(NetworkConnectionToClient conn)
    {
        if (IsConnectionFromHost(conn))
            throw new System.Exception("This method should not be ever called from host");

        if (_clientsRecievedDataCount.ContainsKey(conn.connectionId))
            throw new System.Exception($"there are already {conn.connectionId}");

        _clientsRecievedDataCount[conn.connectionId] = 0;
    }

    protected void OnServerWhenClientDisconnect(NetworkConnectionToClient conn)
    {
        _clientsRecievedDataCount.Remove(conn.connectionId);
    }

    public override void OnStartServer()
    {
        NetworkManagerCallbacks.OnServerWhenClientConnect += OnServerWhenClientConnect;
        NetworkManagerCallbacks.OnServerWhenClientDisconnect += OnServerWhenClientDisconnect;
        
        _maxArraySegmentSize = GetMaxArraySegmentSize();
        //_serverRunning = true;

        FillDataArray();
    }

    public override void OnStartClient()
    {
        if (isServer == false)
            _maxArraySegmentSize = GetMaxArraySegmentSize();
        if (isServer == false)
        {
            _dataCount = 0;
            CmdClientRecievedTotal(_dataCount);
        }
    }

    public override void OnStopClient()
    {
        for (int i=0; i< _dataCount; ++i)
        {
            if (_data[i] != (byte)i)
            {
                string message = $"at {i} of {_dataCount}, {_data[i]} not equal to {(byte)i}";
                Debug.LogError("logged Error:" + message);
                throw new Exception(message);
            }
        }
        Debug.Log($"Checked {_dataCount}");
    }

    public override void OnStopServer()
    {
        //if (_serverRunning == false)
        //    return;

        NetworkManagerCallbacks.OnServerWhenClientConnect -= OnServerWhenClientConnect;
        NetworkManagerCallbacks.OnServerWhenClientDisconnect -= OnServerWhenClientDisconnect;
        
        //_serverRunning = false;
    }
    private int GetMaxArraySegmentSize() =>
        Math.Max(100, NetworkManager.singleton.transport.GetMaxPacketSize() - 64);
    
    private bool IsConnectionFromHost(NetworkConnectionToClient conn)
    {
        if (conn == null)
            throw new System.ArgumentNullException($"are you trying that null connection is local? Why?");

        LocalConnectionToClient local = NetworkServer.localConnection;
        if (local != null)
        {
            if (conn == local)
            {
                Debug.Log($"Just so you know connection: {conn.connectionId} is local: {local.connectionId}");
                return true;
            }
        }
        return false;
    }

    private void FillDataArray()
    {
        for (int i =0;  i < _data.Length; i++)
        {
            _data[i] = (byte)i;
        }
        _dataCount = (uint)_data.Length;
    }
}

/*
private void SendMoreDataForClient(NetworkConnectionToClient client, uint clientDataCount)
{
    uint needToSendUnchunked = _dataCount - clientDataCount;
    if (needToSendUnchunked == 0)
    {
        throw new System.Exception($"Why this method was called for {client.connectionId}, data difference is 0");
        //return;
    }

    if (needToSendUnchunked < 0)
        throw new System.Exception($"for client: {client.connectionId} there is negative number of data that need to be sent: {needToSendUnchunked}, this should never happen");
    //NetworkServer.
    //client.
    throw new NotImplementedException();
}
*/