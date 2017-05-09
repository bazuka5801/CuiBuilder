using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class RustCanvasEx
{
    
    #region RectTransform

    public static void SetPosition(this RectTransform transform, Vector2 anchorMin, bool borderCollision = false)
    {
        SetRect(transform, anchorMin, transform.GetLocalSize()+anchorMin, borderCollision);
    }

    public static void SetRect(this RectTransform transform, Vector2 anchorMin, Vector2 anchorMax, bool borderCollision = false)
    {
        Vector2 shift = Vector2.zero;
        if (borderCollision)
        {
            if (anchorMin.x < 0) shift.x = -anchorMin.x;
            if (anchorMin.y < 0) shift.y = -anchorMin.y;
            if (anchorMax.x > 1) shift.x = -anchorMax.x + 1;
            if (anchorMax.y > 1) shift.y = -anchorMax.y + 1;
        }
        transform.anchorMin = anchorMin + shift;
        transform.anchorMax = anchorMax + shift;
        transform.offsetMin = Vector2.zero;
        transform.offsetMax = Vector2.zero;
    }

    public static void SetWorldSize(this RectTransform transform, Vector2 size)
    {
        var worldAnchorMax = transform.GetParent().GetWorldPoint(transform.anchorMin) + size;
        var localAnchorMax = transform.GetParent().GetLocalPoint(worldAnchorMax);
        transform.SetRect(transform.anchorMin, localAnchorMax);
    }

    public static void SetLocalSize(this RectTransform transform, Vector2 size)
    {
        transform.SetRect(transform.anchorMin, transform.anchorMin+size);
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
        child.transform.SetParent(transform, false);
        child.AddComponent<RectTransform>();
        return child;
    }


    public static Vector2 GetPixelShift(this RectTransform transform)
    {
        return (((RectTransform)transform.parent).GetWorldSize().Div(new Vector2(Screen.width, Screen.height)));
    }
    public static void SetPixelPosition(this RectTransform transform, Vector2 position)
    {
        transform.SetRect(transform.anchorMin + Vector2.Scale(transform.GetPixelShift(), position), transform.anchorMax-transform.anchorMin + Vector2.Scale(transform.GetPixelShift(), position));
    }
    public static void SetPixelSize(this RectTransform transform, Vector2 size)
    {
        transform.SetRect(transform.anchorMin, transform.anchorMin+Vector2.Scale(transform.GetPixelShift(), size));
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
            if (transform.root == current || transform.root == current.parent) break;
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
        return transform.GetParent().GetWorldPoint(transform.anchorMin + Vector2.Scale(transform.GetLocalSize(), pivot));
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
