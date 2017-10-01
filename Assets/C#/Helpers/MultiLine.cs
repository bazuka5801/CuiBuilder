using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MultiLine : MonoBehaviour
{
    private RectTransform m_Transform;

    private float lastHeight;
    [SerializeField]
    private Text placeholder, text;
    [SerializeField]
    private InputField input;

    void Awake()
    {
        m_Transform = GetComponent<RectTransform>();
        lastHeight = m_Transform.sizeDelta.y*3;
    }

    public void Toggle()
    {
        var temp = m_Transform.sizeDelta.y;
        m_Transform.sizeDelta = m_Transform.sizeDelta.WithY(lastHeight);
        lastHeight = temp;
        if (placeholder.alignment == TextAnchor.MiddleLeft)
        {
            placeholder.alignment = text.alignment = TextAnchor.UpperLeft;
            input.lineType = InputField.LineType.MultiLineNewline;
        }
        else
        {
            placeholder.alignment = text.alignment = TextAnchor.MiddleLeft;
            input.lineType = InputField.LineType.SingleLine;
        }
    }
}
