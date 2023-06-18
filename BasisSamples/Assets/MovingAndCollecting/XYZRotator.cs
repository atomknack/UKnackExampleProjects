using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XYZRotator : MonoBehaviour
{
    [SerializeField]
    private Vector3 _speed;

    private void Update()
    {
        transform.rotation = Quaternion.Euler(_speed * Time.deltaTime) * transform.rotation;
    }
}
