using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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

    static bool IsServerCertificateValidation = false;
    static void ServerCertificateValidation()
    {
        IsServerCertificateValidation = true;
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
    }

    static bool MyRemoteCertificateValidationCallback( System.Object sender,
        X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors )
    {
        bool isOk = true;
        // If there are errors in the certificate chain,
        // look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[ i ].Status == X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    continue;
                }
                chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan( 0, 1, 0 );
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                bool chainIsValid = chain.Build( (X509Certificate2) certificate );
                if (!chainIsValid)
                {
                    isOk = false;
                    break;
                }
            }
        }
        return isOk;
    }

    private static Texture Add( string url )
    {
        if (!IsServerCertificateValidation) ServerCertificateValidation();
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
        catch(Exception ex)
        {
            Debug.LogError("Failed download image: "+ex.Message);
            return null;
        }
    }

    public static bool IsUrl( string text )
    {
        return urlRegex.IsMatch( text );
    }
}
