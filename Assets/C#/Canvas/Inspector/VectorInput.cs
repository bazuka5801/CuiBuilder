using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Inspector.UIControls
{
    public class VectorInput : MonoBehaviour {
        [Serializable]
        public class OnVectorChangedEvent : UnityEvent<object>
        { }

        [SerializeField] private FloatBox m_InputBoxX, m_InputBoxY;
        public OnVectorChangedEvent OnEndEdit, OnValueChanged;

        private void Awake()
        {
            m_InputBoxX.OnEndEdit.AddListener((x)=> SendEventCallback(OnEndEdit));
            m_InputBoxY.OnEndEdit.AddListener((y) => SendEventCallback(OnEndEdit));
            m_InputBoxX.OnValueChanged.AddListener((x) => SendEventCallback(OnValueChanged));
            m_InputBoxY.OnValueChanged.AddListener((y) => SendEventCallback(OnValueChanged));
        }

        public void Set(Vector2 vec)
        {
            m_InputBoxX.Set(vec.x.ToString("F4"));
            m_InputBoxY.Set(vec.y.ToString("F4"));
        }

        private void SendEventCallback(OnVectorChangedEvent callback)
        {
            if (callback == null) return;

            object x = m_InputBoxX.Get();
            object y = m_InputBoxY.Get();

            float? xFloat = x as float?;
            float? yFloat = y as float?;

            if (xFloat != null && yFloat != null)
            {
                callback.Invoke(new Vector2((float)xFloat, (float)yFloat));
                return;
            }
            callback.Invoke(m_InputBoxX.Get() + " " + m_InputBoxY.Get());
        }
    }
}