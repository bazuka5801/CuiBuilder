using Oxide.Game.Rust.Cui;
using UnityEngine;

public abstract class BaseComponent : MonoBehaviour
{
    protected CUIObject cuiObject;

    protected virtual void Awake()
    {
        cuiObject = GetComponent<CUIObject>();
    }

    protected virtual bool CanBeAdd()
    {
        return true;
    }
}

public class BaseComponent<T> : BaseComponent
    where T : ICuiComponent
{
    public T CuiComponent
    {
        get { return cuiObject.GetCuiComponent<T>(); }
    }
}

public interface IGraphicComponent { }
public interface ISelectableComponent { }