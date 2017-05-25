using UnityEngine;
using System.Collections;

public class LayoutElementEditor : ComponentEditor<LayoutElementComponent, CuiLayoutElementComponent>
{
    public override void Load(CuiLayoutElementComponent component)
    {
        GetField("weight").SetValue(component.Weight);
    }
}
