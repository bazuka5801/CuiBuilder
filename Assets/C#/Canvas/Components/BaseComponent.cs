using System.Collections;
using System.Collections.Generic;
using Oxide.Game.Rust.Cui;
using UnityEngine;

public class BaseComponent : MonoBehaviour
{
    public ICuiComponent CuiComponent;

    protected CUIObject cuiObject;
    
    protected virtual void Awake()
    {
        cuiObject = GetComponent<CUIObject>();
    }
}

public class BaseComponent<T> : BaseComponent
    where T : ICuiComponent
{
    public new T CuiComponent
    {
        get { return (T)base.CuiComponent; }
        set { base.CuiComponent = value; }
    }
}
