using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(Combobox))]
public class ComboboxEditor : Editor
{


    public override void OnInspectorGUI()
    {
        var m_Combobox = target as Combobox;
        m_Combobox.Name = EditorGUILayout.TextField("Name", m_Combobox.Name);
        m_Combobox.m_Dropdown = (Dropdown) EditorGUILayout.ObjectField("Dropdown",m_Combobox.m_Dropdown, typeof(Dropdown), true);
        m_Combobox.m_EnumTypeFullName = EditorGUILayout.TextField( "Enum Type", m_Combobox.m_EnumTypeFullName );
        var type = Type.GetType(m_Combobox.m_EnumTypeFullName);
        if (type != null && type.BaseType == typeof(Enum))
        {
            EditorGUILayout.HelpBox( type.Name + " Found\n"+ string.Join( ", ", Enum.GetNames( type ) ), MessageType.Info );
        }
        else
        {
            EditorGUILayout.HelpBox( "Not found type", MessageType.Error );
        }
    }
}
