using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerMenu : MonoBehaviour
{
    [SerializeField] protected SteamVR_Action_Boolean _action;
    [SerializeField] protected SteamVR_Input_Sources _handType;
    [SerializeField] protected SteamVR_Behaviour_Pose _controllerPose;
    [SerializeField] protected GameObject _menuCanvas;
    private bool isShowing;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_action.GetStateDown(_handType))
        {
            isShowing = !isShowing;
            _menuCanvas.SetActive(isShowing);
        }
    }
}
