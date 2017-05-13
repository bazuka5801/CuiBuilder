using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Oxide.Game.Rust.Cui;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
public class CUIObject : MonoBehaviour, IPoolHandler {

    public static Dictionary<GameObject, CUIObject> Lookup = new Dictionary<GameObject, CUIObject>();
    public static List<CUIObject> Selection { get { return HierarchyView.GetSelectedItems().Select(o => Lookup[o]).ToList(); } }

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
        if (!Mathf.Approximately(FadeOut, 0f))
        {
            element.FadeOut = FadeOut;
        }
        foreach (var component in Components)
        {
            element.Components.Add(component);
        }
        return element;
    }

    public void Awake()
    {
        Lookup[gameObject] = this;
    }

    public void OnPoolEnter()
    {
        name = "CuiElement";
        FadeOut = 0f;
        Components.RemoveAll(p => p.GetType() != typeof(CuiRectTransformComponent));
        Lookup.Remove(gameObject);
    }

    public void OnPoolLeave()
    {
        Lookup[gameObject] = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log(CuiHelper.ToJson(new CuiElementContainer() { GetCuiElement() }));
        }
    }
}
