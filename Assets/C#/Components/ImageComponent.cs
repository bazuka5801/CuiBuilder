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

    protected override void Load( CuiRawImageComponent component )
    {
        OnSpriteChanged( component.Sprite );
        OnMaterialChanged( component.Material );
        OnColorChanged( component.Color );
        OnPngChanged( component.Png );
        OnFadeInChanged( component.FadeIn );
    }

    [CuiField( "sprite" )]
    private void OnSpriteChanged( object value )
    {
        CuiComponent.Sprite = value.ToString();
    }

    [CuiField( "material" )]
    private void OnMaterialChanged( object value )
    {
        CuiComponent.Material = value.ToString();
    }

    [CuiField( "color" )]
    private void OnColorChanged( object value )
    {
        CuiComponent.Color = value.ToString();
        m_Image.color = ColorEx.Parse( value.ToString() );
    }

    [CuiField( "png" )]
    private void OnPngChanged( object value )
    {
        var url = value.ToString();
        CuiComponent.Png = url;
        m_Image.texture = ImageStorage.Get(url);
    }

    [CuiField( "fadein" )]
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
