using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UnityEngine;

public class AllDisabledToEditInPlaymode : MonoBehaviour
{
    [SerializeField]
    [DisableEditingInPlaymode]
    private int _justSimpleIntAndNotAReference;

    [SerializeField]
    [DisableEditingInPlaymode]
    private int _justSimpleString;

    [SerializeField]
    [DisableEditingInPlaymode]
    [MarkNullAsColor(0.5f, 0.5f, 0.1f)]
    private GenericBehaviour<int> _genericIntDisabledBefore;

    [SerializeField]
    [MarkNullAsColor(0.35f, 0.35f, 0.1f)]
    [DisableEditingInPlaymode]
    private GenericBehaviour<int> _genericIntDisabledAfter;

    [SerializeField]
    [DisableEditingInPlaymode]
    [MarkNullAsPurple]
    private GenericBehaviour<int> _genericInt4DisabledMarkNullAsPurple;

    [SerializeField]
    [DisableEditingInPlaymode]
    [ValidReference]
    private GenericBehaviour<string> _validGenericStringDisabledBefore;

    [SerializeField]
    [ValidReference]
    [DisableEditingInPlaymode]
    private GenericBehaviour<string> _validGenericStringDisabledAfter;
}
