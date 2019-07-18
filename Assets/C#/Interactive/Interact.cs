using System.Collections.Generic;
using System.Linq;
using Oxide.Game.Rust.Cui;
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
    private List<float> Grid = new List<float>() {
        0.01f,
        0.02f,
        0.03f,
        0.04f,
        0.05f,
        0.06f,
        0.07f,
        0.08f,
        0.09f,
        0.1f
    };
    private int currentGridIndex;
    private bool GridEnabled;
    private RectTransform parent { get { return transform.parent.GetComponent<RectTransform>(); } }
    private new RectTransform transform;
    private Vector2 mInteractPoint;
    private Vector2 mAnchor { get { return Vector2.one - mInteractPoint; } }
    private Vector2 mAnchorPos;
    private Vector2 mDelta;
    private GameObject m_TriggerContainer;
    private static RectTransformEditor m_TransformEditor
    {
        get
        {
            return ( (RectTransformEditor) ComponentEditor<RectTransformComponent, CuiRectTransformComponent>
                .Instance() );
        }
    }


    private void Awake()
    {
        transform = (RectTransform) base.transform;
        if (isWindow)
        {
            BuildTriggers();
        }
    }

    private void Update()
    {
        if (isWindow || InspectorView.SelectedItem == null || InspectorView.SelectedItem.gameObject != gameObject) return;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            var leftShift = Input.GetKey(KeyCode.LeftShift);
            if (Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetKey(KeyCode.LeftArrow) && leftShift))
            {
                transform.SetPositionAnchorLocal(transform.anchorMin - transform.GetPixelShiftLocal().WithY(0));
                TransformEditorUpdate();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetKey(KeyCode.RightArrow) && leftShift))
            {
                transform.SetPositionAnchorLocal(transform.anchorMin + transform.GetPixelShiftLocal().WithY(0));
                TransformEditorUpdate();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetKey(KeyCode.UpArrow) && leftShift))
            {
                transform.SetPositionAnchorLocal(transform.anchorMin + transform.GetPixelShiftLocal().WithX(0));
                TransformEditorUpdate();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetKey(KeyCode.DownArrow) && leftShift))
            {
                transform.SetPositionAnchorLocal(transform.anchorMin - transform.GetPixelShiftLocal().WithX(0));
                TransformEditorUpdate();
            }
            if (Input.GetKeyDown(KeyCode.B) && Input.GetKey(KeyCode.LeftControl))
            {
                transform.SetPositionAnchorLocal(new Vector2(0.5f, 0.5f) - transform.GetSizeLocal() * 0.5f);
                TransformEditorUpdate();
            }
            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                GridEnabled = !GridEnabled;
            }
            if (Input.GetKeyDown(KeyCode.Equals))
            {
                currentGridIndex++;
                var gridCount = Grid.Count();
                if (currentGridIndex == Grid.Count())
                {
                    currentGridIndex = gridCount - 1;
                }
            }
            if (Input.GetKeyDown(KeyCode.Minus))
            {
                currentGridIndex--;
                if (currentGridIndex == -1)
                {
                    currentGridIndex = 0;
                }
            }
        }
    }

    #region Event Handlers

    private void OnPointerDown( Vector2 interactPivot )
    {
        if (!isWindow)
        {
            InspectorView.SelectedItem = CUIObject.Lookup[ transform.gameObject ];
        }
        mDelta = transform.anchorMin + transform.offsetMin / RustCanvas.refResolution+ transform.GetSizeLocal() * 0.5f - transform.GetMouseLocal();
        mInteractPoint = interactPivot;
        mAnchorPos = transform.GetPivotLocalPosition( mAnchor );
    }

    private void OnMove() {
        var pos = transform.GetMouseLocal() + mDelta - transform.GetSizeLocal() * 0.5f;
        if (GridEnabled) {
            pos.x = pos.x - (pos.x % Grid[currentGridIndex]);
            pos.y = pos.y - (pos.y % Grid[currentGridIndex]);
        }

        transform.SetPositionAnchorLocal( pos, isWindow );

        TransformEditorUpdate();
    }

    private void OnResize()
    {
        var size = ( mAnchorPos - transform.GetMouseLocal() ).Abs();
        var currentSize = transform.GetSizeLocal();

        for (int j = 0; j < 2; j++)
            if (Mathf.Approximately( mInteractPoint[ j ], 0.5f ))
                size[ j ] = currentSize[ j ];

        var anchorSize = transform.anchorMin + size;
        if (GridEnabled) {
            anchorSize.x = anchorSize.x - (anchorSize.x % Grid[currentGridIndex]);
            anchorSize.y = anchorSize.y - (anchorSize.y % Grid[currentGridIndex]);
        }

        transform.SetRect( transform.anchorMin, anchorSize );
        var posDelta = mAnchorPos - transform.GetPivotLocalPosition( mAnchor );

        var pos = transform.anchorMin + posDelta;
        if (GridEnabled) {
            pos.x = pos.x - (pos.x % Grid[currentGridIndex]);
            pos.y = pos.y - (pos.y % Grid[currentGridIndex]);
        }

        transform.SetPositionAnchorLocal( pos );
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

        //m_TriggerContainer = transform.CreateChild( "_triggers" );
        m_TriggerContainer = PoolManager.Get(PrefabType.Trigger);
        RectTransform tgrTransform = (RectTransform) m_TriggerContainer.transform;
        tgrTransform.SetParent( transform, false );
        tgrTransform.SetRect( Vector2.zero, Vector2.one );
        tgrTransform.offsetMin = tgrTransform.offsetMax = Vector2.zero;

        var eTriggers = m_TriggerContainer.GetComponentsInChildren<EventTrigger>();
        for (var index = 0; index < triggersEventArgs.Count; index++)
        {
            var anchor = triggersEventArgs[index];
            AddDragHandlers( eTriggers[index], anchor);
        }
    }

    private void DestroyTriggers()
    {
        if (m_TriggerContainer == null) return;
        foreach (var eventTrigger in m_TriggerContainer.GetComponentsInChildren<EventTrigger>())
        {
            eventTrigger.triggers.Clear();
        }
        PoolManager.Release(PrefabType.Trigger, m_TriggerContainer);
        m_TriggerContainer = null;
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

    //private EventTrigger CreateTrigger( RectTransform tgrContainer, Vector2 anchor )
    //{
    //    var triggerObj = tgrContainer.CreateChild( anchor.ToString( "F1" ) );
    //    RectTransform tgrTransform = (RectTransform) triggerObj.transform;

    //    tgrTransform.SetRect( anchor, anchor );
    //    tgrTransform.sizeDelta = triggerSize;

    //    var img = triggerObj.AddComponent<Image>();
    //    img.color = triggerColor;
    //    img.sprite = triggerSprite;
    //    return triggerObj.AddComponent<EventTrigger>();
    //}

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
    }

    public void OnUnselected()
    {
        DestroyTriggers();
    }

    #endregion
}
