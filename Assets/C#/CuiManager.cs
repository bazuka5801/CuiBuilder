using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class CuiManager : MonoBehaviour
{
    [SerializeField] private PoolManager m_Pool;
    [SerializeField] private GameObject m_RootDefault;
    private static CuiManager m_Instance;

    private void Awake()
    {
        m_Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown( KeyCode.X ))
        {
            var obj = CreateInternal();
            var rTransform = obj.GetComponent<RectTransformComponent>();
            rTransform.SetPixelLocalPosition( new Vector2( 100, 100 ));
            rTransform.SetPixelSize( new Vector2( 100, 100 ) );
        }
    }

    private CUIObject CreateInternal()
    {
        var obj = m_Pool.Get();
        obj.transform.SetParent( m_RootDefault.transform, false );
        Hierarchy.Lookup[ obj ].SetParent( Hierarchy.Lookup[ m_RootDefault ] );
        return CUIObject.Lookup[obj];
    }

    public static CUIObject Create()
    {
        return m_Instance.CreateInternal();
    }
    
    public static Dictionary<string, MethodInfo> GetCuiFieldHandlers( Type type )
    {
        return type
            .GetMethods( BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance )
            .Where( m => m.GetCustomAttributes( typeof( CuiFieldAttribute ), false ).Length > 0 )
            .ToDictionary(
                m => ( (CuiFieldAttribute) m.GetCustomAttributes( typeof( CuiFieldAttribute ), false )[ 0 ] ).FieldName,
                m => m );
    }

    public static Dictionary<string, MethodInfo> GetCuiFieldHandlers<T>()
    {
        return GetCuiFieldHandlers( typeof( T ) );
    }
}
