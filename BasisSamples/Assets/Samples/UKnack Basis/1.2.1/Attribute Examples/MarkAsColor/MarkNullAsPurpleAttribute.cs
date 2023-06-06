using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UKnack.Attributes;
using UnityEngine;

[Conditional("UNITY_EDITOR")]
[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
public class MarkNullAsPurpleAttribute : MarkNullAsColorAttribute
{
    public MarkNullAsPurpleAttribute() : base(0.4f, 0, 0.55f, "Property should NOT be null")
    {
    }
}