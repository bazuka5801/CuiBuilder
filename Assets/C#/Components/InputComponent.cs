using Oxide.Game.Rust.Cui;
using UnityEngine;
using UnityEngine.UI;

public class InputComponent : BaseComponent<CuiInputFieldComponent>, IGraphicComponent, ISelectableComponent
{
    private Text m_Text;

    protected override void Awake()
    {
        base.Awake();
        m_Text = gameObject.AddComponent<Text>();
        m_Text.font = Resources.GetBuiltinResource<Font>( "Arial.ttf" );
        m_Text.text = "Text";
    }

    private void OnDestroy()
    {
        DestroyImmediate( m_Text );
    }

    [InspectorField( "font" )]
    private void OnFontChanged( object value )
    {
        CuiComponent.Font = value.ToString();
    }

    [InspectorField( "fontsize" )]
    private void OnFontSizeChanged( object value )
    {
        var text = value.ToString();
        int fontSize;
        if (!int.TryParse( text, out fontSize ))
        {
            fontSize = 12;
        }
        m_Text.fontSize = fontSize;
        CuiComponent.FontSize = fontSize;
    }

    [InspectorField( "align" )]
    private void OnAlignChanged( object value )
    {
        var textAnchor = (TextAnchor) value;
        m_Text.alignment = textAnchor;
        CuiComponent.Align = textAnchor;
    }

    [InspectorField( "color" )]
    private void OnColorChanged( object value )
    {
        m_Text.color = ColorEx.Parse( value.ToString() );
        CuiComponent.Color = value.ToString();
    }

    [InspectorField( "charlimit" )]
    private void OnCharLimitChanged( object value )
    {
        var text = value.ToString();
        int charLimit;
        if (!int.TryParse( text, out charLimit ))
        {
            charLimit = 12;
        }
        CuiComponent.CharsLimit = charLimit;
    }

    [InspectorField( "command" )]
    private void OnCommandChanged( object value )
    {
        CuiComponent.Command = value.ToString();
    }
}
