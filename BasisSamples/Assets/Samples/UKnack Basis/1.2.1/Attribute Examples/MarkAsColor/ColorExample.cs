using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UnityEngine;

public class ColorExample : MonoBehaviour
{
    [SerializeField]
    [MarkNullAsColor(0.7f, 0.7f, 0.1f)]
    GenericBehaviour<int> _genericInt1;

    [SerializeField]
    [MarkNullAsColor(0.5f,0.5f,0.1f)]
    GenericBehaviour<int> _genericInt2;


    [SerializeField]
    [MarkNullAsColor(0.35f, 0.35f, 0.1f)]
    GenericBehaviour<int> _genericInt3;

    [SerializeField]
    [MarkNullAsPurple]
    GenericBehaviour<int> _genericInt4MarkNullAsPurple;
}
