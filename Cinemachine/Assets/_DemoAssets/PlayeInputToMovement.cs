using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UKnack.Events;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayeInputToMovement : MonoBehaviour, IPlayeInputToMovement
{
    public Vector3 NextPosition { get; private set; }
    public Quaternion NextRotation { get; private set; }

    public bool IsAimPressed { get; private set; }
    public bool IsFirePressed { get; private set; }

    [SerializeField]
    private float rotationPower = 3f;
    [SerializeField]
    public float rotationLerp = 0.5f;

    [SerializeField]
    private float _speed = 1f;

    [SerializeField]
    [Tooltip("main camera will be used if camera is null")]
    [DisableEditingInPlaymode]
    private Camera _camera;

    [SerializeField]
    [ValidReference]
    [DisableEditingInPlaymode]
    private Transform _follow;

    [SerializeField]
    [ValidReference]
    private SOEvent<Vector2> _horizontalMoveInput;

    [SerializeField]
    [ValidReference]
    private SOEvent<Vector2> _lookInput;

    [SerializeField]


    private Vector2 _move;
    private Vector2 _look;

    private void OnEnable()
    {
        if (_camera == null)
            _camera = Camera.main;

        _horizontalMoveInput.Subscribe(OnMove);
        _lookInput.Subscribe(OnLook);
    }

    private void OnDisable()
    {
        _horizontalMoveInput.UnsubscribeNullSafe(OnMove);
        _lookInput.UnsubscribeNullSafe(OnLook);
    }

    public void OnMove(Vector2 value)
    {
        _move = value;
    }

    public void OnLook(Vector2 value)
    {
        Debug.Log(value);
        _look = value;
    }

    public void OnAim(bool value)
    {
        bool before = IsAimPressed;
        IsAimPressed = value;
        //Debug.Log($"Aim value before: {before}, and now {aimValue}");
    }

    public void OnFire(bool value)
    {
        IsFirePressed = value;
    }

    private void Update()
    {

        _follow.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);
        _follow.rotation *= Quaternion.AngleAxis(_look.y * rotationPower, Vector3.right);


        var angles = _follow.transform.localEulerAngles;
        angles.z = 0;

        var angle = _follow.transform.localEulerAngles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }


        _follow.transform.localEulerAngles = angles;


        NextRotation = Quaternion.Lerp(_follow.transform.rotation, NextRotation, Time.deltaTime * rotationLerp);

        if (_move.x == 0 && _move.y == 0)
        {
            NextPosition = transform.position;

            if (IsAimPressed)
            {
                //Set the player rotation based on the look transform
                transform.rotation = Quaternion.Euler(0, _follow.transform.rotation.eulerAngles.y, 0);
                //reset the y rotation of the look transform
                _follow.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
            }

            return;
        }
        float moveSpeed = _speed / 100f;
        Vector3 position = (transform.forward * _move.y * moveSpeed) + (transform.right * _move.x * moveSpeed);
        NextPosition = transform.position + position;


        //Set the player rotation based on the look transform
        transform.rotation = Quaternion.Euler(0, _follow.transform.rotation.eulerAngles.y, 0);
        //reset the y rotation of the look transform
        _follow.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
    }


}

