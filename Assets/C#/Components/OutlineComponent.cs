using System;
using Oxide.Game.Rust.Cui;
using UnityEngine;
using UnityEngine.UI;

public sealed class OutlineComponent : BaseComponent<CuiOutlineComponent>
{
    private Outline m_Outline;

    protected override void Awake()
    {
        base.Awake();
        m_Outline = gameObject.AddComponent<Outline>();
    }

    private void OnDestroy()
    {
        DestroyImmediate( m_Outline );
    }

    protected override void Load( CuiOutlineComponent component )
    {
        OnDistanceChanged( component.Distance );
        OnUseGraphicAlphaChanged( component.UseGraphicAlpha );
        OnColorChanged( component.Color );
    }

    [CuiField( "distance" )]
    private void OnDistanceChanged( object value )
    {
        var type = value.GetType();
        if (type == typeof( Vector2 ))
        {
            var vec = (Vector2) value;
            m_Outline.effectDistance = vec;
            CuiComponent.Distance = Vector2Ex.ToString( vec );
        }
        else
        {
            var vec = Vector2Ex.Parse(value.ToString());
            if (vec != Vector2.zero)
            {
                OnDistanceChanged(vec);
                return;
            }
            m_Outline.effectDistance = new Vector2( 1, -1 );
            CuiComponent.Distance = value.ToString();
        }
    }

    [CuiField( "usegraphicalpha" )]
    private void OnUseGraphicAlphaChanged( object value )
    {
        var useGraphicAlpha = Convert.ToBoolean( value );
        m_Outline.useGraphicAlpha = useGraphicAlpha;
        CuiComponent.UseGraphicAlpha = useGraphicAlpha;
    }

    [CuiField( "color" )]
    private void OnColorChanged( object value )
    {
        m_Outline.effectColor = ColorEx.Parse( value.ToString() );
        CuiComponent.Color = value.ToString();
    }
}
