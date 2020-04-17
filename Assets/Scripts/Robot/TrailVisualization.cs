using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using RosSharp.RosBridgeClient;

[RequireComponent (typeof(JointStatePatcher))]
public class TrailVisualization : MonoBehaviour
{
    [SerializeField] float _drawRate = .1f;
    [SerializeField] Material _material;
    [SerializeField] int _trailPoolSize;
    [SerializeField] int _trailRenderQueue = 2001;
    [SerializeField] int _robotRenderQueue = 2002;

    GameObject _template;
    GameObject[] _trailPool;
    GameObject _trailHolder;
    Transform[] _childTransforms;
    IEnumerator drawRoutine;

    void Awake()
    {
        _childTransforms = GetComponentsInChildren<Transform>();

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.material.renderQueue = _robotRenderQueue;
        }

        _template = new GameObject(gameObject.name + "_trail_template");
        BuildTemplate(transform, _template);

        _template.SetActive(false);

        _trailHolder = new GameObject("RobotTrail");
        _trailPool = new GameObject[_trailPoolSize];

        for (int i = 0; i < _trailPool.Length; i++)
        {
            _trailPool[i] = CreateTrailElement();
        }

        drawRoutine = DrawRobotTrail(_drawRate);
    }

    private void OnEnable()
    {
        _trailHolder.SetActive(true);

        StartCoroutine(drawRoutine);
    }

    private void OnDisable()
    {
        StopCoroutine(drawRoutine);
        HideRobotTrail();

        _trailHolder.SetActive(false);
    }

    void BuildTemplate(Transform parent, GameObject newParent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject newChild = new GameObject();
            newChild.transform.position = parent.GetChild(i).position;
            newChild.transform.rotation = parent.GetChild(i).rotation;
            newChild.transform.parent = newParent.transform;

            MeshFilter meshFilter = parent.GetChild(i).GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                MeshFilter newMeshFilter = newChild.AddComponent<MeshFilter>();
                newMeshFilter.mesh = meshFilter.mesh;
            }

            MeshRenderer meshRenderer = parent.GetChild(i).GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                MeshRenderer newMeshRenderer = newChild.AddComponent<MeshRenderer>();
                newMeshRenderer.material = _material;
                newMeshRenderer.receiveShadows = false;
                newMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            }

            BuildTemplate(parent.GetChild(i), newChild);
        }
    }

    IEnumerator DrawRobotTrail(float repeatRate)
    {
        int i = 0;

        while(true)
        {
            if (i == (_trailPoolSize - 1))
                i = 0;

            DrawTrailElement(_trailPool[i]);

            yield return new WaitForSeconds(repeatRate);

            i++;
        }
    }

    void HideRobotTrail()
    {
        foreach (GameObject go in _trailPool)
        {
            go.SetActive(false);
        }
    }

    void DrawTrailElement(GameObject go)
    {
        Transform[] trailChildTransforms = go.GetComponentsInChildren<Transform>();

        for (int i = 0; i < _childTransforms.Length; i++)
        {
            trailChildTransforms[i].position = _childTransforms[i].position;
            trailChildTransforms[i].rotation = _childTransforms[i].rotation;
        }

        go.SetActive(true);
    }

    GameObject CreateTrailElement()
    {
        GameObject go = Instantiate(_template, _trailHolder.transform);

        MeshRenderer[] meshRenderers = go.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].materials = new Material[1] { _material };
            meshRenderers[i].material.renderQueue = _trailRenderQueue;
        }

        go.SetActive(false);

        return go;
    }
}
