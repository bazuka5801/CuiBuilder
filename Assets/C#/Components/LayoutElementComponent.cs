using System.Collections;
using System.ComponentModel;
using Oxide.Game.Rust.Cui;

public sealed class LayoutElementComponent : BaseComponent<CuiLayoutElementComponent>
{
    private LayoutGroupComponent m_Group;

    protected override void Awake()
    {
        base.Awake();
        m_Group = GetComponentInParent<LayoutGroupComponent>();
    }

    protected override void Load(CuiLayoutElementComponent component)
    {
        OnWeightChanged(component.Weight);
    }

    [CuiField( "weight" )]
    private void OnWeightChanged( object value )
    {
        float weight;
        if (!float.TryParse( value.ToString(), out weight ))
        {
            weight = 0f;
        }
        CuiComponent.Weight = weight;

        m_Group.UpdateLists();
    }
}

#region Nested type: CuiLayoutElementComponent

[System.Serializable]
public class CuiLayoutElementComponent : ICuiComponent
{
    public string Type
    {
        get { return "LayoutElement"; }
    }

    [DefaultValue( 1.0f )]
    public float Weight = 1.0f;

    public object Clone()
    {
        return this.DeepClone();
    }
}

#endregion