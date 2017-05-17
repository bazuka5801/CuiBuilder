using UnityEngine;
using UnityEngine.UI;

public class FloatBox : InspectorField
{


    [SerializeField] private InputField m_InputField;

    private void Awake()
    {
        EnableEvents();
    }

    public override object GetValue()
    {
        float num;
        if (float.TryParse( m_InputField.text, out num ))
        {
            return num;
        }
        return m_InputField.text;
    }

    public override void SetValue( object value )
    {
        DisableEvents();
        m_InputField.text = value.ToString();
        EnableEvents();
    }

    public void DisableEvents()
    {
        m_InputField.onEndEdit.RemoveAllListeners();
    }

    public void EnableEvents()
    {
        m_InputField.onEndEdit.AddListener( s => { onChanged.Invoke( float.Parse( s ) ); } );
    }
}
