using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControllerMenuInteraction : MonoBehaviour
{
    public float m_DefaultLength = 5.0f;
    public GameObject m_Dot;
    public ControllerVRInputModule m_InputModule;

    private LineRenderer m_LineRenderer = null;

    private void Awake()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        PointerEventData data = m_InputModule.GetData();
        float targetLength = data.pointerCurrentRaycast.distance == 0 ? m_DefaultLength : data.pointerCurrentRaycast.distance;
        RaycastHit hit = CreateRaycast(targetLength);
        Vector3 endPosition = transform.position + (transform.forward * targetLength);

        if (hit.collider != null) {
            endPosition = hit.point;
        }

        m_Dot.transform.position = endPosition;
        m_LineRenderer.SetPosition(0, transform.position);
        m_LineRenderer.SetPosition(1, endPosition);
    }

    private RaycastHit CreateRaycast(float length) {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, m_DefaultLength);

        return hit;
    }

    /* [SerializeField] LayerMask _mask;

     GameObject _laser;
     Material _laserMaterial;
     Transform _laserTransform;

     bool _canInteract;
     Vector3 _hitPoint;

     void Start()
     {
         _laser = Instantiate(_laserPrefab);
         _laserTransform = _laser.transform;
         _laserMaterial = _laser.GetComponent<MeshRenderer>().material;
         _laser.SetActive(false);
     }

     void Update()
     {
         if (_action.GetState(_handType))
         {
             RaycastHit hit;

             _canInteract = false;

             if (Physics.Raycast(_controllerPose.transform.position, transform.forward, out hit, 100f, _mask))
             {
                 _hitPoint = hit.point;
                 int hitLayer = hit.collider.gameObject.layer;
                 string hitTag = hit.collider.gameObject.tag;

                 Debug.Log(hitTag);

                 if (hitTag == "Userinterface")
                 {
                     _canInteract = true;
                 }
                 else
                 {
                     SetPointerColor(_laserColorNotValid);
                 }
             }
             else
             {
                HidePointer();
             }

             ShowLaser(hit.distance);

             if (_canInteract)
             {
                 SetPointerColor(_laserColorValid);
             }
         }

         if (_action.GetStateUp(_handType))
         {
             HidePointer();

             if (_canInteract)
                 Execute();
         }
     }

     protected override void SetPointerColor(Color c)
     {
         _laserMaterial.color = c;
     }

     protected override void ShowLaser(float length)
     {
         _laser.SetActive(true);
         _laserTransform.position = Vector3.Lerp(_controllerPose.transform.position, _hitPoint, .5f);
         _laserTransform.LookAt(_hitPoint);
         _laserTransform.localScale = new Vector3(_laserTransform.localScale.x, _laserTransform.localScale.y, length);
     }

     protected override void Execute()
     {
         Debug.Log("Press button");
     }

     protected override void HidePointer()
     {
         _laser.SetActive(false);
     }

     protected override void ShowIndicator()
     {
         throw new System.NotImplementedException();
     }*/
}
