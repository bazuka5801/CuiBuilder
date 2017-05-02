using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BindManager : MonoBehaviour {
    [Serializable]
    public class OnBindExecuteEvent : UnityEvent { }
    [Serializable]
    public class Bind
    {
        public List<KeyCode> keys;
        public OnBindExecuteEvent action;
    }

    [SerializeField] private List<Bind> m_Binds = new List<Bind>();

    void Update()
    {
        foreach (var obj in m_Binds)
        {
            if (obj.keys.All(p => p == obj.keys.Last() ? Input.GetKeyDown(p) : Input.GetKey(p)))
            {
                if (obj.action != null)
                {
                    obj.action.Invoke();
                }
            }
        }
    }
}
