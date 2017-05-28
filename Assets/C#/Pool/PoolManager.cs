using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface IPoolHandler
{
    void OnPoolEnter();
    void OnPoolLeave();
}

public enum PrefabType
{
    Cui,
    Trigger
}

public class PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public GameObject Prefab;
        public int StartCount = 50;

        public List<GameObject> Collection = new List<GameObject>();
    }

    private static PoolManager m_Instance;
    [SerializeField] private List<Pool> m_Pools;

    private void Awake()
    {
        m_Instance = this;
        for (var index = 0; index < m_Pools.Count; index++)
        {
            var pool = m_Pools[ index ];
            for (int j = 0; j < pool.StartCount; j++)
                Create( (PrefabType) index );
        }
    }

    public static GameObject Get( PrefabType type )
    {
        var pool = m_Instance.GetPool( type );
        if (pool.Collection.Count > 0)
        {
            var obj = pool.Collection[ 0 ];
            pool.Collection.RemoveAt( 0 );
            m_Instance.OnPoolLeave( type, obj );
            return obj;
        }
        m_Instance.Create( type );
        return Get( type );
    }

    private void Create( PrefabType type )
    {
        var pool = GetPool( type );
        var obj = Instantiate( pool.Prefab );
        OnPoolEnter( type, obj );
    }

    private Pool GetPool( PrefabType type )
    {
        return m_Pools[ (int) type ];
    }

    public static void Release( PrefabType type, GameObject obj )
    {
        m_Instance.OnPoolEnter( type, obj );
    }

    private void OnPoolEnter( PrefabType type, GameObject obj )
    {
        var pool = GetPool( type );
        foreach (var handler in GetHandlers( obj ))
            handler.OnPoolEnter();
        obj.SetActive( false );
        obj.transform.SetParent( transform, false );
        pool.Collection.Add( obj );
    }

    private void OnPoolLeave( PrefabType type, GameObject obj )
    {
        var pool = GetPool( type );
        pool.Collection.Remove( obj );
        obj.SetActive( true );
        foreach (var handler in GetHandlers( obj ))
            handler.OnPoolLeave();
    }

    public IEnumerable<IPoolHandler> GetHandlers( GameObject obj )
    {
        return obj.GetComponents( typeof( IPoolHandler ) ).OfType<IPoolHandler>();
    }
}
