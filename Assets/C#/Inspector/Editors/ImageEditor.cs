using System.Collections;
using System.Collections.Generic;
using Oxide.Game.Rust.Cui;
using UnityEngine;

public class ImageEditor : ComponentEditor<ImageComponent, CuiRawImageComponent>
{
    public override void Load(CuiRawImageComponent component)
    {
        GetField("sprite").SetValue(component.Sprite);
        GetField("material").SetValue(component.Material);
        GetField("color").SetValue(component.Color);
        GetField("png").SetValue(component.Png);
        GetField("fadein").SetValue(component.FadeIn);
    }
}
