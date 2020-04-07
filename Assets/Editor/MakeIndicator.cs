using UnityEditor;
using UnityEngine;
using System;

public class MakeIndicator : EditorWindow
{
    GameObject go;
    GameObject indicator;
    Material material;

    [MenuItem("Window/Custom/Make URDF Indicator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(MakeIndicator));
    }

    private void OnGUI()
    {
        go = (GameObject)EditorGUILayout.ObjectField(new GUIContent("GameObject"), go, typeof(GameObject), true);
        material = (Material)EditorGUILayout.ObjectField("Material", material, typeof(Material), true);
        
        if (GUILayout.Button("Make"))
        {
            indicator = Instantiate(go);
            indicator.name = go.name + "_indicator";

            Utility.DestroyImmediateComponentsInChildren(indicator, typeof(MonoBehaviour));
            Utility.DestroyImmediateComponentsInChildren(indicator, typeof(Joint));
            Utility.DestroyImmediateComponentsInChildren(indicator, typeof(Collider));
            Utility.DestroyImmediateComponentsInChildren(indicator, typeof(Rigidbody));

            Utility.SetMaterialInChildren(indicator, material);

            MeshRenderer[] meshRenderers = indicator.GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                meshRenderers[i].receiveShadows = false;
            }
        }
    }
}