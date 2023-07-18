using System.Collections;
using System.Collections.Generic;
using UKnack.Attributes;
using UKnack.Commands;
using UnityEngine;

public class DestroyGameoObjectCommand : CommandMonoBehaviour
{
    [SerializeField]
    [MarkNullAsColor(0.2f, 0.5f, 0.2f, "Null value means, that this gameobject will self-destruct on Command")]
    public GameObject toBeDestroyed;

    public override void Execute()
    {
        if (toBeDestroyed == null)
        {
            Destroy(gameObject);
            return;
        }
        Destroy(toBeDestroyed);
    }
}
