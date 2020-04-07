using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyFreeze : MonoBehaviour
{
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
    }
}
