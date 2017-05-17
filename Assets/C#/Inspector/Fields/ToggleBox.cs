using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleBox : InspectorField
{
    [SerializeField] private Toggle m_Toggle;

    public override object GetValue()
    {
        return m_Toggle.isOn;
    }

    public override void SetValue( object value )
    {
        m_Toggle.isOn = Convert.ToBoolean( value );
    }
}
