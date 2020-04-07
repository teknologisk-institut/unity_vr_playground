using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerGrab : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;
    public Material outline;
    public LayerMask layerMask;

    private GameObject collidingObject;
    private GameObject objectInHand;
    private Material baseMaterial;

    void Update()
    {
        if (grabAction.GetLastStateDown(handType))
        {
            if (collidingObject && InLayerMask(collidingObject))
            {
                GrabObject();
            }
        }

        if (grabAction.GetLastStateUp(handType))
        {
            if (objectInHand)
            {
                ReleaseObject();
            }
        }
    }

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }

        collidingObject = col.gameObject;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (InLayerMask(other.gameObject))
        {
            Highlight(other.gameObject, true);
            SetCollidingObject(other);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (InLayerMask(other.gameObject))
        {
            SetCollidingObject(other);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (InLayerMask(other.gameObject))
        {

            if (!collidingObject)
            {
                return;
            }

            Highlight(other.gameObject, false);

            collidingObject = null;
        }
    }

    private void GrabObject()
    {
        objectInHand = collidingObject;
        collidingObject = null;

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());

            objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
            objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();

        }

        objectInHand = null;
    }

    void Highlight(GameObject go, bool on)
    {
        Material secondaryMaterial = null;

        if (on)
            secondaryMaterial = outline;

        MeshRenderer goRenderer = go.GetComponent<MeshRenderer>();

        if (goRenderer != null)
        {
            Material baseMaterial = goRenderer.materials[0];
            go.GetComponent<MeshRenderer>().materials = new Material[] { baseMaterial, secondaryMaterial };
        }
    }

    bool InLayerMask(GameObject go)
    {
        return (layerMask == (layerMask | (1 << go.layer)));
    }
}
