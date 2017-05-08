using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class ComponentEditor : MonoBehaviour
{
    public class InspectorFieldAttribute : Attribute
    {
        public string FieldName { get { return m_fieldName; } }
        private string m_fieldName;

        public InspectorFieldAttribute(string fieldName)
        {
            this.m_fieldName = fieldName;
        }
    }

    private bool m_Active;
    [SerializeField] private List<InspectorField> m_Fields = new List<InspectorField>();
    [SerializeField] private bool m_Fixed;

    private Dictionary<string, MethodInfo> m_Hooks = new Dictionary<string, MethodInfo>();

    public bool IsActive()
    {
        return m_Active;
    }

    private void Awake()
    {
        if (!m_Fixed)
        {
            GetComponentInChildren<Toggle>().onValueChanged.AddListener(OnStateChanged);
        }
        InitializeHooks();
        AddChangedHandlers();
    }

    private void OnStateChanged(bool state)
    {
        m_Active = state;
        foreach (Transform field in base.transform)
        {
            if (field.tag != "Header")
            {
                field.gameObject.SetActive(state);
            }
        }
    }

    private void InitializeHooks()
    {
        m_Hooks = GetType()
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.GetCustomAttributes(typeof(InspectorFieldAttribute), false).Length > 0)
            .ToDictionary(
                m => ((InspectorFieldAttribute)m.GetCustomAttributes(typeof(InspectorFieldAttribute), false)[0]).FieldName,
                m => m);
    }

    private void AddChangedHandlers()
    {
        foreach (Transform child in transform)
        {
            var field = child.GetComponent<InspectorField>();
            if (field != null)
            {
                field.AddListener(obj => OnFieldChanged(field.Name, obj));
            }
        }
    }

    private void OnFieldChanged(string field, object value)
    {
        MethodInfo method;
        if (m_Hooks.TryGetValue(field, out method))
        {
            method.Invoke(this, new object[] {value});
        }
        else
        {
            throw new Exception("Field doesnt have handler");
        }
    }
}
