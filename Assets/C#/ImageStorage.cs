using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;

public static class ImageStorage
{
    private static Dictionary<string, Texture> cache = new Dictionary<string, Texture>();
    private static Regex urlRegex = new Regex( "^http(s)?://([\\w-]+.)+[\\w-]+(/[\\w- ./?%&=])?$" );
    public static Texture Get( string url )
    {
        Texture sprite;
        return cache.TryGetValue( url, out sprite ) ? sprite : Add( url );
    }

    private static Texture Add( string url )
    {
        if (!IsUrl( url ))
        {
            return null;
        }
        try
        {
            byte[] data = new WebClient().DownloadData( url );
            Texture2D texture = new Texture2D( 64, 64, TextureFormat.ARGB32, false );
            texture.LoadImage( data );
            texture.name = "sprite";
            cache[ url ] = texture;
            return texture;
        }
        catch
        {
            return null;
        }
    }

    public static bool IsUrl( string text )
    {
        return urlRegex.IsMatch( text );
    }
}
