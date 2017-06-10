using Battlehub.UIControls;
using System.Collections.Generic;
using System.Linq;
using Inspector.UIControls;
using UnityEngine;
using UnityEngine.UI;

public class InspectorView : MonoBehaviour
{

    private RectTransformChangeListener m_rtcListener;
    private ScrollRect m_scrollRect;

    [SerializeField] Transform m_PanelsContainer;
    [SerializeField] InputBox m_NameField;
    [SerializeField] FloatBox m_FadeOutField;

    private IEnumerable<ComponentEditor> m_Components;
    private IEnumerable<ComponentEditor> m_ActiveComponents { get { return m_Components.Where( p => p.IsActive() ); } }
    public static List<CUIObject> SelectedItems;

    public static CUIObject SelectedItem = default( CUIObject );

    private void Awake()
    {
        m_scrollRect = GetComponent<ScrollRect>();
        m_rtcListener = GetComponentInChildren<RectTransformChangeListener>();
        m_rtcListener.RectTransformChanged += OnViewportRectTransformChanged;
        HierarchyView.AddSelectionListener( OnSelectionChanged );
        m_Components = GetComponentsInChildren<ComponentEditor>();
        AspectManager.OnChanged += index =>
        {
            OnSelectionChanged(null, new SelectionChangedArgs(new object[] { }, new object[] { }));
        };
    }

    private void Start()
    {
        HideAllPanels();
    }

    public static IEnumerable<T> GetSelectedComponents<T>() where T : BaseComponent
    {
        return CUIObject.Selection.Select( obj => obj.GetComponent<T>() ).Where( c => c );
    }

    private void OnSelectionChanged( object sender, SelectionChangedArgs e )
    {
        SelectedItems = e.NewItems.Select( o => ( (GameObject) o ).GetComponent<CUIObject>() ).Where( p => p != null ).ToList();
        SelectedItem = SelectedItems.Count > 0 ? SelectedItems.Last() : default( CUIObject );
        
        if (e.NewItems == null || e.NewItems.Length == 0)
        {
            HideAllPanels();
        }
        else
        {
            ShowAllPanels();
        }
        if (SelectedItem != null)
        {
            m_NameField.SetValue( SelectedItem.Name );
            m_FadeOutField.SetValue( SelectedItem.FadeOut );
        }
        foreach (var component in m_Components)
        {
            component.OnItemsSelected( SelectedItems );
        }
    }

    private void ShowAllPanels()
    {
        foreach (Transform panel in m_PanelsContainer)
            panel.gameObject.SetActive( true );
    }

    private void HideAllPanels()
    {
        foreach (Transform panel in m_PanelsContainer)
            panel.gameObject.SetActive( false );
    }

    public void OnNameChanged( object newName )
    {
        int i = 1;
        foreach (var item in CUIObject.Selection)
        {
            item.ChangeName( EditorHelper.Evaluate( newName.ToString(), i ) );
            i++;
        }
    }

    public void OnFadeOutChanged( object FadeOut )
    {
        foreach (var item in CUIObject.Selection)
        {
            item.FadeOut = (float) FadeOut;
        }
    }

    private void OnViewportRectTransformChanged()
    {
        Rect viewportRect = m_scrollRect.viewport.rect;
        foreach (Transform panel in m_scrollRect.viewport.GetChild( 0 ))
        {
            LayoutElement lElement = panel.GetComponent<LayoutElement>();
            if (lElement)
            {
                lElement.minWidth = viewportRect.width;
            }
        }
    }
}
