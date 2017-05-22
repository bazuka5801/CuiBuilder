using System.Collections.Generic;
using System.Linq;
using Oxide.Game.Rust.Cui;
using UnityEngine;

public class RectTransformEditor : ComponentEditor<RectTransformComponent, CuiRectTransformComponent>
{
    public RectTransform m_SelectedTransform
    {
        get { return (RectTransform) InspectorView.SelectedItem.transform; }
    }

    private static IEnumerable<RectTransform> m_SelectedTransforms
    {
        get { return InspectorView.SelectedItems.Select( p => (RectTransform) p.transform ); }
    }

    [CuiField( "anchormin" )]
    private void OnAnchorMinChanged( object value )
    {
        OnAnchorChanged();
    }

    [CuiField( "anchormax" )]
    private void OnAnchorMaxChanged( object value )
    {
        OnAnchorChanged();
    }

    [CuiField( "position" )]
    private void OnPositionChanged( object value )
    {
        OnPixelChanged();
    }

    [CuiField( "size" )]
    private void OnSizeChanged( object value )
    {
        OnPixelChanged();
    }

    public void SendAnchorMinUpdate( Vector2 anchor )
    {
        GetField( "anchormin" ).SetValue( anchor );

        Dictionary<GameObject, Vector2> offsets = new Dictionary<GameObject, Vector2>();

        var selected = m_SelectedTransforms.Where( p => p != m_SelectedTransform ).ToList();
        var rootComponent = GetTransformComponent( m_SelectedTransform.gameObject );
        var centerPoint = m_SelectedTransform.GetParent().GetWorldPoint( Vector2Ex.Parse( rootComponent.AnchorMin ) );
        foreach (var rTransform in selected)
        {
            var anchorMin = Vector2Ex.Parse( GetTransformComponent( rTransform.gameObject ).AnchorMin );
            offsets[ rTransform.gameObject ] = rTransform.GetParent().GetWorldPoint( anchorMin ) - centerPoint;
        }

        base.OnFieldChanged( "anchormin", anchor );

        centerPoint = m_SelectedTransform.GetParent().GetWorldPoint( Vector2Ex.Parse( rootComponent.AnchorMin ) );

        foreach (var rTransform in selected)
        {
            var anchorMin = centerPoint + offsets[ rTransform.gameObject ];
            GetTransformComponent( rTransform.gameObject ).AnchorMin =Vector2Ex.ToString(rTransform.GetParent().GetLocalPoint(anchorMin));
            rTransform.SetPositionAnchorWorld( anchorMin );
        }
    }

    private static CuiRectTransformComponent GetTransformComponent( GameObject obj )
    {
        return CUIObject.Lookup[ obj ].GetCuiComponent<CuiRectTransformComponent>();
    }

    public void SendAnchorMaxUpdate( Vector2 anchor )
    {
        GetField( "anchormax" ).SetValue( anchor );

        Dictionary<GameObject, Vector2> offsets = new Dictionary<GameObject, Vector2>();

        var selected = m_SelectedTransforms.Where( p => p != m_SelectedTransform ).ToList();

        var rootComponent = GetTransformComponent( m_SelectedTransform.gameObject );
        var centerPoint = m_SelectedTransform.GetParent().GetWorldPoint( Vector2Ex.Parse( rootComponent.AnchorMax ) );
        foreach (var rTransform in selected)
        {
            var anchorMax = Vector2Ex.Parse( GetTransformComponent( rTransform.gameObject ).AnchorMax );
            offsets[ rTransform.gameObject ] = rTransform.GetParent().GetWorldPoint( anchorMax ) - centerPoint;
        }

        base.OnFieldChanged( "anchormax", anchor );

        centerPoint = m_SelectedTransform.GetParent().GetWorldPoint( Vector2Ex.Parse( rootComponent.AnchorMax ) );

        foreach (var rTransform in selected)
        {
            var point = centerPoint + offsets[ rTransform.gameObject ];
            var anchorMax = point;
            GetTransformComponent(rTransform.gameObject).AnchorMax =
                Vector2Ex.ToString(rTransform.GetParent().GetLocalPoint(anchorMax));
            rTransform.SetRectWorld( rTransform.GetParent().GetWorldPoint(rTransform.anchorMin), anchorMax );
        }
    }

    void OnAnchorChanged()
    {
        Vector2? anchorMin = GetField( "anchormin" ).GetValue() as Vector2?;
        if (anchorMin == null) return;
        var pixelLocalPosition = m_SelectedTransform.GetPositionPixelLocal();
        GetField( "position" ).SetValue( pixelLocalPosition );

        Vector2? anchorMax = GetField( "anchormax" ).GetValue() as Vector2?;
        if (anchorMax == null) return;
        var localSize = m_SelectedTransform.GetSizePixelWorld();
        GetField( "size" ).SetValue( localSize );
    }

    void OnPixelChanged()
    {
        GetField( "anchormin" ).SetValue( m_SelectedTransform.anchorMin );
        GetField( "anchormax" ).SetValue( m_SelectedTransform.anchorMax );
    }

    public override void Load( CuiRectTransformComponent component )
    {
        GetField( "anchormin" ).SetValue( component.AnchorMin );
        GetField( "anchormax" ).SetValue( component.AnchorMax );
        GetField( "offsetmin" ).SetValue( component.OffsetMin );
        GetField( "offsetmax" ).SetValue( component.OffsetMax );
        OnAnchorChanged();
    }
}
