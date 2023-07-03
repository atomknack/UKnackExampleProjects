using Mirror;
using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UKnack.KeyValues;
using UKnack.Values;
using UnityEngine;

public class Collector : NetworkBehaviour
{
    [SerializeField]
    [ValidReference]
    private SOValueMutable<int> _collectedValue;

    [SerializeField]
    [ValidReference]
    private SOKeyValueMutable<int, int> _collectedDictionary_netId_amount;

    [SyncVar(hook = nameof(OnCliendGetNewCollectedValue))]
    private int _collected;

    private int _identityNetId;

    private void OnCollisionEnter(Collision hit)
    {
        if (isServer == false)
            return;

        var other = hit.gameObject;
        if (other == null)
            return;

        var collectibleGold = other.GetComponent<CollectibleGold>();
        //Debug.Log(collectibleGold);
        if (collectibleGold != null && collectibleGold.TryCollect(out int worth, transform))
        {
            //_collectedValue.SetValue(_collectedValue.GetValue() + worth);
            _collected += worth;

            _collectedDictionary_netId_amount[_identityNetId] = _collected;
            Debug.Log($"id {_identityNetId} changed value to {_collectedDictionary_netId_amount[_identityNetId]}");
        }



        if (_collected > 15)
            NetworkManager.singleton.ServerChangeScene("Level2");
    }

    private void OnCliendGetNewCollectedValue(int oldCollected, int newCollected)
    {
        if (isOwned == false)  //probably bug in Mirror, for now this will do. if hook will activates in other not owned scripts, probably need to do bug report.
            return;

        //Debug.Log($"Hook called for {_identityNetId}, s: {isServer} {isServerOnly} c: {isClient} {isClientOnly} o: {isOwned} lp: {isLocalPlayer}");
        _collectedValue.SetValue(newCollected);

    }

    public override void OnStartServer()
    {
        //Debug.Log($"OnStartServer: {isServer} {isServerOnly} {isClient} {isClientOnly} {isOwned} {isLocalPlayer}");
        if (_collectedDictionary_netId_amount == null)
            throw new System.ArgumentNullException(nameof(_collectedDictionary_netId_amount));

        _identityNetId = (int)netIdentity.netId;
    }

    public override void OnStartClient()
    {
        //Debug.Log($"OnStartClient: {isServer} {isServerOnly} {isClient} {isClientOnly} {isOwned} {isLocalPlayer}");
        if (_collectedValue == null)
            throw new System.ArgumentNullException(nameof(_collectedValue));
    }
}
