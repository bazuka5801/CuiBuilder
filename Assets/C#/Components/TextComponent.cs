using Oxide.Game.Rust.Cui;
using UnityEngine;
using UnityEngine.UI;

public sealed class TextComponent : BaseComponent<CuiTextComponent>, IGraphicComponent
{
    private Text m_Text;

    protected override void Awake()
    {
        base.Awake();
        m_Text = gameObject.AddComponent<Text>();
        m_Text.font = Resources.GetBuiltinResource<Font>( "Arial.ttf" );
    }

    private void OnDestroy()
    {
        DestroyImmediate( m_Text );
    }

    protected override void Load( CuiTextComponent component )
    {
        OnTextChanged( component.Text );
        OnFontChanged( component.Font );
        OnFontSizeChanged( component.FontSize );
        OnAlignChanged( (int)component.Align );
        OnColorChanged( component.Color );
        OnFadeInChanged( component.FadeIn );
    }

    [CuiField( "text" )]
    private void OnTextChanged( object value )
    {
        var text = value.ToString();
        m_Text.text = text;
        CuiComponent.Text = text;
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
