using System.Collections;
using System.Collections.Generic;
using Oxide.Game.Rust.Cui;
using UnityEngine;

public class RectTransformComponent : BaseComponent<CuiRectTransformComponent>
{
    private RectTransform m_Transform;

    protected override void Awake()
    {
        base.Awake();
        m_Transform = (RectTransform) base.transform;
    }

    [InspectorField("anchormin")]
    private void OnAnchorMinChanged(object value)
    {
        var type = value.GetType();
        if (type == typeof(Vector2))
        {
            var vec = (Vector2) value;
            m_Transform.SetPosition(vec);
            CuiComponent.AnchorMin = string.Format("{0} {1}", vec.x, vec.y);
        }
        else
        {
            m_Transform.SetPosition(new Vector2(0.1f, 0.1f));
            CuiComponent.AnchorMin = value.ToString();
        }
    }

    [InspectorField("anchormax")]
    private void OnAnchorMaxChanged(object value)
    {
        var type = value.GetType();
        if (type == typeof(Vector2))
        {
            var vec = (Vector2)value;
            m_Transform.anchorMax = (Vector2) value;
            CuiComponent.AnchorMax = string.Format("{0} {1}", vec.x, vec.y);
        }
        else
        {
            m_Transform.SetPixelSize(new Vector2(100f, 100f));
            CuiComponent.AnchorMax = value.ToString();
        }
    }

    [InspectorField("position")]
    private void OnPositionChanged(object value)
    {
        var type = value.GetType();
        if (type == typeof(Vector2))
        {
            m_Transform.SetPixelLocalPosition((Vector2)value);

            CuiComponent.AnchorMin = string.Format("{0} {1}", m_Transform.anchorMin.x, m_Transform.anchorMin.y);
            CuiComponent.AnchorMax = string.Format("{0} {1}",m_Transform.anchorMax.x, m_Transform.anchorMax.y);
        }
    }
    [InspectorField("size")]
    private void OnSizeChanged(object value)
    {
        var type = value.GetType();
        if (type == typeof(Vector2))
        {
            m_Transform.SetPixelSize((Vector2)value);
            
            CuiComponent.AnchorMax = string.Format("{0} {1}", m_Transform.anchorMax.x, m_Transform.anchorMax.y);
        }
    }
}
