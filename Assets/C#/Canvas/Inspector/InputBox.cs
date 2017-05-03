using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Inspector.UIControls
{
    public class InputBox : MonoBehaviour
    {
        [SerializeField] private InputField m_InputField;

        public InputField.SubmitEvent OnEndEdit;
        public InputField.OnChangeEvent OnValueChanged;

        private void Awake()
        {
            EnableEvents();
        }

        public string Get()
        {
            return m_InputField.text;
        }

        public void Set(string value)
        {
            DisableEvents();
            m_InputField.text = value;
            EnableEvents();
        }

        private void DisableEvents()
        {
            m_InputField.onEndEdit.RemoveAllListeners();
            m_InputField.onValueChanged.RemoveAllListeners();
        }

        private void EnableEvents()
        {
             m_InputField.onEndEdit = OnEndEdit;
            m_InputField.onValueChanged = OnValueChanged;
        }


    }
}