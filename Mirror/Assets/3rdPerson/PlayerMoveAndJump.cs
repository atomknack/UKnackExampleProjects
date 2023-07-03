using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UKnack.Attributes;
using UKnack.Events;
using UKnack.Values;
using UnityEngine;

public class PlayerMoveAndJump : NetworkBehaviour
{
    public bool CanJump { get { return _ticksAfterGrounded < 3; } }

    [SerializeField]
    [ValidReference]
    private SOValue<Vector2> _InputMoveXZ;
    [SerializeField]
    private SOEvent _InputJumpEvent;

    private bool _inputInitialized = false;

    private Vector2 _moveXZ;
    private Vector2 _clientPrevMoveDirection;

    [SerializeField]
    private Vector2 _speedXZ;


    [SerializeField]
    private float jumpAmount = 1f;
    [SerializeField]
    private float gravityScale = 5;


    private bool _isGrounded = false;
    private int _ticksAfterGrounded = 10000;

    [SerializeField]
    [ValidReference]
    private Rigidbody _rigidbody;

    public override void OnStartLocalPlayer()
    {
        InitVariablesOrThrow();

        _InputJumpEvent.Subscribe(OnInputJump);
        _inputInitialized = true;

        void InitVariablesOrThrow()
        {
            if (_InputMoveXZ == null)
                throw new System.ArgumentNullException(nameof(_InputMoveXZ));
            if (_InputJumpEvent == null)
                throw new System.ArgumentNullException(nameof(_InputJumpEvent));

            if (_rigidbody == null)
                _rigidbody = GetComponentInChildren<Rigidbody>();
            if (_rigidbody == null)
                throw new System.ArgumentNullException("Gameobject has no set Rigidbody component and it's children have no too");
        }
    }

    private void OnDisable() => OnStopClient();
    public override void OnStopClient()
    {
        if (_inputInitialized == false)
            return;

        _InputJumpEvent.UnsubscribeNullSafe(OnInputJump);
        _inputInitialized = false;
    }


    private void OnInputJump()
    {
        Jump();
    }

    [Command]
    private void Jump()
    {
        //Debug.Log(_isGrounded);
        if (!CanJump)
            return;
        _rigidbody.AddForce(jumpAmount * Vector3.up, ForceMode.Impulse);
    }

    [Command]
    private void Move(Vector2 direction)
    {
        _moveXZ = direction;
    }

    void OnCollisionEnter(Collision hit)
    {
        //Debug.Log("OnTriggerEnter");
        if (hit.gameObject.GetComponent<Ground>() != null)
        {
            _isGrounded = true;
            _ticksAfterGrounded = 0;
        }
    }
    void OnCollisionExit(Collision hit)
    {
        _isGrounded = false;
    }

    private void FixedUpdate()
    {
        HandleClientMoveInput();

        if (isServer == false)
            return;

        CalcGrounded();
        MoveRigidbodyXZ(CanJump ? 1f : 0.5f);
        RemoveFloatiness();

        void CalcGrounded()
        {
            //Debug.Log($"{_isGrounded} {_ticksAfterGrounded}");
            if (_isGrounded)
            {
                _rigidbody.AddForce(_rigidbody.mass * gravityScale * 0.1f * Vector3.up);
                return;
            }
            if (_ticksAfterGrounded > 1000)
                return;
            _ticksAfterGrounded++;
        }
        void MoveRigidbodyXZ(float unrestricted)
        {
            var xz = _moveXZ * _speedXZ * unrestricted;
            _rigidbody.velocity += new Vector3(xz.x, 0, xz.y);
        }
        void RemoveFloatiness()
        {
            _rigidbody.AddForce(Physics.gravity * (gravityScale - 1) * _rigidbody.mass);
        }
    }

    private void HandleClientMoveInput()
    {
        if (isLocalPlayer == false)
            return;

        Vector2 newDirection = _InputMoveXZ.GetValue();
        if (newDirection != _clientPrevMoveDirection)
        {
            Move(newDirection);
            _clientPrevMoveDirection = newDirection;
        }

    }
}
