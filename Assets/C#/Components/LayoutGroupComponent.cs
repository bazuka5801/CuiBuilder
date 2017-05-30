using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Oxide.Game.Rust.Cui;
using UnityEngine;

public sealed class LayoutGroupComponent : BaseComponent<CuiLayoutGroupComponent>
{
    private Hierarchy m_Hierarchy;
    private List<RectTransformComponent> m_Children;
    private List<float> m_Weights;
    private bool locker = false;
    
    private LayoutGroupMode Mode { get { return CuiComponent.Mode; } }
    // TODO: OnParentChanged (Удалять компонент)
    // TODO: OnRectTransformChanged (Обновлять размеры элемента)

    protected override void Awake()
    {
        base.Awake();
        m_Hierarchy = GetComponent<Hierarchy>();
        m_Hierarchy.OnChildAdded += OnChildAdded;
        m_Hierarchy.OnChildRemoved += OnChildRemoved;
        UpdateLists();
        GetComponent<RectTransformComponent>().OnChanged += UpdateChildren;
    }

    private void OnDestroy()
    {
        GetComponent<RectTransformComponent>().OnChanged -= UpdateChildren;
        m_Hierarchy.OnChildAdded -= OnChildAdded;
        m_Hierarchy.OnChildRemoved -= OnChildRemoved;
        foreach (var child in m_Children)
        {
            child.GetComponent<RectTransformComponent>().OnChanged -= UpdateLists;
            child.GetComponent<CUIObject>().OnComponentStateChanged( typeof( CuiLayoutElementComponent ), false );
        }
    }


    private void OnChildAdded( Hierarchy item )
    {
        item.GetComponent<CUIObject>().OnComponentStateChanged(typeof(CuiLayoutElementComponent), true);
        item.GetComponent<RectTransformComponent>().OnChanged += UpdateLists;
        OnChildrenChanged();
    }

    private void OnChildRemoved( Hierarchy item )
    {
        item.GetComponent<RectTransformComponent>().OnChanged -= UpdateLists;
        item.GetComponent<CUIObject>().OnComponentStateChanged(typeof( CuiLayoutElementComponent ), false);
        OnChildrenChanged();
    }

    public void UpdateLists()
    {
        m_Children = m_Hierarchy.GetChildren().Select( p => p.GetComponent<RectTransformComponent>() ).Where(p=>p).ToList();

        m_Weights = m_Children.Select(p =>
        {
            var comp = p.GetComponent<LayoutElementComponent>();
            if (comp) return comp;
            CUIObject.Lookup[ p.gameObject ].OnComponentStateChanged( typeof( CuiLayoutElementComponent ), true );
            return p.GetComponent<LayoutElementComponent>();
        } ).Select( p => p == null ? 1 : p.CuiComponent.Weight ).ToList();
        if (m_Children.Count != m_Weights.Count) return;
        UpdateChildren();
    }

    private void OnChildrenChanged( )
    {
        UpdateLists();
    }

    /// <summary>
    /// UpdateTransforms with element weight
    /// </summary>
    private void UpdateChildren()
    {
        if (locker) return;
        locker = true;

        var sizes = GetSizes();
        Vector2 spacingOffset = ( (RectTransform) transform ).GetPixelShiftLocalForChild() * CuiComponent.Spacing;

        float anchorMin = spacingOffset[(int)Mode];
        for (var i = 0; i < m_Children.Count; i++)
        {
            var weight = m_Weights[i];
            var child = m_Children[i];
            
            switch (CuiComponent.Mode)
            {
                case LayoutGroupMode.Horizontal:
                {
                    var anchorMax = new Vector2( anchorMin+sizes[i], 1- spacingOffset.y );
                        
                        child.SetRect( new Vector2( anchorMin, spacingOffset.y ), anchorMax);
                        
                        break;
                }
                case LayoutGroupMode.Vertical:
                {
                        var anchorMax = new Vector2( 1 - spacingOffset.x, anchorMin + sizes[ i ] );
                        child.SetRect( new Vector2( spacingOffset.x, anchorMin ), anchorMax);
                        break;
                }
            }
            anchorMin += sizes[i] + spacingOffset[(int)Mode];
        }

        locker = false;
    }

    private float[] GetSizes()
    {
        var spacing = CuiComponent.Spacing;
        Vector2 spacingPixelOffset = ( (RectTransform) transform ).GetPixelShiftLocalForChild() * spacing;

        var totalWeights = m_Weights.Sum();
        float maxSize = ( 1 - spacingPixelOffset[(int)Mode] * ( m_Children.Count + 1 ) ) ;
        return m_Weights.Select(p => p / totalWeights * maxSize ).ToArray();
    }

    protected override void Load( CuiLayoutGroupComponent component )
    {
        OnModeChanged( component.Mode );
    }

    [CuiField( "mode" )]
    private void OnModeChanged( object value )
    {
        var mode = (LayoutGroupMode) value;
        CuiComponent.Mode = mode;

        UpdateChildren();
    }
    [CuiField( "spacing" )]
    private void OnSpacingChanged( object value )
    {
        var spacing = (float) value;
        CuiComponent.Spacing = spacing;
        UpdateChildren();
    }
}

#region Nested type: CuiLayoutGroupComponent

public class CuiLayoutGroupComponent : ICuiComponent
{
    public string Type
    {
        get { return "LayoutGroup"; }
    }

    [DefaultValue( LayoutGroupMode.Horizontal )]
    public LayoutGroupMode Mode = 0;

    [DefaultValue( 0.0f )]
    public float Spacing = 0.0f;
}

#endregion

#region Nested type: LayoutGroupMode

public enum LayoutGroupMode
{
    Horizontal = 0,
    Vertical = 1
}

#endregion
