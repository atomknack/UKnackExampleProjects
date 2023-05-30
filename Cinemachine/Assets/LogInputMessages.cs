using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class LogInputMessages : MonoBehaviour
{
    private void OnMove(InputValue value)
    {
        Debug.Log($"name: {name}, OnMove message, value: {value.Get<Vector2>()}");
    }
    private void OnLook(InputValue value)
    {
        Debug.Log($"name: {name}, OnLook message, value: {value.Get<Vector2>()}");
    }
}
