using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UKnack.Events;
using UnityEngine;
using UnityEngine.Events;

public class SOEventIntCondition : MonoBehaviour
{
    [SerializeField]
    [ValidReference]
    private SOEvent<int> _soEvent;

    [SerializeField]
    private int _biggerThan;

    [SerializeField]
    private UnityEvent<int> _whenBiggerThan;

    private void Conditions(int value)
    {
        if(value > _biggerThan)
            _whenBiggerThan?.Invoke(value);
    }

    private void OnEnable()
    {
        if (_soEvent == null)
            throw new System.ArgumentNullException(nameof(_soEvent));
        _soEvent.Subscribe(Conditions);
    }

    private void OnDisable()
    {
        _soEvent.UnsubscribeNullSafe(Conditions);
    }
}
