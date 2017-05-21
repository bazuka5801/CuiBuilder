using Oxide.Game.Rust.Cui;
using UnityEngine.UI;

public sealed class ButtonComponent : BaseComponent<CuiButtonComponent>, IGraphicComponent, ISelectableComponent
{
    private Button m_Button;
    private RawImage m_Image;

    protected override void Awake()
    {
        base.Awake();
        m_Button = gameObject.AddComponent<Button>();
        m_Button.interactable = false;
        m_Image = gameObject.AddComponent<RawImage>();
    }

    private void OnDestroy()
    {
        DestroyImmediate( m_Button );
        DestroyImmediate( m_Image );
    }

    protected override void Load(CuiButtonComponent component )
    {
        OnCommandChanged(component.Command);
        OnCloseChanged(component.Close);
        OnSpriteChanged( component.Sprite );
        OnMaterialChanged( component.Material );
        OnColorChanged(component.Color);
    }

    [CuiField( "command" )]
    private void OnCommandChanged( object value )
    {
        CuiComponent.Command = value.ToString();
    }

    [CuiField( "close" )]
    private void OnCloseChanged( object value )
    {
        CuiComponent.Close = value.ToString();
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
        m_Image.color = ColorEx.Parse( value.ToString() );
        CuiComponent.Color = value.ToString();
    }
}
