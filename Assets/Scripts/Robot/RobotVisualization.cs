using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RobotVisualization : MonoBehaviour
{
    [HideInInspector] public GameObject Indicator { get; private set; }

    [SerializeField] Material _indicatorMaterial;
    [SerializeField] bool _drawTrail = false;
    [SerializeField] float _drawRate = .5f;
    [SerializeField] Material _trailMaterial;
    [SerializeField] int _trailPoolSize;

    GameObject[] _trailPool;
    GameObject _trailHolder;
    Transform[] _childTransforms;
    bool isDrawing = false;
    IEnumerator drawRoutine;

    void Awake()
    {
        _childTransforms = GetComponentsInChildren<Transform>();

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer mr in meshRenderers)
        {
            mr.material.renderQueue = 2002;
        }

        Indicator = new GameObject(gameObject.name + "_indicator");
        BuildIndicator(transform, Indicator);

        Indicator.SetActive(false);

        _trailHolder = new GameObject("RobotTrail");
        _trailPool = new GameObject[_trailPoolSize];

        for (int i = 0; i < _trailPool.Length; i++)
        {
            _trailPool[i] = CreateTrailElement();
        }

        drawRoutine = DrawRobotTrail(_drawRate);
    }

    private void Update()
    {
        if (_drawTrail)
        {
            if (!isDrawing)
            {
                StartCoroutine(drawRoutine);

                isDrawing = true;
            }
        }
        else
        {
            if (isDrawing)
            {
                StopCoroutine(drawRoutine);
                HideRobotTrail();

                isDrawing = false;
            }
        }
    }

    void BuildIndicator(Transform parent, GameObject newParent)
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
                newMeshRenderer.material = _indicatorMaterial;
                newMeshRenderer.receiveShadows = false;
                newMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            }

            BuildIndicator(parent.GetChild(i), newChild);
        }
    }

    IEnumerator DrawRobotTrail(float repeatRate)
    {
        int i = 0;

        while(_drawTrail)
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
        GameObject go = Instantiate(Indicator, _trailHolder.transform);

        MeshRenderer[] meshRenderers = go.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].materials = new Material[1] { _trailMaterial };
            meshRenderers[i].material.renderQueue = 2001;
        }

        go.SetActive(false);

        return go;
    }
}
