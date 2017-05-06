using Battlehub.UIControls;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CUIInspector : MonoBehaviour {

    private RectTransformChangeListener m_rtcListener;
    private ScrollRect m_scrollRect;

    [SerializeField] List<GameObject> panels;

    private void Awake()
    {
        m_scrollRect = GetComponent<ScrollRect>();
        m_rtcListener = GetComponentInChildren<RectTransformChangeListener>();
        m_rtcListener.RectTransformChanged += OnViewportRectTransformChanged;
        HierarchyView.AddSelectionListener(OnSelectionChanged);
    }

    private void OnSelectionChanged(object sender, SelectionChangedArgs e)
    {
        if (e.NewItems == null || e.NewItems.Length == 0)
        {
            HideAllPanels();
        }
        else
        {
            ShowAllPanels();
        }
    }

    private void ShowAllPanels()
    {
        foreach (var panel in panels)
            panel.SetActive(true);
    }

    private void HideAllPanels()
    {
        foreach (var panel in panels)
            panel.SetActive(false);
    }

    public void OnNameChanged(string name)
    {
        foreach(var item in HierarchyView.GetSelectedItems())
        {
            HierarchyView.ChangeName(item, name);
        }
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
