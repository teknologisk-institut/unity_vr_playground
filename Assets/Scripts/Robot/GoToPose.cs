using System;
using RosSharp.RosBridgeClient;
using std_srvs = RosSharp.RosBridgeClient.MessageTypes.Std;
using UnityEngine;

namespace RosSharp.RosBridgeClientTest
{

    public class GoToPose : MonoBehaviour
    {
        [SerializeField] RosConnector _rosConnector;
        [SerializeField] string _planningService;
        [SerializeField] string _executionService;

        RosSocket _rosSocket;
        bool _hasPlan;

        void Start()
        {
            _rosSocket = _rosConnector.RosSocket;
        }

        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _rosSocket.CallService<std_srvs.TriggerRequest, std_srvs.TriggerResponse>(_planningService, HasPlan, new std_srvs.TriggerRequest());

                if (_hasPlan)
                    _rosSocket.CallService<std_srvs.TriggerRequest, std_srvs.TriggerResponse>(_executionService, Success, new std_srvs.TriggerRequest());
            }
        }

        private void HasPlan(std_srvs.TriggerResponse resp)
        {
            _hasPlan = resp.success;
            Debug.Log(resp.message);
        }

        private void Success(std_srvs.TriggerResponse resp)
        {
            Debug.Log(resp.success);
        }
    }
}