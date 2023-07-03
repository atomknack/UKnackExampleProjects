using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UKnack.Attributes;
using UKnack.KeyValues;
using UKnack.Events;
using UnityEngine.Events;

public class RunServerEventOnCount : NetworkBehaviour, ISubscriberToEvent<int, int>
{
	[SerializeField]
	[ValidReference]
	private SOKeyValue<int, int> _keyValue;

	[SerializeField]
	private int _valueCountForAnyKey;

	[SerializeField]
	private UnityEvent<int, int> _event;

    public string Description => throw new System.NotImplementedException();

    public void OnEventNotification(int key, int value)
    {
		if (value == _valueCountForAnyKey)
			_event.Invoke(key, value);
    }

    public override void OnStartServer()
	{
		if (_keyValue == null)
			throw new System.ArgumentNullException(nameof(_keyValue));
		_keyValue.Subscribe(this);
	}
    public override void OnStopServer()
    {
        base.OnStopServer();
		_keyValue.UnsubscribeNullSafe(this);
    }
}
