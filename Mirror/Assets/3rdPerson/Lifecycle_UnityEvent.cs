using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lifecycle_UnityEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onAwake;
    private void Awake() => _onAwake?.Invoke();

    [SerializeField]
    private UnityEvent _onStart;
    private void Start() => _onStart?.Invoke();

    [SerializeField]
    private UnityEvent _onEnable;
    private void OnEnable() => _onEnable?.Invoke();

    [SerializeField]
    private UnityEvent _onDisable;
    private void OnDisable() => _onDisable?.Invoke();

    [SerializeField]
    private UnityEvent _onDestroy;
    private void OnDestroy() => _onDestroy?.Invoke();
}
