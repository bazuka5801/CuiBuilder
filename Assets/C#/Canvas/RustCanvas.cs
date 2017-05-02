using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class RustCanvas : MonoBehaviour, IEventSystemHandler
{

    private static readonly Vector2 refResolution = new Vector2(1280f,720f);
    private CanvasScaler scaler;
    private static RectTransform canvas;

    void Awake()
    {
        scaler = GetComponent<CanvasScaler>();
        canvas = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (scaler.referenceResolution.Equals(refResolution))return;

        scaler.referenceResolution = refResolution;
    }


    public static Vector2 GetMousePos() {
        var pos = (Vector2)Input.mousePosition;
        return pos.Div(canvas.lossyScale);
    }

    public static Vector2 GetMouseAnchor()
    {
        return new Vector2(GetMousePos().x / canvas.rect.size.x, GetMousePos().y / canvas.rect.size.y); 
    }

    private void OnGUI()
    {
        GUILayout.Label("MOUSE ANCHOR: "+RustCanvas.GetMouseAnchor().ToString("F3"));
    }


}
