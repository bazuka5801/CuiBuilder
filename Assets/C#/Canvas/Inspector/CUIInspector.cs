using Battlehub.UIControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CUIInspector : MonoBehaviour {

    private RectTransformChangeListener m_rtcListener;
    private ScrollRect m_scrollRect;

    private void Awake()
    {
        m_scrollRect = GetComponent<ScrollRect>();
        m_rtcListener = GetComponentInChildren<RectTransformChangeListener>();
        m_rtcListener.RectTransformChanged += OnViewportRectTransformChanged;
    }

    private void OnViewportRectTransformChanged()
    {
        Rect viewportRect = m_scrollRect.viewport.rect;
        foreach(Transform panel in m_scrollRect.viewport.GetChild(0))
        {
            LayoutElement lElement = panel.GetComponent<LayoutElement>();
            if (lElement)
            {
                lElement.minWidth = viewportRect.width;
            }
        }
    }
}
