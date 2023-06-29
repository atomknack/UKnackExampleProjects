using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectOfLevel : MonoBehaviour
{
    float lowerBorder = - 100f;

    public virtual void WentOutside()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (transform.position.y < lowerBorder)
            WentOutside();
    }
}
