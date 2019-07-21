using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class CuiManager : MonoBehaviour
{
    [SerializeField] private GameObject m_RootDefault;
    private static CuiManager m_Instance;

    private void Awake()
    {
        m_Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown( KeyCode.X ))
            CreateInternalParent();
    }

    private CUIObject CreateInternalParent() {
        var obj = PoolManager.Get(PrefabType.Cui);

        GameObject selectedObj = null;
        try {
            var selected = CUIObject.Selection; //Get the currently selected object, this throws errors if the selection is HUD/OVERLAY
            selectedObj = HierarchyView.GetCurrent().Where(selected.Contains).Select(p => p.gameObject).ToList()[0];
        } catch (Exception) {

        }

        var rTransform = obj.GetComponent<RectTransformComponent>();
        if (selectedObj != null) {
            obj.transform.SetParent(selectedObj.transform, false);
            Hierarchy.Lookup[ obj ].SetParent( Hierarchy.Lookup[ selectedObj ] );
            rTransform.SetPixelLocalPosition(new Vector2(0, 0));
        } else {
            obj.transform.SetParent(m_RootDefault.transform, false);
            Hierarchy.Lookup[ obj ].SetParent( Hierarchy.Lookup[ m_RootDefault ] );
            rTransform.SetPixelLocalPosition(new Vector2(100, 100));
        }
        rTransform.SetPixelSize(new Vector2(100, 100));

        return CUIObject.Lookup[ obj ];
    }

    private CUIObject CreateInternal()
    {
        var obj = PoolManager.Get(PrefabType.Cui);
        obj.transform.SetParent(m_RootDefault.transform, false);
        Hierarchy.Lookup[ obj ].SetParent( Hierarchy.Lookup[ m_RootDefault ] );
        return CUIObject.Lookup[ obj ];
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
