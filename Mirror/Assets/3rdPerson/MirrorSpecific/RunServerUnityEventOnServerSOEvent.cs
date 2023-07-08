using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UKnack.Attributes;
using UKnack.KeyValues;
using UKnack.Events;
using UnityEngine.Events;

public sealed class RunServerUnityEventOnServerSOEvent : ServerBehaviourSimpleAbstract, ISubscriberToEvent
{
	[SerializeField]
	[ValidReference]
	private SOEvent _serverSOEvent;

	[SerializeField]
	private UnityEvent _serverUnityEvent;

    private string _description = string.Empty;
    public string Description => _description;

    public void OnEventNotification()
    {
        _serverUnityEvent?.Invoke();
    }
    protected override void NetworkBehaviourOnEnable()
    {
        _description = $"{gameObject.name} - {nameof(RunServerUnityEventOnServerSOEvent)}";
        if (_serverSOEvent == null)
            throw new System.ArgumentNullException(nameof(_serverSOEvent));
        _serverSOEvent.Subscribe(this);
    }
    protected override void NetworkBehaviourOnDisable()
    {
        _serverSOEvent.UnsubscribeNullSafe(this);
    }


}
