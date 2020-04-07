using UnityEngine;
using Valve.VR;

public abstract class PointerAction : MonoBehaviour
{
    [SerializeField] protected SteamVR_Input_Sources _handType;
    [SerializeField] protected SteamVR_Behaviour_Pose _controllerPose;
    [SerializeField] protected SteamVR_Action_Boolean _action;
    [SerializeField] protected GameObject _laserPrefab;
    [SerializeField] protected GameObject _reticlePrefab;
    [SerializeField] protected GameObject _invalidReticlePrefab;
    [SerializeField] protected Color _laserColorValid;
    [SerializeField] protected Color _laserColorNotValid;
    [SerializeField] protected Vector3 _reticleOffset;

    protected abstract void ShowLaser(float length);
    protected abstract void ShowIndicator();
    protected abstract void HidePointer();
    protected abstract void SetPointerColor(Color c);
    protected abstract void Execute();
}
