using System;
using UnityEngine;
using UnityEngine.Events;

public class AspectManager : MonoBehaviour
{
    public delegate void AspectChanged(int aspectIndex);
    public delegate void AspectPreChange( int aspectIndex );

    private static AspectManager m_Instance;

    public static int AspectIndex = 0;

    public static event AspectChanged OnChanged;
    public static event AspectPreChange OnPreChange;

    private void Awake()
    {
        m_Instance = this;
    }

    public static string GetAspect()
    {
        switch (AspectIndex)
        {
            case 0: return "16x9";
            case 1: return "16x10";
            case 2: return "5x4";
            case 3: return "4x3";
        }
        return "";
    }

    private Vector2[] m_AspectResolutions = { new Vector2( 1920f, 1080f ), new Vector2( 1728f, 1080f ), new Vector2( 1350f, 1080 ), new Vector2( 1440f, 1080f ) };
    
#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetAspect(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetAspect(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetAspect(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetAspect(3);
    }
#endif

    /// <summary>
    /// Выставляет разрешение
    /// </summary>
    /// <param name="index">1 - 16:9, 2 - 16:10, 3 - 5:4, 4 - 4:3</param>
    public void SetAspect( int index )
    {
        if (OnPreChange != null)
        {
            OnPreChange.Invoke(AspectIndex);
        }
        AspectIndex = index;
#if !UNITY_EDITOR
        var size = m_Instance.m_AspectResolutions[ index ];
        Screen.SetResolution( Convert.ToInt32( size[ 0 ] ), Convert.ToInt32( size[ 1 ] ), Screen.fullScreen, Screen.currentResolution.refreshRate );
#endif
        BackgroundManager.Instance.SetBackground( GetAspect() );
        if (OnChanged != null)
        {
            OnChanged.Invoke(index);
        }
    }
}
