using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hierarchy : MonoBehaviour, IPoolHandler,  IPointerClickHandler, ISelectHandler{

    public static Dictionary<GameObject, Hierarchy> Lookup = new Dictionary<GameObject, Hierarchy>();
    public List<Hierarchy> Children = new List<Hierarchy>();
    [SerializeField]private Hierarchy parent;

    public static List<Hierarchy> Selection { get { return HierarchyView.GetSelectedItems().Select(o=>Lookup[o]).ToList(); } }

    private void Awake()
    {
        Lookup[gameObject] = this;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        foreach (Transform childT in transform)
        {
            if (childT.tag == "CUI")
            {
                var child = Lookup[childT.gameObject];
                child.parent = this;
                Children.Add(child);
            }
        }
    }


    public void OnPoolEnter()
    {
        Lookup.Remove(gameObject);
        Children.Clear();
        parent = null;
    }

    public void OnPoolLeave()
    {
        Lookup[gameObject] = this;
        Init();
    }

    public void SetParent(Hierarchy newParent)
    {
        var rTransform = (RectTransform)transform;
        if (parent != null)
        {
            parent.Children.Remove(this);

            var position = rTransform.GetParent().GetWorldPoint(rTransform.anchorMin);
            var size = rTransform.GetWorldSize();
            Debug.Log(position + " " + size);
            transform.SetParent(newParent.transform, false);
            rTransform.SetPosition(rTransform.GetParent().GetLocalPoint(position));
            rTransform.SetWorldSize(size);
        }
        else
        {
            transform.SetParent(newParent.transform, false);
        }
        parent = newParent;
        parent.Children.Add(this);
    }

    public void SetParent(GameObject newParent)
    {
        SetParent(Lookup[newParent]);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (HierarchyView.Select(gameObject)) return;
    }

    public void OnSelected()
    {

    }

    public void OnUnselected()
    {

    }
}
