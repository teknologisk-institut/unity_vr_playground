using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField] float maxHeight;
    [SerializeField] float minHeight;
    [SerializeField] float hoverSpeed = 10f;

    void Update()
    {
        float hoverHeight = (maxHeight + minHeight) / 2.0f;
        float hoverRange = maxHeight - minHeight;

        this.transform.position += -transform.forward * hoverHeight * Mathf.Cos(Time.time * hoverSpeed) * hoverRange;
    }
}
