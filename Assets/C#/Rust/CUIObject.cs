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
            Name = Name
        };
        if (!Mathf.Approximately( FadeOut, 0f ))
        {
            element.FadeOut = FadeOut;
        }
        foreach (var component in Components)
        {
            element.Components.Add( component );
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
        Components.RemoveAll( p => p.GetType() != typeof( CuiRectTransformComponent ) );
        Lookup.Remove( gameObject );
    }

    public void OnComponentStateChanged<CT, CCT>( ComponentEditor<CT, CCT> component, bool state )
        where CCT : ICuiComponent
        where CT : BaseComponent<CCT>
    {
        if (state)
        {
            if (Components.Any( c => c is CCT )) return;
            Components.Add( (CCT) Activator.CreateInstance( typeof( CCT ) ) );
            gameObject.AddComponent<CT>();
        }
        else
        {
            if (Components.All( c => !( c is CCT ) )) return;
            Components.Remove( GetCuiComponent<CCT>() );
            DestroyImmediate( GetComponent<CT>() );
        }
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
            Debug.Log( CuiHelper.ToJson( new CuiElementContainer() { GetCuiElement() } ) );
        }
    }
}
