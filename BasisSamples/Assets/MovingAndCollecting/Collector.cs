using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UKnack.Values;
using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField]
    [ValidReference]
    private SOValueMutable<int> _collectedValue;

    private void OnCollisionEnter(Collision hit)
    {
        var other = hit.gameObject;
        if (other == null)
            return;



        var collectibleGold = other.GetComponent<CollectibleGold>();
        //Debug.Log(collectibleGold);
        if (collectibleGold != null && collectibleGold.TryCollect(out int worth))
        {
            _collectedValue.SetValue(_collectedValue.GetValue() + worth);
        }    
    }

    private void Awake()
    {
        if (_collectedValue == null)
            throw new System.ArgumentNullException(nameof(_collectedValue));
    }
}
