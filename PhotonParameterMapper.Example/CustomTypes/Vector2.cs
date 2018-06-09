using System.Globalization;

namespace PhotonParameterMapper.Example.CustomTypes
{
    public struct Vector2 {

        public static readonly Vector2 Zero = new Vector2(0f, 0f);

        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float[] ToArray()
        {
            return new[]
            {
                X,
                Y
            };
        }

        public static Vector2 FromArray(float[] aArray)
        {
            if (aArray.Length == 2)
                return new Vector2(aArray[0], aArray[1]);

            return Zero;
        }
        
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0:F},{1:F}", new object[]
            {
                X,
                Y
            });
        }
    }
}
