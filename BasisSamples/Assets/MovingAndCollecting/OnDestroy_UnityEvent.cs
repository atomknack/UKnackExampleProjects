using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnDestroy_UnityEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onDestroy;

    private void OnDestroy()
    {
        _onDestroy?.Invoke();
    }
}
