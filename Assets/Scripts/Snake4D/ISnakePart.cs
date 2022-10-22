namespace Snake4D
{
    public interface ISnakePart
    {
        public SnakeBody ParentSnakeBody { get; }
        public Vector4Int Position { get; }
        public Vector4Int Direction { get; }
        public ISnakePart SnakePartInFront { get; }

        public void SetParentSnakeBody(SnakeBody snakeBody);
        public Vector4Int GetPredictedPosition();
        public void UpdateSnakePart();
    }
}