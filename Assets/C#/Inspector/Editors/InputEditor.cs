﻿using System.Collections;
using System.Collections.Generic;
using Oxide.Game.Rust.Cui;
using UnityEngine;

public class InputEditor : ComponentEditor<InputComponent, CuiInputFieldComponent>
{
    public override void Load(CuiInputFieldComponent component)
    {
        GetField( "fontsize" ).SetValue( component.FontSize );
        GetField( "font" ).SetValue( component.FontSize );
        GetField( "align" ).SetValue( (int) component.Align );
        GetField( "color" ).SetValue( component.Color );
        GetField( "charlimit" ).SetValue( component.CharsLimit );
        GetField( "command" ).SetValue( component.Command );
    }
}
