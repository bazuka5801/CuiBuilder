using System.IO;
using UnityEngine;

public static class FileHelper
{

    public static Texture2D LoadImage( string filePath )
    {
        Texture2D tex = null;

        if (File.Exists( filePath ))
        {
            var fileData = File.ReadAllBytes( filePath );
            tex = new Texture2D( 2, 2 );
            tex.LoadImage( fileData ); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }

}
