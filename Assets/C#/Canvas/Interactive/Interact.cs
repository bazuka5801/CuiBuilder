using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interact : MonoBehaviour
{
    public Transform triggerContainer;

    RectTransform parent { get { return transform.parent.GetComponent<RectTransform>(); } }
    private new RectTransform transform;
    Vector2 mInteractPoint;
    Vector2 mAnchor { get { return Vector2.one - mInteractPoint; } }
    Vector2 mAnchorPos;
    Vector2 mDelta;

    void Awake()
    {
        transform = (RectTransform)base.transform;
        InitTriggers();
    }

    void InitTriggers()
    {
        List<string> triggersEventArgs = new List<string>();
        for (float j = 0; j <= 1; j+= 0.5f)
            for (float k = 0; k <= 1f; k += 0.5f)
                triggersEventArgs.Add(string.Format("{0:F1} {1:F1}", k, j));
        for (int i = 0; i < triggerContainer.childCount; i++)
        {
            var resizePivot = triggersEventArgs[i];

            Transform triggerObject = triggerContainer.GetChild(i);
            EventTrigger trigger = triggerObject.gameObject.AddComponent<EventTrigger>();

            trigger.Add(EventTriggerType.PointerDown, ()=> OnPointerDown(resizePivot));
            trigger.Add(EventTriggerType.PointerEnter, ()=> CursorManager.SetCursorByAnchor(resizePivot));
            trigger.Add(EventTriggerType.PointerExit, () => CursorManager.SetCursor(CursorManager.CursorMode.Arrow));
            trigger.Add(EventTriggerType.Drag, () => { if (resizePivot == "0.5 0.5") OnMove(); else OnResize(); });
        }
    }

    private void OnPointerDown(string interactPivot)
    {
        mDelta = transform.anchorMin + transform.GetLocalSize() * 0.5f - transform.GetMouseLocal();
        mInteractPoint = Vector2Ex.Parse(interactPivot);
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
}
