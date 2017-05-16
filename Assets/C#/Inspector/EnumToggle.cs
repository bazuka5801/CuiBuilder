using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnumToggle : InspectorField
{
    public GameObject m_Template;
    public string m_EnumTypeFullName;
    private Type m_EnumType;
    List<GameObject> toggles = new List<GameObject>();

    void Awake()
    {
        foreach (var enumValue in Enum.GetNames( m_EnumType = Type.GetType( m_EnumTypeFullName ) ))
        {
            var obj = Instantiate(m_Template);
            obj.transform.SetParent(m_Template.transform.parent, false);
            obj.SetActive(true);
            obj.GetComponentInChildren<Text>().text = enumValue;
            obj.name = enumValue;
            obj.GetComponent<Toggle>().onValueChanged.AddListener(p =>
            {
                if (p)
                {
                    onChanged.Invoke( (int) Enum.Parse( m_EnumType, obj.name ) );
                }
            });
            toggles.Add(obj);
            
        }
    }

    public override object GetValue()
    {
        return (int)Enum.Parse(m_EnumType, GetComponent<ToggleGroup>().ActiveToggles().Single().name);
    }

    public override void SetValue(object value)
    {
        m_Template.transform.parent.FindChild(Enum.GetNames(m_EnumType)[Convert.ToInt32(value.ToString())]).GetComponent<Toggle>().isOn =
            true;
    }
}
