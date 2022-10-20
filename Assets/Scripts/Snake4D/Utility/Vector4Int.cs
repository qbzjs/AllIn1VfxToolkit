namespace Snake4D
{
    public struct Vector4Int
    {
        public int w;
        public int x;
        public int y;
        public int z;

        public Vector4Int(int x)
        {
            this.w = 0;
            this.x = x;
            this.y = 0;
            this.z = 0;
        }

        public Vector4Int(int x, int y)
        {
            this.w = 0;
            this.x = x;
            this.y = y;
            this.z = 0;
        }

        public Vector4Int(int x, int y, int z)
        {
            this.w = 0;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector4Int(int w, int x, int y, int z)
        {
            this.w = w;
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}