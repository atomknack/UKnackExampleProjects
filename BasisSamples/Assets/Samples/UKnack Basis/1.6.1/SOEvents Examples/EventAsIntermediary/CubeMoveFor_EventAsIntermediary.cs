using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UKnack.Events;
using UnityEngine;

public class CubeMoveFor_EventAsIntermediary : MonoBehaviour
{
    [SerializeField]
    [DisableEditingInPlaymode]
    [ValidReference]
    SOEvent<Vector2> _xzMoveEvent;

    [SerializeField]
    [DisableEditingInPlaymode]
    [ValidReference]
    SOPublisher<Vector3> _playerCoords;

    [SerializeField]
    [DisableEditingInPlaymode]
    [ValidReference]
    SOEvent<Vector3> _movePlayer;

    [SerializeField]
    private float _speed = 5f;

    private Vector3 _move;

    private void OnEnable()
    {
        if (_xzMoveEvent == null)
            throw new System.ArgumentNullException(nameof(_xzMoveEvent));
        if (_playerCoords == null)
            throw new System.ArgumentNullException(nameof(_playerCoords));
        if (_movePlayer == null)
            throw new System.ArgumentNullException(nameof(_movePlayer));
        _move = Vector3.zero;
        _xzMoveEvent.Subscribe(OnMoveInput);
        _movePlayer.Subscribe(OnPlayerMove);
    }

    private void OnDisable()
    {
        _xzMoveEvent.UnsubscribeNullSafe(OnMoveInput);
        _movePlayer.UnsubscribeNullSafe(OnPlayerMove);
    }

    private void OnMoveInput(Vector2 xz) => 
        _move = new Vector3(xz.x, 0, xz.y);

    private void OnPlayerMove(Vector3 newPosition)=>
        transform.position = newPosition;

    private void Update()
    {
        Vector3 newPosition = transform.position + _move * _speed * Time.deltaTime;
        if ((transform.position - newPosition).sqrMagnitude > 0.00000001f)
            _playerCoords.Publish(newPosition);
    }
}
