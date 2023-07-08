using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UKnack.Attributes;
using UKnack.KeyValues;
using UKnack.Events;
using UnityEngine.Events;

public sealed class RunServerEventOnCondition : NetworkBehaviour, ISubscriberToEvent<int, int>
{
    public enum Condition
    {
        Equal,
        NotEqual,
        Smaller,
        EqualOrSmaller,
        Bigger,
        EqualOrBigger,
    }

    [SerializeField]
    [ValidReference]
    private SOKeyValue<int, int> _givenKeyValue;

    [SerializeField]
    private Condition _whenAnyValue;

    [SerializeField]
    private int _thanBaseValue;

    [SerializeField]
    private UnityEvent<int, int> _then_InvokeEvent;

    public string Description => nameof(RunServerEventOnCondition);

    public void OnEventNotification(int key, int value)
    {
        if (CheckCondition(_whenAnyValue, _thanBaseValue, value))
            _then_InvokeEvent.Invoke(key, value);
    }

    public override void OnStartServer()
    {
        if (_givenKeyValue == null)
            throw new System.ArgumentNullException(nameof(_givenKeyValue));
        _givenKeyValue.Subscribe(this);
    }
    public override void OnStopServer()
    {
        Debug.Log($"RunServerEventOnCount OnStopServer called");
        _givenKeyValue.UnsubscribeNullSafe(this);
    }

    private static bool CheckCondition(Condition condition, int baseValue, int comparedValue)
    {
        switch (condition)
        {
            case Condition.Equal:
                return comparedValue == baseValue;
            case Condition.NotEqual:
                return comparedValue != baseValue;
            case Condition.Smaller:
                return comparedValue < baseValue;
            case Condition.EqualOrSmaller:
                return comparedValue <= baseValue;
            case Condition.Bigger:
                return comparedValue > baseValue;
            case Condition.EqualOrBigger:
                return comparedValue >= baseValue;
            default:
                throw new System.ArgumentException("Unknown condition");
        }
    }
}
