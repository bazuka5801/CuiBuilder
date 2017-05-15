using System.Collections;
using System.Collections.Generic;
using Oxide.Game.Rust.Cui;
using UnityEngine;

public class TextEditor : ComponentEditor<TextComponent, CuiTextComponent>
{
    public override void Load(CuiTextComponent component)
    {
        GetField("text").SetValue(component.Text);
        GetField("fontsize").SetValue(component.FontSize);
        GetField("align").SetValue((int) component.Align);
        GetField("color").SetValue(component.Color);
        GetField("fadein").SetValue(component.FadeIn);
    }
}
