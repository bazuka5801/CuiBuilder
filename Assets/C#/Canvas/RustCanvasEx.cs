using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class RustCanvasEx
{

    private static Vector2 ScreenVec
    {
        get { return new Vector2( Screen.width, Screen.height ); }
    }

    private static Vector2 ScreenScaling
    {
        get { return ScreenVec.Div(RustCanvas.refResolution); }
    }

    #region RectTransform

    public static GameObject CreateChild( this RectTransform transform, string name )
    {
        var child = new GameObject( name );
        child.transform.SetParent( transform, false );
        child.AddComponent<RectTransform>();
        return child;
    }

    public static void SetRect( this RectTransform transform, Vector2 anchorMin, Vector2 anchorMax,
        bool borderCollision = false )
    {
        Vector2 shift = Vector2.zero;
        if (borderCollision)
        {
            if (anchorMin.x < 0) shift.x = -anchorMin.x;
            if (anchorMin.y < 0) shift.y = -anchorMin.y;
            if (anchorMax.x > 1) shift.x = -anchorMax.x + 1;
            if (anchorMax.y > 1) shift.y = -anchorMax.y + 1;
        }

        Vector2 lastOffsetMin = transform.offsetMin;
        Vector2 lastOffsetMax = transform.offsetMax;

        transform.anchorMin = anchorMin + shift;
        transform.anchorMax = anchorMax + shift;
        transform.offsetMin = lastOffsetMin;
        transform.offsetMax = lastOffsetMax;
    }
    public static void SetRectWorld( this RectTransform transform, Vector2 anchorMin, Vector2 anchorMax,
        bool borderCollision = false )
    {
        anchorMin = transform.GetParent().GetLocalPoint( anchorMin );
        anchorMax = transform.GetParent().GetLocalPoint( anchorMax );
        SetRect( transform, anchorMin, anchorMax, borderCollision );
    }

    #region Position

    #region Anchor

    public static void SetPositionAnchorLocal( this RectTransform transform, Vector2 anchorMin, bool borderCollision = false )
    {
        SetRect( transform, anchorMin, transform.GetSizeLocal() + anchorMin, borderCollision );
    }
    public static void SetPositionAnchorWorld( this RectTransform transform, Vector2 anchorMin, bool borderCollision = false )
    {
        SetRect( transform, transform.GetParent().GetLocalPoint( anchorMin ), transform.GetParent().GetLocalPoint( transform.GetSizeLocal() + anchorMin ), borderCollision );
    }

    public static Vector2 GetPositionAnchorWorld( this RectTransform transform )
    {
        return transform.GetParent().GetWorldPoint( transform.anchorMin );
    }

    #endregion

    #region Pixel

    public static Vector2 GetPositionPixelLocal( this RectTransform transform )
    {
        return Vector2.Scale( transform.anchorMin, transform.GetParent().GetSizePixelWorld() );
    }

    public static Vector2 GetPositionPixelWorld( this RectTransform transform )
    {
        return transform.GetPixel( transform.GetParent().GetWorldPoint( transform.anchorMin ) );
    }

    public static void SetPositionPixelLocal( this RectTransform transform, Vector2 position )
    {
        transform.SetPositionAnchorLocal( Vector2.Scale( transform.GetPixelShiftLocal(), position ) );
    }

    #endregion

    #endregion


    #region Size

    #region Anchor

    public static Vector2 GetSizeLocal( this RectTransform transform )
    {
        return transform.anchorMax - transform.anchorMin + (transform.offsetMax-transform.offsetMin)/RustCanvas.refResolution;
    }

    public static Vector2 GetSizeWorld( this RectTransform transform )
    {
        var worldSize = GetSizeLocal( transform );
        foreach (var parent in GetParents( transform ))
            worldSize = Vector2.Scale( worldSize, parent.GetSizeLocal() );
        return worldSize;
    }

    public static void SetSizeWorld( this RectTransform transform, Vector2 size )
    {
        var worldAnchorMax = transform.GetParent().GetWorldPoint( transform.anchorMin + transform.offsetMin / RustCanvas.refResolution ) + size;
        var localAnchorMax = transform.GetParent().GetLocalPoint( worldAnchorMax );
        transform.SetRect( transform.anchorMin, localAnchorMax );
    }

    public static void SetSizeLocal( this RectTransform transform, Vector2 size )
    {
        transform.SetRect( transform.anchorMin, transform.anchorMin + size );
    }

    #endregion

    #region Pixel
    
    public static Vector2 GetSizePixelWorld( this RectTransform transform )
    {
        return transform.GetPixel( transform.GetSizeWorld() );
    }


    public static void SetSizePixel( this RectTransform transform, Vector2 size )
    {
        transform.SetRect( transform.anchorMin,
            transform.anchorMin + Vector2.Scale( transform.GetPixelShiftLocal(), size ) );
    }

    #endregion

    #endregion

    #region Mouse

    public static Vector2 GetMouseLocal( this RectTransform transform )
    {
        return transform.GetParent().GetLocalPoint( RustCanvas.GetMouseAnchor() );
    }

    #endregion

    #region Pixel

    public static Vector2 GetPixel( this RectTransform transform, Vector2 worldAnchor )
    {
        return Vector2.Scale( worldAnchor, ScreenVec );
    }

    public static Vector2 GetPixelShiftWorld( this RectTransform transform )
    {
        return Vector2.one.Div( ScreenVec );
    }

    public static Vector2 GetPixelShiftLocal( this RectTransform transform )
    {
        return ( ( Vector2.one).Div(Vector2.Scale( ScreenVec, ( (RectTransform) transform.parent ).GetSizeWorld()) ) );
    }


    public static Vector2 GetPixelShiftLocalForChild( this RectTransform transform )
    {
        return ( ( Vector2.one ).Div( Vector2.Scale( ScreenVec, ( transform ).GetSizeWorld() ) ) );
    }
    #endregion

    #endregion

    #region Transform Point

    public static Vector2 GetWorldPoint( this RectTransform transform, Vector2 localPoint )
    {
        foreach (var parent in GetHierarchy( transform ))
            localPoint = Vector2.Scale( localPoint, parent.GetSizeLocal() ) + parent.anchorMin;
        return localPoint;
    }

    public static Vector2 GetLocalPoint( this RectTransform transform, Vector2 point )
    {
        var hierarchy = GetHierarchy( transform );
        hierarchy.Reverse();
        foreach (var hierarchyElement in hierarchy)
        {
            point -= hierarchyElement.anchorMin + hierarchyElement.offsetMin / RustCanvas.refResolution;
            point = point.Div( hierarchyElement.GetSizeLocal() );
        }
        return point;
    }

    #endregion

    #region Hierarchy Functions

    private static List<RectTransform> GetHierarchy( RectTransform transform )
    {
        var hierarchy = GetParents( transform );
        if (transform.root != transform)
            hierarchy.Insert( 0, transform );
        return hierarchy;
    }

    private static List<RectTransform> GetParents( RectTransform transform )
    {
        var parents = new List<RectTransform>();
        RectTransform current = transform;
        while (true)
        {
            if (transform.root == current || transform.root == current.parent) break;
            current = (RectTransform) current.parent;
            parents.Add( current );
        }
        return parents;
    }

    public static RectTransform GetParent( this Transform transform )
    {
        return (RectTransform) transform.parent;
    }

    #endregion

    #region Pivot

    public static Vector2 GetPivotLocalPosition( this RectTransform transform, Vector2 pivot )
    {
        return transform.anchorMin+ transform.offsetMin / RustCanvas.refResolution + Vector2.Scale( transform.GetSizeLocal(), pivot );
    }

    public static Vector2 GetPivotPositionWorld( this RectTransform transform, Vector2 pivot )
    {
        return transform.GetParent().GetWorldPoint( transform.anchorMin+ transform.offsetMin / RustCanvas.refResolution+Vector2.Scale( transform.GetSizeLocal(),pivot) );
    }

    #endregion

    #region EventTrigger

    public static void Add( this EventTrigger eventTrigger, EventTriggerType eventID, Action callback )
    {
        EventTrigger.Entry eventEntry = new EventTrigger.Entry() { eventID = eventID };
        eventEntry.callback.AddListener( e => callback() );
        eventTrigger.triggers.Add( eventEntry );
    }

    #endregion

}
