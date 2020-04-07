using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerScript : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.StartListening("DetachController", DetachController);
    }

    private void OnDisable()
    {
        EventManager.StopListening("DetachController", DetachController);
    }

    public void DetachController()
    {
        Utility.SetBehavioursEnabled(GetComponents<Behaviour>(), false);
    }

    public void AttachController()
    {
        Utility.SetBehavioursEnabled(GetComponents<Behaviour>(), true);
    }
}
