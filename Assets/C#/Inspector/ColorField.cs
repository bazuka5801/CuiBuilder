using UnityEngine;
using UnityEngine.UI;

public class ColorField : InspectorField {

    [SerializeField] private InputField m_InputField;

    private void Awake()
    {
        m_InputField.onEndEdit.AddListener( ( s ) => onChanged.Invoke( s ) );
    }

    /// <summary>
    /// Get Color
    /// </summary>
    /// <returns>Return color else string</returns>
    public override object GetValue()
    {
        Color32 color;
        if (HexColorField.HexToColor(m_InputField.text, out color))
        {
            return new Color(color.r, color.g, color.b, color.a);
        }
        return m_InputField.text;
    }

    /// <summary>
    /// Set color field value
    /// </summary>
    /// <param name="value">text</param>
    public override void SetValue(object value)
    {
        m_InputField.text = value.ToString();
    }
}
