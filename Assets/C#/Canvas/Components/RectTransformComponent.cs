using System.Collections;
using System.Collections.Generic;
using Oxide.Game.Rust.Cui;
using UnityEngine;

<<<<<<< HEAD
public class RectTransformComponent : BaseComponent<CuiRectTransformComponent>
{
    private RectTransform m_Transform;

    protected override void Awake()
    {
        base.Awake();
        m_Transform = (RectTransform) base.transform;
    }

    [InspectorField("position")]
    private void OnPositionChanged(object value)
    {
        var type = value.GetType();
        if (type == typeof(Vector2))
        {
            m_Transform.SetPosition((Vector2) value);
        }
        else
        {
            m_Transform.SetPosition(new Vector2(0.1f,0.1f));
        }
        CuiComponent.AnchorMin = value.ToString();
    }

    [InspectorField("size")]
    private void OnSizeChanged(object value)
    {
        var type = value.GetType();
        if (type == typeof(Vector2))
        {
            m_Transform.SetLocalSize((Vector2)value);
        }
        else
        {
            m_Transform.SetPixelSize(new Vector2(100f, 100f));
        }
        CuiComponent.AnchorMin = value.ToString();
    }
=======
public class RectTransformComponent : BaseComponent<CuiRectTransformComponent> {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
>>>>>>> ad2b1da... Component Systemr 1/2
}
