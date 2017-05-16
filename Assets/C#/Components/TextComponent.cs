using Oxide.Game.Rust.Cui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public sealed class TextComponent : BaseComponent<CuiTextComponent>, IGraphicComponent
{
    private Text m_Text;

    protected override void Awake()
    {
        base.Awake();
        m_Text = gameObject.AddComponent<Text>();
        m_Text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
    }

    private void OnDestroy()
    {
        DestroyImmediate( m_Text );
    }

    [InspectorField("text")]
    private void OnTextChanged(object value)
    {
        var text = value.ToString();
        m_Text.text = text;
        CuiComponent.Text = text;
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
        if (!int.TryParse(text, out fontSize))
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
