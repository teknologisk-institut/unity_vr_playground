using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerRotate : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean turnLeftAction;
    public SteamVR_Action_Boolean turnRightAction;
    public Transform cameraRig;
    public float turnAmount = 10f;

    void Update()
    {
        if (turnLeftAction.GetLastStateDown(handType))
        {
            Rotate(-turnAmount);
        }
        else if (turnRightAction.GetLastStateDown(handType))
        {
            Rotate(turnAmount);
        }
    }

    void Rotate(float degrees)
    {
        cameraRig.Rotate(0f, degrees, 0f, Space.Self);
    }
}