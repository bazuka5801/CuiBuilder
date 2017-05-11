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

    static readonly Dictionary<Vector2, CursorMode> AnchorModes = new Dictionary<Vector2, CursorMode>()
    {
        { new Vector2(0.5f,0.5f), CursorMode.Cross },

        { new Vector2(0.0f,0.0f), CursorMode.NESW },
        { new Vector2(1.0f,1.0f), CursorMode.NESW },

        {  new Vector2(0.0f,1.0f), CursorMode.NWSE },
        {  new Vector2(1.0f,0.0f), CursorMode.NWSE },

        {  new Vector2(0.5f,1.0f), CursorMode.NS },
        {  new Vector2(0.5f,0.0f), CursorMode.NS },

        {  new Vector2(1.0f,0.5f), CursorMode.EW },
        {  new Vector2(0.0f,0.5f), CursorMode.EW },
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
    public static void SetCursorByAnchor(Vector2 anchor)
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
