using Oxide.Game.Rust.Cui;
using UnityEngine;

public sealed class RectTransformComponent : BaseComponent<CuiRectTransformComponent>
{
    private RectTransform m_Transform;

    protected override void Awake()
    {
        base.Awake();
        m_Transform = (RectTransform) base.transform;
    }

    protected override void Load( CuiRectTransformComponent component )
    {
        OnAnchorMinChanged(component.AnchorMin);
        OnAnchorMaxChanged(component.AnchorMax);
    }

    [CuiField( "anchormin" )]
    private void OnAnchorMinChanged( object value )
    {
        var type = value.GetType();
        if (type == typeof( Vector2 ))
        {
            var vec = (Vector2) value;
            m_Transform.SetPositionAnchorLocal( vec );
            CuiComponent.AnchorMin = string.Format( "{0} {1}", vec.x, vec.y );
        }
        else
        {
            var vec = Vector2Ex.Parse( value.ToString() );
            if (vec != Vector2.zero)
            {
                OnAnchorMinChanged( vec );
                return;
            }
            m_Transform.SetPositionAnchorLocal( new Vector2( 0.1f, 0.1f ) );
            CuiComponent.AnchorMin = value.ToString();
        }
    }

    [CuiField( "anchormax" )]
    private void OnAnchorMaxChanged( object value )
    {
        var type = value.GetType();
        if (type == typeof( Vector2 ))
        {
            var vec = (Vector2) value;
            m_Transform.anchorMax = (Vector2) value;
            CuiComponent.AnchorMax = string.Format( "{0} {1}", vec.x, vec.y );
        }
        else
        {
            var vec = Vector2Ex.Parse( value.ToString() );
            if (vec != Vector2.zero)
            {
                OnAnchorMaxChanged( vec );
                return;
            }
            m_Transform.SetSizePixel( new Vector2( 100f, 100f ) );
            CuiComponent.AnchorMax = value.ToString();
        }
    }

    [CuiField( "position" )]
    private void OnPositionChanged( object value )
    {
        var type = value.GetType();
        if (type == typeof( Vector2 ))
        {
            m_Transform.SetPositionPixelLocal( (Vector2) value );

            CuiComponent.AnchorMin = string.Format( "{0} {1}", m_Transform.anchorMin.x, m_Transform.anchorMin.y );
            CuiComponent.AnchorMax = string.Format( "{0} {1}", m_Transform.anchorMax.x, m_Transform.anchorMax.y );
        }
    }
    [CuiField( "size" )]
    private void OnSizeChanged( object value )
    {
        var type = value.GetType();
        if (type == typeof( Vector2 ))
        {
            m_Transform.SetSizePixel( (Vector2) value );

            CuiComponent.AnchorMax = string.Format( "{0} {1}", m_Transform.anchorMax.x, m_Transform.anchorMax.y );
        }
    }


    [CuiField( "offsetmin" )]
    private void OnOffsetMinChanged( object value )
    {
        var type = value.GetType();
        if (type == typeof( Vector2 ))
        {
            var vec = (Vector2) value;
            m_Transform.offsetMin = (Vector2) value;
            CuiComponent.OffsetMin = string.Format( "{0} {1}", vec.x, vec.y );
        }
        else
        {
            m_Transform.offsetMin = Vector2.zero;
            CuiComponent.OffsetMin = value.ToString();
        }
    }

    [CuiField( "offsetmax" )]
    private void OnOffsetMaxChanged( object value )
    {
        var type = value.GetType();
        if (type == typeof( Vector2 ))
        {
            var vec = (Vector2) value;
            m_Transform.offsetMax = (Vector2) value;
            CuiComponent.OffsetMax = string.Format( "{0} {1}", vec.x, vec.y );
        }
        else
        {
            m_Transform.offsetMax = Vector2.zero;
            CuiComponent.OffsetMax = value.ToString();
        }
    }

    public void SetPixelLocalPosition(Vector2 pos)
    {
        OnPositionChanged(pos);
    }
    public void SetPixelSize( Vector2 pos )
    {
        OnSizeChanged( pos );
    }
}
