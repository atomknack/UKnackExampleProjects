using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleGold : MonoBehaviour
{
    [SerializeField]
    private int _worth = 1;

    private bool collected = false;

    public bool TryCollect(out int worth)
    {
        if (collected)
        {
            worth = 0;
            return false;
        }

        worth = _worth;
        collected = true;
        Destroy(gameObject);
        return true;
    }
}
