using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using RosSharp.RosBridgeClient;
using std_srvs = RosSharp.RosBridgeClient.MessageTypes.Std;

public class ControllerExecuteAction : MonoBehaviour
{
    [SerializeField] protected SteamVR_Input_Sources _handType;
    [SerializeField] protected SteamVR_Behaviour_Pose _controllerPose;
    [SerializeField] protected SteamVR_Action_Boolean _action;
    [SerializeField] RosConnector _rosConnector;
    [SerializeField] string _service;

    RosSocket _rosSocket;

    // Start is called before the first frame update
    void Start()
    {
        _rosSocket = _rosConnector.RosSocket;
    }

    // Update is called once per frame
    void Update()
    {
        if (_action.GetStateUp(_handType))
        {
            _rosSocket.CallService<std_srvs.EmptyRequest, std_srvs.EmptyResponse>(_service, ServiceCallHandler, new std_srvs.EmptyRequest());
        }
    }

    private static void ServiceCallHandler(std_srvs.EmptyResponse message)
    {
        Debug.Log("ROS distro: " + message);
    }
}