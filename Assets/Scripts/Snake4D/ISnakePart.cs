namespace Snake4D
{
    public interface ISnakePart
    {
        public Vector4Int Position { get; }
        public Vector4Int Direction { get; }
        public ISnakePart SnakePartInFront { get; }

        public Vector4Int GetNextPosition();
        public void UpdateSnakePart();
    }
}