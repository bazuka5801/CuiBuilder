using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Oxide.Game.Rust.Cui;
using UnityEngine;

public abstract class BaseComponent : MonoBehaviour
{
    protected CUIObject cuiObject;
    private static Dictionary<Type, Type> m_ComponentTypes;

    protected virtual void Awake()
    {
        cuiObject = GetComponent<CUIObject>();
    }

    protected virtual bool CanBeAdd()
    {
        return true;
    }


    public static Type GetComponentType( Type cuiType )
    {
        if (m_ComponentTypes == null)
        {
            Dictionary<Type, Type> dictionary = new Dictionary<Type, Type>();
            foreach (var t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t.IsSubclassOf(typeof( BaseComponent)) && t.IsSealed && t.BaseType.GetGenericArguments().Length > 0)
                {
                    KeyValuePair<Type, Type> pair = new KeyValuePair<Type, Type>(t.BaseType.GetGenericArguments()[0], t);
                    dictionary.Add(pair.Key, pair.Value);
                }
            }
            m_ComponentTypes = dictionary;
        }
        Type componentType;
        if (!m_ComponentTypes.TryGetValue( cuiType, out componentType ))
            throw new Exception( cuiType + " doesn't presented in the m_ComponentsTypes" );
        return componentType;
    }

    public abstract void LoadInternal(ICuiComponent component);
}

public abstract class BaseComponent<T> : BaseComponent
    where T : ICuiComponent
{
    public T CuiComponent
    {
        get { return cuiObject.GetCuiComponent<T>(); }
    }

    protected abstract void Load( T component );

    public override void LoadInternal(ICuiComponent component)
    {
        if (component.GetType() != typeof(T)) return;
        Load((T)component);
    }
}

public interface IGraphicComponent { }
public interface ISelectableComponent { }