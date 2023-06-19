using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOfLevel : SubjectOfLevel
{
    [SerializeField]
    private UnityEvent playerOutsideOfLevel;
    public override void WentOutside()
    {
        base.WentOutside();
        playerOutsideOfLevel?.Invoke();
    }
}
