using UnityEngine;

public class RectTransformEditor : ComponentEditor<RectTransformComponent>
{



    public void SendAnchorMinUpdate( Vector2 anchor )
    {
        GetField( "anchormin" ).SetValue( anchor );
        OnAnchorChanged();
    }
    public void SendAnchorMaxUpdate( Vector2 anchor )
    {
        GetField( "anchormax" ).SetValue( anchor );
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

    void OnAnchorChanged()
    {
        Vector2? anchorMin = GetField( "anchormin" ).GetValue() as Vector2?;
        if (anchorMin == null) return;
        var pixelLocalPosition = Interact.Selected.GetPositionPixelLocal();
        GetField( "position" ).SetValue( pixelLocalPosition );

        Vector2? anchorMax = GetField( "anchormax" ).GetValue() as Vector2?;
        if (anchorMax == null) return;
        var localSize = Interact.Selected.GetSizePixelLocal();
        GetField( "size" ).SetValue( localSize );

    }
    void OnPixelChanged()
    {
        var pos = GetField( "position" ).GetValue() as Vector2?;
        var size = GetField( "size" ).GetValue() as Vector2?;
        GetField( "anchormin" ).SetValue( Interact.Selected.anchorMin );
        GetField( "anchormax" ).SetValue( Interact.Selected.anchorMax );

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
}
