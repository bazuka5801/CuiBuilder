using UnityEngine;
using UnityEngine.UI;

namespace Inspector.UIControls
{
    public class InputBox : InspectorField
    {
        [SerializeField] private InputField m_InputField;

        private void Awake()
        {
            EnableEvents();
        }

        private void DisableEvents()
        {
            m_InputField.onEndEdit.RemoveAllListeners();
        }

        private void EnableEvents()
        {
            m_InputField.onEndEdit.AddListener( ( s ) => onChanged.Invoke( s ) );
        }


        public override object GetValue()
        {
            return m_InputField.text;
        }

        public override void SetValue( object value )
        {
            DisableEvents();
            m_InputField.text = value == null ? "" : value.ToString();
            EnableEvents();
        }
    }
}