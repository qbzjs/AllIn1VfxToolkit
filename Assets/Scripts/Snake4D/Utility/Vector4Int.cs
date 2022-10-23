using UnityEngine;

namespace Snake4D
{
    public struct Vector4Int
    {
        public int x;
        public int y;
        public int z;
        public int w;

        public float magnitude => CalculateMagnitude();

        /// <summary>
        /// Creates a new vector with given x component and sets y, z and w to zero.
        /// </summary>
        /// <param name="x"></param>
        public Vector4Int(int x)
        {
            this.x = x;
            this.y = 0;
            this.z = 0;
            this.w = 0;
        }

        /// <summary>
        /// Creates a new vector with given x, y components and sets z and w to zero.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector4Int(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.z = 0;
            this.w = 0;
        }

        /// <summary>
        /// Creates a new vector with given x, y, z components and sets w to zero.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector4Int(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 0;
        }

        /// <summary>
        /// Creates a new vector with given x, y, z, w components.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="w"></param>
        public Vector4Int(int x, int y, int z, int w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        private float CalculateMagnitude()
        {
            float x2 = x * x;
            float y2 = y * y;
            float z2 = z * z;
            float w2 = w * w;

            return Mathf.Sqrt(x2 + y2 + z2 + w2);
        }

        public static Vector4Int operator +(Vector4Int a, Vector4Int b)
            => new Vector4Int(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);

        public static Vector4Int operator -(Vector4Int a, Vector4Int b)
            => new Vector4Int(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);

        /// <summary>
        /// Returns the distance between a and b.
        /// </summary>
        public static float Distance(Vector4Int a, Vector4Int b)
        {
            float x2 = Mathf.Pow((a.x - b.x), 2);
            float y2 = Mathf.Pow((a.y - b.y), 2);
            float z2 = Mathf.Pow((a.z - b.z), 2);
            float w2 = Mathf.Pow((a.w - b.w), 2);

            return Mathf.Abs(Mathf.Sqrt(x2 + y2 + z2 + w2));
        }

        public static Vector3Int ToVector3Int(Vector4Int a)
        {
            return new Vector3Int(a.x, a.y, a.z);
        }

        public Vector3Int ToVector3Int()
        {
            return ToVector3Int(this);
        }
    }
}