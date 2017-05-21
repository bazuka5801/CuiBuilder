using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hierarchy : MonoBehaviour, IPoolHandler, IPointerClickHandler, ISelectHandler
{

    public static Dictionary<GameObject, Hierarchy> Lookup = new Dictionary<GameObject, Hierarchy>();
    public List<Hierarchy> Children = new List<Hierarchy>();
    [SerializeField] private Hierarchy parent;

    public static List<Hierarchy> Selection { get { return HierarchyView.GetSelectedItems().Select( o => Lookup[ o ] ).ToList(); } }

    private void Awake()
    {
        Lookup[ gameObject ] = this;
    }

    private void Start()
    {
        Init();
    }

    public static Hierarchy FindByName(string name)
    {
        foreach (var root in HierarchyView.GetRoot())
        {
            var res = FindRecursive(name, Lookup[root]);
            if (res != null) return res;
        }
        return null;
    }

    private static Hierarchy FindRecursive(string name, Hierarchy start)
    {
        if (start.Children.Count == 0) return null;

        foreach (var child in start.Children)
        {
            if (child.name == name) return child;
            var res = FindRecursive(name, child);
            if (res != null) return res;
        }
        return null;
    }

    private void Init()
    {
        foreach (Transform childT in transform)
        {
            if (childT.tag == "CUI")
            {
                var child = Lookup[ childT.gameObject ];
                child.parent = this;
                Children.Add( child );
            }
        }
    }


    public void OnPoolEnter()
    {
        Lookup.Remove( gameObject );
        HierarchyView.Remove(gameObject);
        Children.Clear();
        parent = null;
    }

    public void OnPoolLeave()
    {
        Lookup[ gameObject ] = this;
        Init();
    }

    public void SetParent( Hierarchy newParent )
    {
        var rTransform = (RectTransform) transform;
        if (parent != null)
        {
            parent.Children.Remove( this );

            var position = rTransform.GetParent().GetWorldPoint( rTransform.anchorMin );
            var size = rTransform.GetSizeWorld();
            transform.SetParent( newParent.transform, false );
            rTransform.SetPositionAnchorLocal( rTransform.GetParent().GetLocalPoint( position ) );
            rTransform.SetSizeWorld( size );
        }
        else
        {
            transform.SetParent( newParent.transform, false );
        }
        parent = newParent;
        parent.Children.Add( this );
        if (!HierarchyView.IsCreated(gameObject))
            HierarchyView.AddChild( gameObject );
        HierarchyView.ChangeParent( parent.gameObject, gameObject );
    }

    public void SetParent(string parentName)
    {
        Hierarchy parent = Lookup.Values.FirstOrDefault(hierarchy => hierarchy.name == parentName);
        if (parent != null)
        {
            SetParent(parent);
        }
    }

    public void SetParent( GameObject newParent )
    {
        SetParent( Lookup[ newParent ] );
    }
    public void OnPointerClick( PointerEventData eventData )
    {
        if (HierarchyView.Select( gameObject )) return;
    }

    public void OnSelected()
    {

    }

    public void OnUnselected()
    {

    }
}
