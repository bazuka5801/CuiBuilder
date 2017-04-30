using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class RustCanvasEx
{
    
    #region RectTransform

    public static void SetPosition(this RectTransform transform, Vector2 anchorMin)
    {
        SetRect(transform, anchorMin, transform.GetLocalSize()+anchorMin);
    }
    public static void SetRect(this RectTransform transform, Vector2 anchorMin, Vector2 anchorMax)
    {
        transform.anchorMin = anchorMin;
        transform.anchorMax = anchorMax;
        transform.offsetMin = Vector2.zero;
        transform.offsetMax = Vector2.zero;
    }
    public static Vector2 GetLocalSize(this RectTransform transform)
    {
        return transform.anchorMax - transform.anchorMin;
    }
    public static Vector2 GetWorldSize(this RectTransform transform)
    {
        var worldSize = GetLocalSize(transform);
        foreach (var parent in GetParents(transform))
            worldSize = Vector2.Scale(worldSize, parent.GetLocalSize());
        return worldSize;
    }

    public static Vector2 GetMouseLocal(this RectTransform transform)
    {
        return transform.GetParent().GetLocalPoint(RustCanvas.GetMouseAnchor());
    }

    public static GameObject CreateChild(this RectTransform transform, string name)
    {
        var child = new GameObject(name);
        child.transform.SetParent(transform);
        child.AddComponent<RectTransform>();
        return child;
    }

    #endregion

    #region Transform Point

    public static Vector2 GetWorldPoint(this RectTransform transform, Vector2 localPoint)
    {
        foreach (var parent in GetHierarchy(transform))
            localPoint = Vector2.Scale(localPoint, parent.GetLocalSize())+parent.anchorMin;
        return localPoint;
    }

    public static Vector2 GetLocalPoint(this RectTransform transform, Vector2 point)
    {
        var hierarchy = GetHierarchy(transform);
        hierarchy.Reverse();
        foreach (var hierarchyElement in hierarchy)
        {
            point -= hierarchyElement.anchorMin;
            point = point.Div(hierarchyElement.GetLocalSize());
        }
        return point;
    }

    #endregion

    #region Hierarchy Functions

    private static List<RectTransform> GetHierarchy(RectTransform transform)
    {
        var hierarchy = GetParents(transform);
        hierarchy.Insert(0, transform);
        return hierarchy;
    }
    private static List<RectTransform> GetParents(RectTransform transform)
    {
        var parents = new List<RectTransform>();
        RectTransform current = transform;
        while (true)
        {
            if (transform.root == current.parent) break;
            current = (RectTransform) current.parent;
            parents.Add(current);
        }
        return parents;
    }

    public static RectTransform GetParent(this Transform transform)
    {
        return (RectTransform) transform.parent;
    }

    #endregion

    #region Pivot

    public static Vector2 GetPivotLocalPosition(this RectTransform transform, Vector2 pivot)
    {
        return transform.anchorMin + Vector2.Scale(transform.GetLocalSize(), pivot);
    }
    public static Vector2 GetPivotWorldPosition(this RectTransform transform, Vector2 pivot)
    {
        return transform.GetWorldPoint(transform.anchorMin + Vector2.Scale(transform.GetLocalSize(), pivot));
    }

    #endregion

    #region Vector2

    public static Vector2 WithY(this Vector2 vec, float y)
    {
        return new Vector2(vec.x, y);
    }
    public static Vector2 WithX(this Vector2 vec, float x)
    {
        return new Vector2(x, vec.y);
    }
    public static Vector2 Div(this Vector2 vec, Vector2 vec2)
    {
        return new Vector2(vec.x/vec2.x, vec.y/vec2.y);
    }

    #endregion

    #region EventTrigger

    public static void Add(this EventTrigger eventTrigger, EventTriggerType eventID, Action callback)
    {
        EventTrigger.Entry eventEntry = new EventTrigger.Entry() { eventID = eventID };
        eventEntry.callback.AddListener(e => callback());
        eventTrigger.triggers.Add(eventEntry);
    }

    #endregion

}
