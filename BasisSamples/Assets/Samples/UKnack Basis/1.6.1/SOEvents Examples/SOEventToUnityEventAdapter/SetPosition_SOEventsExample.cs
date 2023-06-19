using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour
{
    public void Set(float y)
    {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, y, pos.z);
    }

    public void Set(Vector2 xz)
    {
        transform.position = new Vector3(xz.x, transform.position.y, xz.y);
    }
}
