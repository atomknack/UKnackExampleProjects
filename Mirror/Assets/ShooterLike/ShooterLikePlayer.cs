using Mirror;
using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UKnack.Events;
using UnityEngine;

public class ShooterLikePlayer : NetworkBehaviour
{
    [SerializeField]
    [ValidReference]
    [DisableEditingInPlaymode]
    private SOEvent<Vector2> _xzInputEvent;

    private Vector3 _move = Vector3.zero;

    public void OnEnable()
    {
        if (_xzInputEvent == null)
            throw new System.ArgumentNullException(nameof(_xzInputEvent));
        _xzInputEvent.Subscribe(OnInputXZ);

    }

    public void OnDisable()
    {
        _xzInputEvent.UnsubscribeNullSafe(OnInputXZ);
    }

    public void OnInputXZ(Vector2 input)
    {
        //Debug.Log(input);
        _move = new Vector3(input.x, 0, input.y);
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        transform.Rotate(0, _move.x * Time.deltaTime * 110.0f, 0);
        transform.Translate(0, 0, _move.z * Time.deltaTime * 4f);
    }
}
