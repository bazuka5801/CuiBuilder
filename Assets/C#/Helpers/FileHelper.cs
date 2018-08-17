using System.IO;
using System.Text.RegularExpressions;
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

    public static void SaveJson( string filePath, string data )
    {
        filePath = SanitizeName(filePath);
        var ext = Path.GetExtension(filePath);
        if (string.IsNullOrEmpty(ext) == false)
            filePath = filePath.Replace(ext, "");
        filePath += ".json";
        string directory = GetDirectoryName(filePath);
        if (directory != null && Directory.Exists(directory) == false)
            Directory.CreateDirectory(directory);
        File.WriteAllText(filePath, data);
    }
    
    public static string GetDirectoryName(string name)
    {
        try
        {
            name = name.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            return name.Substring(0, name.LastIndexOf(Path.DirectorySeparatorChar));
        }
        catch
        {
            return (string) null;
        }
    }
    
    public static string SanitizeName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return string.Empty;
        name = name.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        name = Regex.Replace(name, "[" + Regex.Escape(new string(Path.GetInvalidPathChars())) + "]", "_");
        name = Regex.Replace(name, "\\.+", ".");
        return name.TrimStart('.');
    }

}
