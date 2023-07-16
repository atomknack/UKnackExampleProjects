using System.Collections;
using System.Collections.Generic;
using UKnack.Commands;
using UnityEngine;

public class DestroyGameoObjectCommand : CommandMonoBehaviour
{
    public override void Execute()
    {
        Destroy(gameObject);
    }
}
