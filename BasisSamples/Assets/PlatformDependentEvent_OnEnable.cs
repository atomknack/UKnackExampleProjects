using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformDependentEvent_OnEnable : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _android;

    [SerializeField]
    private UnityEvent _ios;

    private void OnEnable()
    {

#if UNITY_ANDROID
        _android.Invoke();
#endif

#if UNITY_IOS
        _ios.Invoke();
#endif

    }

}
