namespace UnityEngine
{
    public static class Vector2Ex
    {
        public static Vector2 Parse(string p)
        {
            string[] strArrays = p.Split(new char[] { ' ' });
            if ((int)strArrays.Length != 2)
            {
                return Vector2.zero;
            }
            return new Vector2(float.Parse(strArrays[0]), float.Parse(strArrays[1]));
        }

        public static Vector2 Abs(this Vector2 input)
        {
            return new Vector2(Mathf.Abs(input.x), Mathf.Abs(input.y));
        }
    }
}