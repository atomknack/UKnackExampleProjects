using System.Collections;
using System.Collections.Generic;
using TMPro;
using UKnack.Attributes;
using UKnack.Events;
using UKnack.Values;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveAndJump : MonoBehaviour
{
    public bool CanJump { get { return _ticksAfterGrounded < 3; } }

    [SerializeField]
    [ValidReference]
    private SOValue<Vector2> _moveXZ;

    [SerializeField]
    private Vector2 _speedXZ;

    [SerializeField]
    private SOEvent _jumpEvent;

    [SerializeField]
    private float jumpAmount = 1f;
    [SerializeField]
    private float gravityScale = 5;


    private bool _isGrounded = false;
    private int _ticksAfterGrounded = 10000;

    private Rigidbody _rigidbody;

    private void OnEnable()
    {
        InitVariablesOrThrow();

        _jumpEvent.Subscribe(Jump);

        void InitVariablesOrThrow()
        {
            if (_moveXZ == null)
                throw new System.ArgumentNullException(nameof(_moveXZ));
            if (_jumpEvent == null)
                throw new System.ArgumentNullException(nameof(_jumpEvent));
            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody == null)
                throw new System.ArgumentNullException("Gameobject has no Rigidbody component");
        }
    }

    private void Jump()
    {
        //Debug.Log(_isGrounded);
        if (!CanJump)
            return;
        _rigidbody.AddForce(Vector3.up * jumpAmount, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision hit)
    {
        Debug.Log("OnTriggerEnter");
        _isGrounded = true;
        _ticksAfterGrounded = 0;
    }
    void OnCollisionExit(Collision hit)
    {
        _isGrounded = false;
    }

    private void FixedUpdate()
    {
        CalcGrounded();
        MoveRigidbodyXZ(CanJump ? 1f : 0.5f);
        RemoveFloatiness();

        void CalcGrounded()
        {
            //Debug.Log($"{_isGrounded} {_ticksAfterGrounded}");
            if (_isGrounded)
                return;
            if (_ticksAfterGrounded > 1000)
                return;
            _ticksAfterGrounded++;
        }
        void MoveRigidbodyXZ(float unrestricted)
        {
            var xz = _moveXZ.GetValue() * _speedXZ * unrestricted;
            _rigidbody.velocity += new Vector3(xz.x, 0, xz.y);
        }
        void RemoveFloatiness()
        {
            _rigidbody.AddForce(Physics.gravity * (gravityScale - 1) * _rigidbody.mass);
        }
    }

    private void OnDisable()
    {
        _jumpEvent.UnsubscribeNullSafe(Jump);
    }
}
