using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Utility : MonoBehaviour
{
    public static void DestroyAll<T>(ref T[] objs)
    {
        foreach (T component in objs)
        {
            Destroy(component as UnityEngine.Component);
        }
    }

    public static void DestroyComponentInChildren(GameObject go, System.Type type)
    {
        if (go == null || type == null)
            return;

        Component[] destroy = go.GetComponentsInChildren(type);

        if (destroy.Length == 0)
            return;

        for (int i = 0; i < destroy.Length; i++)
        {
            Destroy(destroy[i]);
        }
    }

    public static void DestroyImmediateComponentsInChildren(GameObject go, System.Type type)
    {
        if (go == null || type == null)
            return;

        Component[] destroy = go.GetComponentsInChildren(type);

        if (destroy.Length == 0)
            return;

        for (int i = 0; i < destroy.Length; i++)
        {
            DestroyImmediate(destroy[i]);
        }
    }

    public static void SetMaterialInChildren(GameObject go, Material mat)
    {
        if (go == null || mat == null)
            return;

        MeshRenderer[] meshRenderers = go.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].materials = new Material[1];
            meshRenderers[i].material = mat;
        }
    }

    public static void SetBehavioursEnabled(Behaviour[] behaviours, bool enabled)
    {
        if (behaviours.Length == 0)
            return;

        foreach (Behaviour b in behaviours)
        {
            b.enabled = enabled;
        }
    }
}
