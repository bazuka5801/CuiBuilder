using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class InspectorFieldAttribute : Attribute
{
    public string FieldName { get { return m_fieldName; } }
    private string m_fieldName;

    public InspectorFieldAttribute(string fieldName)
    {
        this.m_fieldName = fieldName;
    }
}

public abstract class ComponentEditor : MonoBehaviour
{
   
    
    [SerializeField] private Dictionary<string, InspectorField> m_Fields = new Dictionary<string, InspectorField>();

    private bool m_Active;
    [SerializeField] private bool m_Fixed;

    private Dictionary<string, MethodInfo> m_Hooks = new Dictionary<string, MethodInfo>();
        
    protected InspectorField GetField(string fieldName)
    {
        return m_Fields[fieldName];
    }
        
    public bool IsActive()
    {
        return m_Active;
    }
        
    protected virtual void Awake()
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

    protected virtual void InitializeHooks()
    {
        m_Hooks = GetInspectorFieldHandlers(GetType());
    }

    private void AddChangedHandlers()
    {
        foreach (Transform child in transform)
        {
            var field = child.GetComponent<InspectorField>();
            if (field != null)
            {
                m_Fields.Add(field.Name, field);
                field.AddListener(obj => OnFieldChanged(field.Name, obj));
            }
        }
    }
    
    protected virtual void OnFieldChanged(string field, object value)
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

    protected Dictionary<string, MethodInfo> GetInspectorFieldHandlers(Type type)
    {
        return type
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.GetCustomAttributes(typeof(InspectorFieldAttribute), false).Length > 0)
            .ToDictionary(
                m => ((InspectorFieldAttribute)m.GetCustomAttributes(typeof(InspectorFieldAttribute), false)[0]).FieldName,
                m => m);
    }

    protected Dictionary<string, MethodInfo> GetInspectorFieldHandlers<T>()
    {
        return GetInspectorFieldHandlers(typeof(T));
    }
}

public class ComponentEditor<T> : ComponentEditor
    where T : BaseComponent
{
    private Dictionary<string, MethodInfo> m_ComponentHooks = new Dictionary<string, MethodInfo>();

    private static ComponentEditor<T> m_Instance;

    public static ComponentEditor<T> Instance()
    {
        return m_Instance;
    }

    protected override void Awake()
    {
        base.Awake();
        m_Instance = this;
    }

    protected override void InitializeHooks()
    {
        base.InitializeHooks();
        m_ComponentHooks = GetInspectorFieldHandlers<T>();
    }

    protected override void OnFieldChanged(string field, object value)
    {
        MethodInfo method;
        if (m_ComponentHooks.TryGetValue(field, out method))
        {
            foreach (var comp in InspectorView.GetSelectedComponents<T>())
            {
                method.Invoke(comp, new object[] {value});
            }
        }
        else
        {
            throw new Exception("Field doesnt have handler in component");
        }
        base.OnFieldChanged(field, value);
    }
}
