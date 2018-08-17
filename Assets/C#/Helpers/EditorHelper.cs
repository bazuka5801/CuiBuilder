using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NCalc;
using UnityEngine;

public static class EditorHelper
{
    private static Regex paramEx = new Regex( "\\(.*\\)" );
    public static string Evaluate(string input, int index)
    {
        return paramEx.Replace(input, m=> HandleParam( m, index ) );
    }

    private static string HandleParam( Match m, int index )
    {
        var expr = new Expression( m.Value );
        expr.Parameters[ "i" ] = index;
        return expr.Evaluate().ToString();
    }
}
