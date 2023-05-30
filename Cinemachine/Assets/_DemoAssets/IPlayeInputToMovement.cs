using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayeInputToMovement
{
    bool IsAimPressed { get; }
    bool IsFirePressed { get; }
    Vector3 NextPosition { get; }
    Quaternion NextRotation { get; }
}