using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutGroupEditor : ComponentEditor<LayoutGroupComponent, CuiLayoutGroupComponent>
{
    public override void Load( CuiLayoutGroupComponent component )
    {
        GetField( "mode" ).SetValue( (int) component.Mode );
        GetField( "spacing" ).SetValue( component.Spacing );
    }
}
