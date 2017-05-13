using Oxide.Game.Rust.Cui;
using UnityEngine;

public class RectTransformEditor : ComponentEditor<RectTransformComponent, CuiRectTransformComponent>
{
    public RectTransform m_SelectedTransform
    {
        get { return (RectTransform) InspectorView.Selected.transform; }
    }

    [InspectorField( "anchormin" )]
    private void OnAnchorMinChanged( object value )
    {
        OnAnchorChanged();
    }

    [InspectorField( "anchormax" )]
    private void OnAnchorMaxChanged( object value )
    {
        OnAnchorChanged();
    }

    [InspectorField( "position" )]
    private void OnPositionChanged( object value )
    {
        OnPixelChanged();
    }

    [InspectorField( "size" )]
    private void OnSizeChanged( object value )
    {
        OnPixelChanged();
    }

    public void SendAnchorMinUpdate( Vector2 anchor )
    {
        GetField( "anchormin" ).SetValue( anchor );
        base.OnFieldChanged("anchormin", anchor);
    }

    public void SendAnchorMaxUpdate( Vector2 anchor )
    {
        GetField( "anchormax" ).SetValue( anchor );
        base.OnFieldChanged( "anchormax", anchor );
    }

    void OnAnchorChanged()
    {
        var rTransform = ((RectTransform) InspectorView.Selected.transform);
        Vector2? anchorMin = GetField( "anchormin" ).GetValue() as Vector2?;
        if (anchorMin == null) return;
        var pixelLocalPosition = rTransform.GetPositionPixelLocal();
        GetField( "position" ).SetValue( pixelLocalPosition );

        Vector2? anchorMax = GetField( "anchormax" ).GetValue() as Vector2?;
        if (anchorMax == null) return;
        var localSize = rTransform.GetSizePixelLocal();
        GetField( "size" ).SetValue( localSize );

    }

    void OnPixelChanged()
    {
        var pos = GetField( "position" ).GetValue() as Vector2?;
        var size = GetField( "size" ).GetValue() as Vector2?;
        GetField( "anchormin" ).SetValue( m_SelectedTransform.anchorMin );
        GetField( "anchormax" ).SetValue( m_SelectedTransform.anchorMax );
    }

    public override void Load(CuiRectTransformComponent component)
    {
        GetField("anchormin").SetValue( component.AnchorMin );
        GetField("anchormax").SetValue( component.AnchorMax );
        GetField("offsetmin").SetValue(component.OffsetMin);
        GetField("offsetmax").SetValue(component.OffsetMax);
        OnAnchorChanged();
    }
}
