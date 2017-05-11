using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager : MonoBehaviour {
    
    private static ResolutionManager m_Instance;

    public static int AspectIndex = 0;


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

    private Vector2[] m_Resolutions = {new Vector2(1920f, 1080f), new Vector2(1728f, 1080f), new Vector2(1350f, 1080), new Vector2(1440f, 1080f)};

    /// <summary>
    /// Выставляет разрешение
    /// </summary>
    /// <param name="index">1 - 16:9, 2 - 16:10, 3 - 5:4, 4 - 4:3</param>
    public static void SetResolution( int index )
    {
        var resolution = m_Instance.m_Resolutions[ index];
        Screen.SetResolution(Convert.ToInt32(resolution[0]), Convert.ToInt32(resolution[1]), Screen.fullScreen, Screen.currentResolution.refreshRate);
    }
}
