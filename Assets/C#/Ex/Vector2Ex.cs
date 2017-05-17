namespace UnityEngine
{
    public static class Vector2Ex
    {
        public static Vector2 Parse( string p )
        {
            string[] strArrays = p.Split( new char[] { ' ' } );
            if ((int) strArrays.Length != 2)
            {
                return Vector2.zero;
            }
            return new Vector2( float.Parse( strArrays[ 0 ] ), float.Parse( strArrays[ 1 ] ) );
        }

        public static Vector2 Abs( this Vector2 input )
        {
            return new Vector2( Mathf.Abs( input.x ), Mathf.Abs( input.y ) );
        }

        public static Vector2 WithY( this Vector2 vec, float y )
        {
            return new Vector2( vec.x, y );
        }

        public static Vector2 WithX( this Vector2 vec, float x )
        {
            return new Vector2( x, vec.y );
        }

        public static Vector2 Div( this Vector2 vec, Vector2 vec2 )
        {
            return new Vector2( vec.x / vec2.x, vec.y / vec2.y );
        }
        public static string ToString( Vector2 vec )
        {
            return string.Format( "{0} {1}", vec.x, vec.y );
        }
    }
}