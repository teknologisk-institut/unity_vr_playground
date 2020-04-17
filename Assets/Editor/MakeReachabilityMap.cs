using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using System.IO;
using UnityEditor;

public class MakeReachabilityMap : EditorWindow
{
    enum Type { Spheres, Perimeter };

    GameObject _go;
    Transform _robotTransform;
    string _pathToMapFile;
    Material _material;
    Type _type;
    bool _colorByReachability = false;
    Map _map;

    [MenuItem("Window/DTI/Make Reachability Map")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MakeReachabilityMap));
    }

    private void OnGUI()
    {
        EditorGUILayout.HelpBox("A map file is required to generate a reachability map. Please select the map file below.", MessageType.Info);

        if (GUILayout.Button("Select File ..."))
        {
            _pathToMapFile = EditorUtility.OpenFilePanel("Load Map File", "", "json,JSON");
        }

        if (string.IsNullOrEmpty(_pathToMapFile))
            return;

        _robotTransform = (Transform) EditorGUILayout.ObjectField("Robot Transform", _robotTransform, typeof(Transform), true);
        _material = (Material) EditorGUILayout.ObjectField("Material", _material, typeof(Material), true);
        _type = (Type) EditorGUILayout.EnumPopup("Map Type:", _type);

        if (_type == Type.Spheres)
            _colorByReachability = EditorGUILayout.Toggle("Color by Reachability", _colorByReachability);

        if (GUILayout.Button("Make"))
        {
            _go = new GameObject("Reachability Map");
            _go.transform.position = _robotTransform.position;

            DrawReachability();
        }
    }

    public void DrawReachability()
    {
        bool success = DeserializeMap();

        if (!success)
            return;

        if (_type == Type.Spheres)
        {
            DrawSpheres();
        }
        else if (_type == Type.Perimeter)
        {
            DrawPerimeter();
        }
    }

    private bool DeserializeMap()
    {
        string file = File.ReadAllText(_pathToMapFile);

        if (!File.Exists(_pathToMapFile))
        {
            Debug.LogError("Path does not contain a valid map file. Aborting.");

            return false;
        }

        _map = (Map)JsonUtility.FromJson(file, typeof(Map));

        return true;
    }

    void DrawSpheres()
    {
        int length = _map.points.Length;

        MeshRenderer[] meshRenderers = new MeshRenderer[length];

        for (int i = 0; i < length; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            go.transform.position = _map.points[i];
            go.transform.parent = _go.transform;

            MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            meshRenderer.receiveShadows = false;
            meshRenderers[i] = meshRenderer;
        }

        if (_colorByReachability)
        {
            for (int i = 0; i < length; i++)
            {
                SetRiColor(meshRenderers[i], _map.ris[i]);
            }
        }
    }

    void DrawPerimeter()
    {
        float dist = 0f;
        float newDist = 0f;

        for(int i = 0; i < _map.points.Length; i++)
        {
            Vector3 point = new Vector3(_map.points[i].x, _map.points[i].y, _map.points[i].z);

            newDist = Vector3.Distance(_go.transform.position, point);

            if (newDist > dist)
            {
                dist = newDist;
            }
        }

        newDist *= 2f;

        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.localScale = new Vector3(newDist, newDist, newDist);
        go.transform.parent = _go.transform;

        MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        meshRenderer.receiveShadows = false;
        meshRenderer.material = _material;
    }

    void SetRiColor(MeshRenderer mr, float ri)
    {
        mr.material = _material;

        if (ri >= 90f)
        {
            mr.material.color = new Color(0f, 0f, 255f, 50f);
        }
        else if (ri < 90f && ri >= 50f)
        {
            mr.material.color = new Color(0f, 255f, 255f, 50f);
        }
        else if (ri < 50f && ri >= 30f)
        {
            mr.material.color = new Color(0f, 255f, 0f, 50f);
        }
        else if (ri < 30f && ri >= 5f)
        {
            mr.material.color = new Color(255f, 255f, 0f, 50f);
        }
        else
        {
            mr.material.color = new Color(255f, 0f, 0f, 50f);
        }
    }
}
