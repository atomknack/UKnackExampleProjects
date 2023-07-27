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
        if (sender == null)
            throw new System.NullReferenceException($"{nameof(sender)} should be generated by Mirror and never be null");

        uint wasConfirmedCount = _clientsRecievedDataCount[sender.connectionId];
        if (wasConfirmedCount >= clientDataCount)
            throw new System.Exception($"Client before had already {wasConfirmedCount} but new number should be bigger {clientDataCount}");
        if (clientDataCount > _dataCount)
            throw new System.Exception($"Client can never have more data {clientDataCount} than server {_dataCount}");

        _clientsRecievedDataCount[sender.connectionId] = clientDataCount;

        if (clientDataCount < _dataCount)
            SendMoreDataForClient(sender, clientDataCount);
    }

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

    [Command(requiresAuthority = false)]
    protected void CmdCliendConfirmedReceivedDataOnServer(long lastCount, NetworkConnectionToClient sender = null)
    {
        Debug.Log($"CmdOnServer called from {sender.connectionId}");

        if (IsConnectionFromHost(sender))
            throw new System.Exception($"Host client({sender.connectionId}) should never call this command");

        if (sender == null)
            throw new System.NullReferenceException($"{nameof(sender)} should be generated by Mirror and never be null");

        CommandCodeOnServer(sender, lastCount);
    }

    private void CommandCodeOnServer(NetworkConnectionToClient sender, long lastCount)
    {
        var expecting = _clientsRecievedDataCount[sender.connectionId] != lastCount;
        if (expecting)
            throw new System.Exception($"client confirmed total {lastCount}, but server expecting it to have {expecting}");
        if (lastCount < _dataCount)
            SendDataToClient(sender);
    }

    protected void SendDataToClient(NetworkConnectionToClient target)
    {
        uint targetDataSended = _clientsRecievedDataCount[target.connectionId];
        if (targetDataSended > _dataCount)
            throw new System.Exception("this should never happen");
        if (targetDataSended == _dataCount)
            return;

        int needToSend = Math.Min((int)_dataCount - (int)targetDataSended, _maxArraySegmentSize);
        if (needToSend <= 0)
            throw new System.Exception($"this should never happen {needToSend}, check {_maxArraySegmentSize}");

        RpcSendedDataToClient(target, new ArraySegment<byte>(_data, (int)targetDataSended, needToSend), targetDataSended);
        _clientsRecievedDataCount[target.connectionId] = targetDataSended + (uint)needToSend;
    }

    [TargetRpc]
    protected void RpcSendedDataToClient(NetworkConnectionToClient target, ArraySegment<byte> transfer, uint currentClientCountShouldBe)
    {
        Debug.Log($"RpcSendedDataToClient called from {target.connectionId}");

        if (currentClientCountShouldBe != _dataCount)
            throw new System.Exception($"client have {_dataCount}, and server expecting it to have {currentClientCountShouldBe}, there is tear somewhere");
        int start = (int)_dataCount;
        for (int i = 0; i< transfer.Count; ++i)
        {
            int dataIndex = start + i;
            _data[dataIndex] = transfer[i];
        }
        _dataCount = (uint)(start + transfer.Count);
        CmdCliendConfirmedReceivedDataOnServer(_dataCount);
    }



    protected void OnServerWhenClientConnect(NetworkConnectionToClient conn)
    {
        if (IsConnectionFromHost(conn))
        {
            Debug.Log("No need to send data to itself");
            return;
        }
        ZeroClient(conn);
        SendDataToClient(conn);
    }

    private void ZeroClient(NetworkConnectionToClient conn)
    {
        if (IsConnectionFromHost(conn))
            throw new System.Exception("This method should not be ever called from host");

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