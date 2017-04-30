using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{    
    [SerializeField] private bool isWindow;
    [SerializeField] private GameObject windowDragPanel;
    [SerializeField] private Vector2 triggerSize;
    [SerializeField] private Color triggerColor;
    [SerializeField] private Sprite triggerSprite;
    

    private static readonly Vector2 Center = new Vector2(0.5f, 0.5f);

    private RectTransform parent { get { return transform.parent.GetComponent<RectTransform>(); } }
    private new RectTransform transform;
    private Vector2 mInteractPoint;
    private Vector2 mAnchor { get { return Vector2.one - mInteractPoint; } }
    private Vector2 mAnchorPos;
    private Vector2 mDelta;


    private void Awake()
    {
        transform = (RectTransform)base.transform;
        InitTriggers();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.SetPosition(transform.anchorMin - GetPixelShift().WithY(0));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.SetPosition(transform.anchorMin + GetPixelShift().WithY(0));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.SetPosition(transform.anchorMin + GetPixelShift().WithX(0));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.SetPosition(transform.anchorMin - GetPixelShift().WithX(0));
        }
    }

    private Vector2 GetPixelShift()
    {
        return (parent.GetWorldSize().Div(new Vector2(Screen.width, Screen.height)));
    }

    #region Event Handlers

    private void OnPointerDown(Vector2 interactPivot)
    {
        mDelta = transform.anchorMin + transform.GetLocalSize() * 0.5f - transform.GetMouseLocal();
        mInteractPoint = interactPivot;
        mAnchorPos = transform.GetPivotLocalPosition(mAnchor);
    }

    private void OnMove()
    {
        transform.SetPosition(transform.GetMouseLocal() + mDelta - transform.GetLocalSize() * 0.5f);
    }
    
    private void OnResize()
    {
        var size = (mAnchorPos - transform.GetMouseLocal()).Abs();
        var currentSize = transform.GetLocalSize();

        for (int j = 0; j < 2; j++)
            if (Mathf.Approximately(mInteractPoint[j], 0.5f))
                size[j] = currentSize[j];

        transform.SetRect(transform.anchorMin, transform.anchorMin + size);
        var posDelta = mAnchorPos - transform.GetPivotLocalPosition(mAnchor);
        transform.SetPosition(transform.anchorMin + posDelta);
    }

    #endregion

    #region Triggers

    private void InitTriggers()
    {
        if (isWindow)
        {
            // Move
            AddDragHandlers(windowDragPanel.AddComponent<EventTrigger>(), Center);
            return;
        }

        List<Vector2> triggersEventArgs = new List<Vector2>();

        // Move + Resize
        for (float j = 0; j <= 1; j += 0.5f)
            for (float k = 0; k <= 1f; k += 0.5f)
                triggersEventArgs.Add(new Vector2(k, j));

        var triggerContainer = transform.CreateChild("_triggers");
        RectTransform tgrTransform = (RectTransform)triggerContainer.transform;
        tgrTransform.SetRect(Vector2.zero, Vector2.one);

        foreach (var anchor in triggersEventArgs)
        {
            AddDragHandlers(CreateTrigger(tgrTransform, anchor), anchor);
        }
    }

    private void AddDragHandlers(EventTrigger trigger, Vector2 anchor)
    {
        trigger.Add(EventTriggerType.PointerDown, () => OnPointerDown(anchor));
        trigger.Add(EventTriggerType.Drag, () => { if (anchor == Center) OnMove(); else OnResize(); });
        if (!isWindow)
        {
            trigger.Add(EventTriggerType.PointerEnter, () => CursorManager.SetCursorByAnchor(anchor));
            trigger.Add(EventTriggerType.PointerExit, () => CursorManager.SetCursor(CursorManager.CursorMode.Arrow));
        }
    }

    private EventTrigger CreateTrigger(RectTransform tgrContainer, Vector2 anchor)
    {
        var triggerObj = tgrContainer.CreateChild(anchor.ToString("F1"));
        RectTransform tgrTransform = (RectTransform)triggerObj.transform;

        tgrTransform.SetRect(anchor, anchor);
        tgrTransform.sizeDelta = triggerSize;

        var img = triggerObj.AddComponent<Image>();
        img.color = triggerColor;
        img.sprite = triggerSprite;
        return triggerObj.AddComponent<EventTrigger>();
    }

    #endregion
}
