using Oxide.Game.Rust.Cui;
using UnityEngine;

public sealed class RectTransformComponent : BaseComponent<CuiRectTransformComponent>, IPoolHandler
{
    public delegate void OnRectTransformChangedEvent();
    private RectTransform m_Transform;

    public event OnRectTransformChangedEvent OnChanged;

    protected override void Awake()
    {
        base.Awake();
        m_Transform = (RectTransform) base.transform;
    }

    protected override void Load( CuiRectTransformComponent component )
    {
        OnAnchorMinChanged(component.AnchorMin);
        OnAnchorMaxChanged(component.AnchorMax);
        OnOffsetMinChanged( component.OffsetMin );
        OnOffsetMaxChanged( component.OffsetMax );
    }

    public void SetRect(Vector2 anchorMin, Vector2 anchorMax)
    {
        OnAnchorMinChanged(anchorMin);
        OnAnchorMaxChanged(anchorMax);
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
            if (value.ToString().IsVector())
            {
                var vec = Vector2Ex.Parse(value.ToString());
                    OnAnchorMinChanged(vec);
                    return;
            }
            m_Transform.SetPositionAnchorLocal( new Vector2( 0.1f, 0.1f ) );
            CuiComponent.AnchorMin = value.ToString();
        }
        if (OnChanged != null) OnChanged.Invoke();
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
            if (value.ToString().IsVector())
            {
                var vec = Vector2Ex.Parse( value.ToString() );
                OnAnchorMaxChanged( vec );
                return;
            }
            m_Transform.SetSizePixel( new Vector2( 100f, 100f ) );
            CuiComponent.AnchorMax = value.ToString();
        }
        if (OnChanged != null) OnChanged.Invoke();
    }

    [CuiField( "position" )]
    private void OnPositionChanged( object value )
    {
        if (value is string && value.ToString().IsVector())
        {
            value = Vector2Ex.Parse( value.ToString() );
        }
        if (value is Vector2)
        {
            m_Transform.SetPositionPixelLocal( (Vector2) value );

            CuiComponent.AnchorMin = string.Format( "{0} {1}", m_Transform.anchorMin.x, m_Transform.anchorMin.y );
            CuiComponent.AnchorMax = string.Format( "{0} {1}", m_Transform.anchorMax.x, m_Transform.anchorMax.y );

            if (OnChanged != null) OnChanged.Invoke();
        }
    }
    [CuiField( "size" )]
    private void OnSizeChanged( object value )
    {
        if (value is string && value.ToString().IsVector())
        {
            value = Vector2Ex.Parse(value.ToString());
        }
        if (value is Vector2)
        {
            m_Transform.SetSizePixel( (Vector2) value );

            CuiComponent.AnchorMax = string.Format( "{0} {1}", m_Transform.anchorMax.x, m_Transform.anchorMax.y );

            if (OnChanged != null) OnChanged.Invoke();
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
            if (value.ToString().IsVector())
            {
                var vec = Vector2Ex.Parse( value.ToString() );
                OnOffsetMinChanged( vec );
                return;
            }
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
            if (value.ToString().IsVector())
            {
                var vec = Vector2Ex.Parse( value.ToString() );
                OnOffsetMaxChanged( vec );
                return;
            }
            m_Transform.offsetMax = Vector2.zero;
            CuiComponent.OffsetMax = value.ToString();
        }
    }

    public void SetPixelLocalPosition(Vector2 pos)
    {
        OnPositionChanged(pos);
    }
    public void SetPixelSize( Vector2 size )
    {
        OnSizeChanged( size );
    }

    public void OnPoolEnter()
    {
        m_Transform.offsetMin = m_Transform.offsetMax = Vector2.zero;
        OnChanged = null;
    }

    public void OnPoolLeave()
    {

    }
}
