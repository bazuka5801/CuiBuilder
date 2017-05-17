using Oxide.Game.Rust.Cui;
using UnityEngine.UI;

public sealed class ImageComponent : BaseComponent<CuiRawImageComponent>, IGraphicComponent
{
    private RawImage m_Image;

    protected override void Awake()
    {
        base.Awake();
        m_Image = gameObject.AddComponent<RawImage>();

    }

    private void OnDestroy()
    {
        DestroyImmediate( m_Image );
    }

    [InspectorField( "sprite" )]
    private void OnSpriteChanged( object value )
    {
        CuiComponent.Sprite = value.ToString();
    }

    [InspectorField( "material" )]
    private void OnMaterialChanged( object value )
    {
        CuiComponent.Material = value.ToString();
    }

    [InspectorField( "color" )]
    private void OnColorChanged( object value )
    {
        CuiComponent.Color = value.ToString();
        m_Image.color = ColorEx.Parse( value.ToString() );
    }

    [InspectorField( "png" )]
    private void OnPngChanged( object value )
    {
        CuiComponent.Png = value.ToString();
    }

    [InspectorField( "fadein" )]
    private void OnFadeInChanged( object value )
    {
        float fadeIn;
        if (!float.TryParse( value.ToString(), out fadeIn ))
        {
            fadeIn = 0f;
        }
        CuiComponent.FadeIn = fadeIn;
    }

}
