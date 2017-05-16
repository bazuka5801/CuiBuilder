using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class InspectorField : MonoBehaviour
{
#if UNITY_EDITOR
    private Text m_Label;
    void OnValidate()
    {
        if (!m_Label)
        {
            m_Label = GetComponentInChildren<Text>();
        }
        if (m_Label)
        {
            m_Label.text = gameObject.name;
        }
    }
#endif

    public string Name;
    [Serializable]
    public class OnInspectorFieldChangedEvent : UnityEvent<object>
    { }
    public abstract object GetValue();
    public abstract void SetValue(object value);

    [SerializeField]
    protected OnInspectorFieldChangedEvent onChanged;

    public virtual void AddListener(UnityAction<object> callback)
    {
        this.onChanged.AddListener(callback);
    }
}
