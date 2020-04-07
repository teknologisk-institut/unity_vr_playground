using UnityEngine;
using Valve.VR;

public class ControllerTeleport : PointerAction
{
    [SerializeField] Transform _cameraRigTransform;
    [SerializeField] Transform _headTransform;
    [SerializeField] LayerMask _mask;

    GameObject _laser;
    GameObject _reticle;
    GameObject _invalidReticle;
    Transform _laserTransform;
    Material _laserMaterial;
    Material _reticleMaterial;
    Material _invalidReticleMaterial;
    Vector3 _hitPoint;
    Vector3 _hitNormal;
    bool _canTeleport;

    void Start()
    {
        _laser = Instantiate(_laserPrefab);
        _laserTransform = _laser.transform;
        _laserMaterial = _laser.GetComponent<MeshRenderer>().material;
        _laser.SetActive(false);

        _reticle = Instantiate(_reticlePrefab);
        _reticleMaterial = _reticle.GetComponent<MeshRenderer>().material;
        _reticle.SetActive(false);

        _invalidReticle = Instantiate(_invalidReticlePrefab);
        _invalidReticleMaterial = _invalidReticle.GetComponentInChildren<MeshRenderer>().material;
        _invalidReticle.SetActive(false);
    }

    void Update()
    {
        if (_action.GetState(_handType))
        {
            RaycastHit hit;

            _canTeleport = false;

            if (Physics.Raycast(_controllerPose.transform.position, transform.forward, out hit, 100f, _mask))
            {
                _hitPoint = hit.point;
                _hitNormal = hit.normal;
                int hitLayer = hit.collider.gameObject.layer;
                string hitTag = hit.collider.gameObject.tag;

                if (hitTag == "Floor")
                {
                    _canTeleport = true;
                }
                else
                    SetPointerColor(_laserColorNotValid);

                ShowLaser(hit.distance);
                ShowIndicator();
            }
            else
                HidePointer();

            if (_canTeleport)
            {
                SetPointerColor(_laserColorValid);
            }

        }

        if (_action.GetStateUp(_handType))
        {
            HidePointer();

            if (_canTeleport)
                Execute();
        }
    }

    protected override void SetPointerColor(Color c)
    {
        _laserMaterial.color = c;
        _reticleMaterial.color = c;
        _invalidReticleMaterial.color = c;
    }

    protected override void ShowLaser(float length)
    {
        _laser.SetActive(true);
        _laserTransform.position = Vector3.Lerp(_controllerPose.transform.position, _hitPoint, .5f);
        _laserTransform.LookAt(_hitPoint);
        _laserTransform.localScale = new Vector3(_laserTransform.localScale.x, _laserTransform.localScale.y, length);
    }

    protected override void ShowIndicator()
    {
        GameObject curReticle;
        curReticle = (_canTeleport) ? _reticle : _invalidReticle;
        curReticle.SetActive(true);

        if (curReticle == _reticle)
            _invalidReticle.SetActive(false);
        else
            _reticle.SetActive(false);

        curReticle.transform.position = _hitPoint + _reticleOffset;
        curReticle.transform.up = _hitNormal;
    }

    protected override void HidePointer()
    {
        _laser.SetActive(false);
        _reticle.SetActive(false);
        _invalidReticle.SetActive(false);
    }
    protected override void Execute()
    {
        Vector3 difference = _cameraRigTransform.position - _headTransform.position;
        difference.y = 0;

        _cameraRigTransform.position = _hitPoint + difference;
    }
}