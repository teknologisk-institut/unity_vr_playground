using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtY : MonoBehaviour
{
    [SerializeField] Transform _target;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_target);

        Vector3 lookPos = _target.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }
}