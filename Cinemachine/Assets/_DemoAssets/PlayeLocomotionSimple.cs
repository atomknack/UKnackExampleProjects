using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UnityEngine;


[RequireComponent(typeof(IPlayeInputToMovement))]
public class PlayeLocomotionSimple : MonoBehaviour
{
    IPlayeInputToMovement _movement;

    private void OnEnable()
    {
        _movement = GetComponent<IPlayeInputToMovement>();
    }

    void Update()
    {
        transform.position = _movement.NextPosition;
        //transform.rotation = _movement.NextRotation;
    }
}
