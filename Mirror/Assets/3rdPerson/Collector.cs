using Mirror;
using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UKnack.Values;
using UnityEngine;

public class Collector : NetworkBehaviour
{
    [SerializeField]
    [ValidReference]
    private SOValueMutable<int> _collectedValue;

    [SyncVar(hook = nameof(OnCliendGetNewCollectedValue))]
    private int _collected;

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
        }  
        
        if (_collected > 15)
            NetworkManager.singleton.ServerChangeScene("Level2");
    }

    private void OnCliendGetNewCollectedValue(int oldCollected, int newCollected)
    {
        _collectedValue.SetValue(newCollected);

    }

    private void OnEnable()
    {
        if (isLocalPlayer == false)
            return;

        if (_collectedValue == null)
            throw new System.ArgumentNullException(nameof(_collectedValue));
    }
}
