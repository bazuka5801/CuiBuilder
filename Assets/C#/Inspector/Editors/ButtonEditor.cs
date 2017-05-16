using System.Collections;
using System.Collections.Generic;
using Oxide.Game.Rust.Cui;
using UnityEngine;

public class ButtonEditor : ComponentEditor<ButtonComponent, CuiButtonComponent>
{
    public override void Load(CuiButtonComponent component)
    {
        GetField("command").SetValue(component.Command);
        GetField("close").SetValue(component.Close);
        GetField("sprite").SetValue(component.Sprite);
        GetField("material").SetValue(component.Material);
        GetField("color").SetValue(component.Color);
    }
}
