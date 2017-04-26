using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {
    public enum CursorMode
    {
        Arrow,
        Cross,
        NS,
        EW,
        NWSE,
        NESW
    }

    public Texture2D Arrow, Cross, NS, EW, NWSE, NESW;

    private Dictionary<CursorMode, Vector2> offsets;
    private static CursorManager _instance;

    private const int OFFSET = 8;

    static readonly Dictionary<string, CursorMode> AnchorModes = new Dictionary<string, CursorMode>()
    {
        { "0.5 0.5", CursorMode.Cross },

        { "0.0 0.0", CursorMode.NESW },
        { "1.0 1.0", CursorMode.NESW },

        { "0.0 1.0", CursorMode.NWSE },
        { "1.0 0.0", CursorMode.NWSE },
        
        { "0.5 1.0", CursorMode.NS },
        { "0.5 0.0", CursorMode.NS },

        { "1.0 0.5", CursorMode.EW },
        { "0.0 0.5", CursorMode.EW },
    };

    void Awake()
    {
        _instance = this;
        offsets = new Dictionary<CursorMode, Vector2>()
        {
            {CursorMode.Arrow, Vector2.zero},
            {CursorMode.Cross, new Vector2(OFFSET,OFFSET)},
            {CursorMode.NS, new Vector2(0,OFFSET)},
            {CursorMode.EW, new Vector2(OFFSET,0)},
            {CursorMode.NESW, new Vector2(OFFSET,OFFSET)},
            {CursorMode.NWSE, new Vector2(OFFSET,OFFSET)},
        };
    }

    public static void SetCursor(CursorMode mode)
    {
        _instance.set_cursor(mode);
    }
    public static void SetCursorByAnchor(string anchor)
    {
        if (!AnchorModes.ContainsKey(anchor))Debug.LogError("Anchor not contained: "+anchor);
        SetCursor(AnchorModes[anchor]);
    }

    private void set_cursor(CursorMode mode)
    {
        Cursor.SetCursor(GetTexture2D(mode), offsets[mode], UnityEngine.CursorMode.Auto);
    }

    public Texture2D GetTexture2D(CursorMode mode)
    {
        switch (mode)
        {
            case CursorMode.Arrow:
                return Arrow;
            case CursorMode.Cross:
                return Cross;
            case CursorMode.NS:
                return NS;
            case CursorMode.EW:
                return EW;
            case CursorMode.NESW:
                return NESW;
            case CursorMode.NWSE:
                return NWSE;
        }
        return null;
    }
}
