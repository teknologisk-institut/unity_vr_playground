using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseGoal : MonoBehaviour
{
    [SerializeField] Transform _parent;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = _parent;
        transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
