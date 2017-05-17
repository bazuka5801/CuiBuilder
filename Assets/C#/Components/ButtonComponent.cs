using Oxide.Game.Rust.Cui;
using UnityEngine.UI;

public class ButtonComponent : BaseComponent<CuiButtonComponent>, IGraphicComponent, ISelectableComponent
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

    [InspectorField( "command" )]
    private void OnCommandChanged( object value )
    {
        CuiComponent.Command = value.ToString();
    }

    [InspectorField( "close" )]
    private void OnCloseChanged( object value )
    {
        CuiComponent.Close = value.ToString();
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
        m_Image.color = ColorEx.Parse( value.ToString() );
        CuiComponent.Color = value.ToString();
    }
}
