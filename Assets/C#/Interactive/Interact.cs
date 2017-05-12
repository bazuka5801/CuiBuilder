using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Interact : MonoBehaviour, IPoolHandler, ISelectHandler
{
    [SerializeField] private bool isWindow;
    [SerializeField] private GameObject dragHandle;
    [SerializeField] private Vector2 triggerSize;
    [SerializeField] private Color triggerColor;
    [SerializeField] private Sprite triggerSprite;


    private static readonly Vector2 Center = new Vector2( 0.5f, 0.5f );

    private RectTransform parent { get { return transform.parent.GetComponent<RectTransform>(); } }
    private new RectTransform transform;
    private Vector2 mInteractPoint;
    private Vector2 mAnchor { get { return Vector2.one - mInteractPoint; } }
    private Vector2 mAnchorPos;
    private Vector2 mDelta;
    private GameObject m_TriggerContainer;
    private static RectTransformEditor m_TransformEditor;

    public static RectTransform Selected;

    private void Awake()
    {
        transform = (RectTransform) base.transform;
        if (isWindow)
        {
            BuildTriggers();
        }
        if (m_TransformEditor == null)
        {
            m_TransformEditor = ( (RectTransformEditor) ComponentEditor<RectTransformComponent>.Instance() );
        }
    }

    private void Update()
    {
        if (isWindow || !HierarchyView.GetSelectedItems().Contains(gameObject)) return;
        var leftControl = Input.GetKey(KeyCode.LeftControl);
        if (Input.GetKeyDown( KeyCode.LeftArrow ) || ( Input.GetKey( KeyCode.LeftArrow ) && leftControl ))
        {
            transform.SetPositionAnchor( transform.anchorMin - transform.GetPixelShiftLocal().WithY( 0 ) ); TransformEditorUpdate();
        }
        if (Input.GetKeyDown( KeyCode.RightArrow ) || (Input.GetKey(KeyCode.RightArrow) && leftControl ) )
        {
            transform.SetPositionAnchor( transform.anchorMin + transform.GetPixelShiftLocal().WithY( 0 ) ); TransformEditorUpdate();
        }
        if (Input.GetKeyDown( KeyCode.UpArrow ) || ( Input.GetKey( KeyCode.UpArrow ) && leftControl ))
        {
            transform.SetPositionAnchor( transform.anchorMin + transform.GetPixelShiftLocal().WithX( 0 ) ); TransformEditorUpdate();
        }
        if (Input.GetKeyDown( KeyCode.DownArrow ) || ( Input.GetKey( KeyCode.DownArrow ) && leftControl ))
        {
            transform.SetPositionAnchor( transform.anchorMin - transform.GetPixelShiftLocal().WithX( 0 ) ); TransformEditorUpdate();
        }
    }

    #region Event Handlers

    private void OnPointerDown( Vector2 interactPivot )
    {
        mDelta = transform.anchorMin + transform.GetSizeLocal() * 0.5f - transform.GetMouseLocal();
        mInteractPoint = interactPivot;
        mAnchorPos = transform.GetPivotLocalPosition( mAnchor );
    }

    private void OnMove()
    {
        transform.SetPositionAnchor( transform.GetMouseLocal() + mDelta - transform.GetSizeLocal() * 0.5f, isWindow );

        TransformEditorUpdate();
    }

    private void OnResize()
    {
        var size = ( mAnchorPos - transform.GetMouseLocal() ).Abs();
        var currentSize = transform.GetSizeLocal();

        for (int j = 0; j < 2; j++)
            if (Mathf.Approximately( mInteractPoint[ j ], 0.5f ))
                size[ j ] = currentSize[ j ];

        transform.SetRect( transform.anchorMin, transform.anchorMin + size );
        var posDelta = mAnchorPos - transform.GetPivotLocalPosition( mAnchor );
        transform.SetPositionAnchor( transform.anchorMin + posDelta );
        TransformEditorUpdate();
    }

    void TransformEditorUpdate()
    {
        if (isWindow) return;

        m_TransformEditor.SendAnchorMinUpdate( transform.anchorMin );
        m_TransformEditor.SendAnchorMaxUpdate( transform.anchorMax );
    }

    #endregion

    #region Triggers

    private void BuildTriggers()
    {
        if (isWindow)
        {
            // Move
            AddDragHandlers( dragHandle.AddComponent<EventTrigger>(), Center );
            return;
        }

        if (m_TriggerContainer != null) return;
        List<Vector2> triggersEventArgs = new List<Vector2>();

        // Move + Resize
        for (float j = 0; j <= 1; j += 0.5f)
            for (float k = 0; k <= 1f; k += 0.5f)
                triggersEventArgs.Add( new Vector2( k, j ) );

        m_TriggerContainer = transform.CreateChild( "_triggers" );
        RectTransform tgrTransform = (RectTransform) m_TriggerContainer.transform;
        tgrTransform.SetRect( Vector2.zero, Vector2.one );
        tgrTransform.offsetMin = tgrTransform.offsetMax = Vector2.zero;

        foreach (var anchor in triggersEventArgs)
        {
            AddDragHandlers( CreateTrigger( tgrTransform, anchor ), anchor );
        }
    }

    private void DestroyTriggers()
    {
        if (m_TriggerContainer == null) return;
        Destroy( m_TriggerContainer );
    }

    private void AddDragHandlers( EventTrigger trigger, Vector2 anchor )
    {
        trigger.Add( EventTriggerType.PointerDown, () => OnPointerDown( anchor ) );
        trigger.Add( EventTriggerType.Drag, () => { if (anchor == Center) OnMove(); else OnResize(); } );
        if (!isWindow)
        {
            trigger.Add( EventTriggerType.PointerEnter, () => CursorManager.SetCursorByAnchor( anchor ) );
            trigger.Add( EventTriggerType.PointerExit, () => CursorManager.SetCursor( CursorManager.CursorMode.Arrow ) );
        }
    }

    private EventTrigger CreateTrigger( RectTransform tgrContainer, Vector2 anchor )
    {
        var triggerObj = tgrContainer.CreateChild( anchor.ToString( "F1" ) );
        RectTransform tgrTransform = (RectTransform) triggerObj.transform;

        tgrTransform.SetRect( anchor, anchor );
        tgrTransform.sizeDelta = triggerSize;

        var img = triggerObj.AddComponent<Image>();
        img.color = triggerColor;
        img.sprite = triggerSprite;
        return triggerObj.AddComponent<EventTrigger>();
    }

    public void OnPoolEnter()
    {
        DestroyTriggers();
    }

    public void OnPoolLeave()
    {

    }

    public void OnSelected()
    {
        BuildTriggers();
        Selected = transform;
    }

    public void OnUnselected()
    {
        DestroyTriggers();
    }

    #endregion
}
