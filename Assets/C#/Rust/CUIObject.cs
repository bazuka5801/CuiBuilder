using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Oxide.Game.Rust.Cui;
using UnityEngine;
using UnityEngine.UI;

[JsonObject( MemberSerialization.OptIn )]
public class CUIObject : MonoBehaviour, IPoolHandler
{

    public static Dictionary<GameObject, CUIObject> Lookup = new Dictionary<GameObject, CUIObject>();
    public static List<CUIObject> Selection { get { return HierarchyView.GetSelectedItems().Select( o => Lookup[ o ] ).ToList(); } }

    public string Name { get { return name; } }
    List<ICuiComponent> Components = new List<ICuiComponent>() { new CuiRectTransformComponent() };
    public float FadeOut;

    public T GetCuiComponent<T>() where T : ICuiComponent
    {
        return Components.OfType<T>().FirstOrDefault();
    }

    public CuiElement GetCuiElement()
    {
        var element = new CuiElement()
        {
            Name = Name,
            Parent = transform.parent.name
        };
        if (!Mathf.Approximately( FadeOut, 0f ))
        {
            element.FadeOut = FadeOut;
        }
        foreach (var component in Components)
        {
            element.Components.Add( (ICuiComponent)component.Clone() );
        }
        return element;
    }


    public void Awake()
    {
        Lookup[ gameObject ] = this;
    }

    public void OnPoolEnter()
    {
        name = "CuiElement";
        FadeOut = 0f;
        foreach (var component in GetComponents(typeof(BaseComponent)))
        {
            if (component.GetType() != typeof(RectTransformComponent))
            {
                DestroyImmediate( component);
            }
        }
        Components.RemoveAll( p => p.GetType() != typeof( CuiRectTransformComponent ) );
        Lookup.Remove( gameObject );
    }

    public void OnComponentStateChanged( Type componentType, bool state )
    {
        if (state)
        {
            LoadCuiComponent( (ICuiComponent) Activator.CreateInstance( componentType ) );
        }
        else
        {
            DestroyCuiComponent( componentType );
        }
    }

    public void Load(CuiElement element)
    {
        ChangeName(element.Name);
        FadeOut = element.FadeOut;
        GetComponent<Hierarchy>().SetParent(element.Parent);
        foreach (var component in element.Components)
        {
            LoadCuiComponent(component);
        }
    }

    public void ChangeName(string newName)
    {
        HierarchyView.ChangeName( gameObject, newName );
    }

    public void LoadCuiComponent( ICuiComponent cuiComponent )
    {
        var cuiComponentType = cuiComponent.GetType();
        var componentType = BaseComponent.GetComponentType( cuiComponentType );
        if (Components.Any(c => c.GetType() == cuiComponentType))
        {
            ((BaseComponent)GetComponent( componentType )).LoadInternal( cuiComponent );
            return;
        }
        Components.Add( cuiComponent );
        var component = (BaseComponent)gameObject.AddComponent( componentType );
        component.LoadInternal(cuiComponent);
    }

    public void DestroyCuiComponent( Type cuiComponentType )
    {
        var cuiComponent = Components.FirstOrDefault(p=>p.GetType() == cuiComponentType );
        if (cuiComponent == null) return;

        var componentType = BaseComponent.GetComponentType( cuiComponentType );
        Components.Remove( cuiComponent );
        DestroyImmediate( GetComponent( componentType ) );
    }

    public bool CanBeAdd<CT, CCT>()
        where CT : BaseComponent
        where CCT : ICuiComponent
    {
        if (typeof( IGraphicComponent ).IsAssignableFrom( typeof( CT ) ))
        {
            if (GetComponent<Graphic>()) return false;
        }
        if (typeof( ISelectableComponent ).IsAssignableFrom( typeof( CT ) ))
        {
            if (GetComponent<Selectable>()) return false;
        }
        return true;
    }

    public void OnPoolLeave()
    {
        Lookup[ gameObject ] = this;
    }

    void Update()
    {
        if (Input.GetKeyDown( KeyCode.C ))
        {
            //Debug.Log( string.Join( ", ", HierarchyView.GetCurrent().Select( p => p.Name ).ToArray() ) );
            //Debug.Log( CuiHelper.ToJson( new CuiElementContainer() { GetCuiElement() } ) );
        }
    }
}
