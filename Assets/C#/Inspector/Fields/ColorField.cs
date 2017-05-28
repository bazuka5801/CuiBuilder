using Oxide.Game.Rust.Cui;
using UnityEngine;
using UnityEngine.UI;

public class ColorField : InspectorField
{

    [SerializeField] private InputField m_InputField;
    private bool lockedObject = false;
    private void Awake()
    {
        GetComponent<ColorPicker>().CurrentColor = Color.white;
        m_InputField.onEndEdit.AddListener( ( s ) =>
        {
            if (lockedObject) return;
            Color32 color32;
             if (HexColorField.HexToColor( s, out color32 ))
             {
                 Color color = color32;
                 onChanged.Invoke( string.Format( "{0} {1} {2} {3}", color.r, color.g, color.b, color.a ) );
                 return;
             }
             onChanged.Invoke( s );
         } );
        GetComponent<ColorPicker>().onValueChanged.AddListener( color =>
        {
            if (lockedObject) return;
             onChanged.Invoke( string.Format( "{0} {1} {2} {3}", color.r, color.g, color.b, color.a ) );
         } );
    }

    /// <summary>
    /// Get Color
    /// </summary>
    /// <returns>Return color else string</returns>
    public override object GetValue()
    {
        Color32 color32;
        if (HexColorField.HexToColor( m_InputField.text, out color32 ))
        {
            Color color = color32;
            return color;
        }
        return m_InputField.text;
    }

    /// <summary>
    /// Set color field value
    /// </summary>
    /// <param name="value">text</param>
    public override void SetValue( object value )
    {
        lockedObject = true;
        if (value.ToString().Split().Length == 4)
        {
            value = ColorEx.Parse( value.ToString() ).ToRGBHex();
        }
        Color32 color32;
        if (value is string && HexColorField.HexToColor( value.ToString(), out color32 ))
        {
            Color color = color32;
            GetComponent<ColorPicker>().CurrentColor = color;
            lockedObject = false;
            return;
        }
        m_InputField.text = value.ToString();
        lockedObject = false;
    }
}
