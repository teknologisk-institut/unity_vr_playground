using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalSocketScript : MonoBehaviour
{
    [SerializeField] private Transform objectPlacer;

    private void OnTriggerEnter(Collider other)
    {
        EventManager.TriggerEvent("detach");
        other.transform.position = objectPlacer.position;
    }
}
