using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class Combobox : InspectorField {

    [SerializeField]
    public Dropdown m_Dropdown;
    [SerializeField]
    public string m_EnumTypeFullName;

    private Type m_EnumType;
    private void Awake()
    {
        m_Dropdown.onValueChanged.AddListener(SendEventCallback);
        m_Dropdown.AddOptions( Enum.GetNames( m_EnumType = Type.GetType(m_EnumTypeFullName) ).ToList() );
    }

    private void SendEventCallback(int index)
    {
        var selectedItem = m_Dropdown.options[index].text;
        onChanged.Invoke(m_EnumType != null ? Enum.Parse(m_EnumType, selectedItem) : selectedItem);
    }

    public override object GetValue()
    {
        return m_Dropdown.value;
    }

    public override void SetValue(object value)
    {
        DisableEvents();
        m_Dropdown.value = Convert.ToInt32(value);
        EnableEvents();
    }
    private void DisableEvents()
    {
        m_Dropdown.onValueChanged.RemoveAllListeners();
    }

    private void EnableEvents()
    {
        m_Dropdown.onValueChanged.AddListener(s => { onChanged.Invoke(s); });
    }
}
