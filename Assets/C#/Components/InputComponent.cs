using System;
using Oxide.Game.Rust.Cui;
using UnityEngine;
using UnityEngine.UI;

public sealed class InputComponent : BaseComponent<CuiInputFieldComponent>, IGraphicComponent, ISelectableComponent
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

    protected override void Load( CuiInputFieldComponent component )
    {
        OnTextChanged( component.Text );
        OnFontChanged( component.Font );
        OnFontSizeChanged( component.FontSize );
        OnAlignChanged( (int)component.Align );
        OnColorChanged( component.Color );
        OnCharLimitChanged( component.CharsLimit );
        OnCommandChanged( component.Command );
        OnPasswordChanged( component.IsPassword );
    }

    [CuiField( "text" )]
    private void OnTextChanged( object value )
    {
        CuiComponent.Text = value.ToString();
    }

    [CuiField( "font" )]
    private void OnFontChanged( object value )
    {
        CuiComponent.Font = value.ToString();
        m_Text.font = FindObjectOfType<RustCanvas>().Fonts[value.ToString() == "robotocondensed-bold.ttf" ? 1 : 0];
    }

    [CuiField( "fontsize" )]
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

    [CuiField( "align" )]
    private void OnAlignChanged( object value )
    {
        var textAnchor = (TextAnchor) value;
        m_Text.alignment = textAnchor;
        CuiComponent.Align = textAnchor;
    }

    [CuiField( "color" )]
    private void OnColorChanged( object value )
    {
        m_Text.color = ColorEx.Parse( value.ToString() );
        CuiComponent.Color = value.ToString();
    }

    [CuiField( "charlimit" )]
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

    [CuiField( "command" )]
    private void OnCommandChanged( object value )
    {
        CuiComponent.Command = value.ToString();
    }

    [CuiField( "password" )]
    private void OnPasswordChanged( object value )
    {
        CuiComponent.IsPassword = Convert.ToBoolean( value );
    }
}
