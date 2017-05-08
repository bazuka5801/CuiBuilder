using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Inspector.UIControls
{
    public class VectorInput : InspectorField {

        [SerializeField] private FloatBox m_InputBoxX, m_InputBoxY;

        private void Awake()
        {
            m_InputBoxX.AddListener(x => SendEventCallback());
            m_InputBoxY.AddListener(x => SendEventCallback());
        }

        public void Set(Vector2 vec)
        {
            m_InputBoxX.SetValue(vec.x.ToString("F4"));
            m_InputBoxY.SetValue(vec.y.ToString("F4"));
        }

        private void SendEventCallback()
        {
            onChanged.Invoke(GetValue());
        }
        private void DisableEvents()
        {
            m_InputBoxX.DisableEvents();
            m_InputBoxY.DisableEvents();
        }

        private void EnableEvents()
        {
            m_InputBoxX.EnableEvents();
            m_InputBoxY.EnableEvents();
        }

        public override object GetValue()
        {
            object x = m_InputBoxX.GetValue();
            object y = m_InputBoxY.GetValue();

            float? xFloat = x as float?;
            float? yFloat = y as float?;

            if (xFloat != null && yFloat != null)
            {
                return new Vector2((float)xFloat, (float)yFloat);
            }
            return x + " " + y;
        }

        public override void SetValue(object value)
        {
            DisableEvents();
            string strValue = value as string;
            Vector2 vec;
            if (strValue != null)
            {
                vec = Vector2Ex.Parse(strValue);
            }
            else if (value is Vector2)
            {
                vec = (Vector2)value;
            }
            else
            {
                throw new InvalidCastException();
            }
            m_InputBoxX.SetValue(vec.x);
            m_InputBoxY.SetValue(vec.y);
            EnableEvents();
        }
    }
}