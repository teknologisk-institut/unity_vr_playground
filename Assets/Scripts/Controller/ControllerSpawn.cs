using UnityEngine;
using Valve.VR;

public class ControllerSpawn : PointerAction
{
    [SerializeField] GameObject _spawnPrefab;
    [SerializeField] LayerMask _mask;

    GameObject _laser;
    GameObject _reticle;
    Transform _laserTransform;
    Material _laserMaterial;
    bool _canPlace = false;
    Vector3 _hitPoint;
    GameObject _hitGo;
    Vector3 _hitNormal;
    GameObject _invalidReticle;
    Material _invalidReticleMaterial;
    GameObject _curReticle;

    void Start()
    {
        _laser = Instantiate(_laserPrefab);
        _laserTransform = _laser.transform;
        _laserMaterial = _laser.GetComponent<MeshRenderer>().material;
        _laser.SetActive(false);

        _reticle = Instantiate(_reticlePrefab);
        _reticle.SetActive(false);

        _invalidReticle = Instantiate(_invalidReticlePrefab);
        _invalidReticleMaterial = _invalidReticle.GetComponentInChildren<MeshRenderer>().material;
        _invalidReticleMaterial.color = _laserColorNotValid;
        _invalidReticle.SetActive(false);
    }

    void Update()
    {
        if (_action.GetState(_handType))
        {
            _canPlace = false;

            RaycastHit hit;

            if (Physics.Raycast(_controllerPose.transform.position, transform.forward, out hit, 50f, _mask))
            {
                _hitGo = hit.collider.gameObject;
                _hitNormal = hit.normal;
                string hitTag = _hitGo.tag;
                _hitPoint = hit.point;

                if (hitTag == "SpawnArea")
                    _canPlace = true;
                else
                    SetPointerColor(_laserColorNotValid);

                ShowLaser(hit.distance);
                ShowIndicator();
            }
            else
                HidePointer();

            if (_canPlace)
            {
                SetPointerColor(_laserColorValid);
            }
        }
        else if (_action.GetStateUp(_handType))
        {
            HidePointer();

            if (_canPlace)
                Execute();
        }
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
        _curReticle = (_canPlace) ? _reticle : _invalidReticle;
        _curReticle.SetActive(true);

        if (_curReticle == _reticle)
        {
            _invalidReticle.SetActive(false);
            _curReticle.transform.position = _hitGo.transform.position;
            _curReticle.transform.rotation = _hitGo.transform.rotation;
        }
        else
        {
            _reticle.SetActive(false);
            _curReticle.transform.position = _hitPoint + _reticleOffset;
            _curReticle.transform.up = _hitNormal;
        }
    }

    protected override void HidePointer()
    {
        _laser.SetActive(false);
        _reticle.SetActive(false);
        _invalidReticle.SetActive(false);
    }

    protected override void SetPointerColor(Color c)
    {
        _laserMaterial.color = c;
    }

    protected override void Execute()
    {
        _hitGo.SetActive(false);
        Instantiate(_spawnPrefab, _curReticle.transform.position, _curReticle.transform.rotation);
    }
}