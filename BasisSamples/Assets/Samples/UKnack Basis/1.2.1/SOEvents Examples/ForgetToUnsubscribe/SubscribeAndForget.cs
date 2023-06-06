using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UKnack.Events;
using UnityEngine;
using UnityEngine.Events;

public class SubscribeAndForget : MonoBehaviour, ISubscriberToEvent<string>
{
    [SerializeField]
    [ValidReference]
    private SOEvent<string> _subscribeAndForget;

    [SerializeField]
    private UnityEvent<string> _unityEventToBeCalledOnSOEvent;

    private string _cachedName = string.Empty;
    public string Description => $"{nameof(SubscribeAndForget)} of {_cachedName}";

    private void OnEnable()
    {
        _cachedName = name;
        _subscribeAndForget.Subscribe(MethodThatLogString);
        _subscribeAndForget.Subscribe(this);
        _subscribeAndForget.Subscribe(_unityEventToBeCalledOnSOEvent);
    }
    private void OnDisable()
    {
        //Uncomment unsubscribe calls, and there will be no Debug errors when after stopping player in Editor

        //_subscribeAndForget.UnsubscribeNullSafe(MethodThatLogString);
        //_subscribeAndForget.UnsubscribeNullSafe(this);
        //_subscribeAndForget.UnsubscribeNullSafe(_unityEventToBeCalledOnSOEvent);
    }

    private void MethodThatLogString(string t) =>
        LogStringFrom(nameof(MethodThatLogString), t);

    public void OnEventNotification(string t) =>
        LogStringFrom(nameof(OnEventNotification), t);

    public void MethodForUnityEvent(string t) =>
        LogStringFrom(nameof(MethodForUnityEvent),t);

    public static void LogStringFrom(string name, string t) => 
        Debug.Log($"Called {name} with {t}");
}
