using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance;

    Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    private Image m_Image;
    private bool m_Inventory = false;

    private void Awake()
    {
        Instance = this;
        m_Image = GetComponent<Image>();
        Load();
    }

    void Load()
    {
        foreach (var filename in Directory.GetFiles( "Background" ))
        {
            Texture2D t = FileHelper.LoadImage( filename );
            Sprite s = Sprite.Create( t, new Rect( 0, 0, t.width, t.height ), new Vector2( 0.5f, 0.5f ) );
            sprites.Add( Path.GetFileNameWithoutExtension( filename ), s );
        }
        SetBackground( AspectManager.GetAspect() );
    }

    public void ToggleInventory()
    {
        m_Inventory = !m_Inventory;
        SetBackground( AspectManager.GetAspect() );
    }

    public void SetBackground( string res )
    {
        m_Image.sprite = sprites[ res + ( m_Inventory ? "_Inv" : "" ) ];
    }

}
