using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class Combobox : InspectorField {
    [Serializable]
    public class OnComboboxChangedEvent : UnityEvent<object>
    { }

    [SerializeField] private Dropdown m_Dropdown;
    public OnComboboxChangedEvent OnValueChanged;

    private Type m_EnumType;

    private void Awake()
    {
        m_Dropdown.onValueChanged.AddListener(SendEventCallback);
    }

    private void SendEventCallback(int index)
    {
        if (OnValueChanged == null) return;
        var selectedItem = m_Dropdown.options[index].text;
        OnValueChanged.Invoke(m_EnumType != null ? Enum.Parse(m_EnumType, selectedItem) : selectedItem);
    }

    public void Init(Type enumType)
    {
        m_EnumType = enumType;
        m_Dropdown.AddOptions(Enum.GetNames(enumType).ToList());
    }

    public void Init(List<string> options)
    {
        m_Dropdown.AddOptions(options);
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
