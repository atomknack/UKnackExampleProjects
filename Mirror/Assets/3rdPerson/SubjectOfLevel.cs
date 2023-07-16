using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectOfLevel : NetworkBehaviour
{
    float lowerBorder = - 100f;

    protected bool _alreadyKnow = false;
    public virtual void WentOutside()
    {
        _alreadyKnow = true;
        if (!isServer)
            return;
        NetworkServer.Destroy(gameObject);
        //Destroy(gameObject);

    }

    private void Update()
    {
        if (_alreadyKnow)
            return;

        if (transform.position.y < lowerBorder)
            WentOutside();
    }
}
