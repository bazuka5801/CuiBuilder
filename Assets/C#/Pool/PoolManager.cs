using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IPoolHandler
{
    void OnPoolEnter();
    void OnPoolLeave();
}

[System.Serializable]
public class Pool
{
    
}

public class PoolManager : MonoBehaviour {

    [SerializeField] private GameObject Prefab;
    [SerializeField] private int StartCount;

    [SerializeField] private List<GameObject> Collection = new List<GameObject>();

    private static PoolManager m_Instance;

    private void Awake()
    {
        m_Instance = this;
        for (int j = 0; j < StartCount; j++)
            Create();
    }

    public GameObject Get()
    {
        if (Collection.Count > 0)
        {
            var obj = Collection[0];
            Collection.RemoveAt(0);
            OnPoolLeave(obj);
            return obj;
        }
        Create();
        return Get();
    }

    private void Create()
    {
        var obj = Object.Instantiate(Prefab);
        OnPoolEnter(obj);
    }

    public static void Release(GameObject obj)
    {
        m_Instance.OnPoolEnter(obj);
    }

    private void OnPoolEnter(GameObject obj)
    {
        foreach (var handler in GetHandlers(obj))
            handler.OnPoolEnter();
        obj.SetActive(false);
        obj.transform.SetParent(transform, false);
        Collection.Add(obj);
    }

    private void OnPoolLeave(GameObject obj)
    {
        Collection.Remove(obj);
        obj.SetActive(true);
        foreach (var handler in GetHandlers(obj))
            handler.OnPoolLeave();
    }

    public IEnumerable<IPoolHandler> GetHandlers(GameObject obj)
    {
        return obj.GetComponents(typeof(IPoolHandler)).OfType<IPoolHandler>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            var obj = Get();
            obj.transform.SetParent(GameObject.Find("Overlay").transform, false);
            HierarchyView.AddChild(obj);
            obj.GetComponent<RectTransform>().SetRect(new Vector2(0.3f, 0.2f), new Vector2(0.4f, 0.3f));
            obj.GetComponent<RectTransform>().SetSizePixel(new Vector2(100, 100));
            Hierarchy.Lookup[obj].SetParent(Hierarchy.Lookup[GameObject.Find("Overlay")]);
        }
    }
}
