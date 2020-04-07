using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectContainer : MonoBehaviour
{

    public GameObject prefab;
    public GameObject instantiatedPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (instantiatedPrefab != null)
        {
            instantiatedPrefab.transform.parent = transform;

            return;
        }

        if (prefab != null)
        {
            GameObject go = Instantiate(prefab, transform.position, Quaternion.identity);
            go.transform.parent = transform;
        }
    }
}
