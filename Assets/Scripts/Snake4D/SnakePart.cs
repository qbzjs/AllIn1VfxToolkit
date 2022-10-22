namespace Snake4D
{
    public class SnakePart : ISnakePart
    {
        public SnakeBody ParentSnakeBody => _parentSnakeBody;
        public Vector4Int Position => _position;
        public Vector4Int Direction => _direction;
        public ISnakePart SnakePartInFront => _snakePartInFront;

        protected SnakeBody _parentSnakeBody;
        protected Vector4Int _position;
        protected Vector4Int _direction;

        ISnakePart _snakePartInFront;

        public SnakePart(Vector4Int position, Vector4Int direction, SnakePart snakePartInFront)
        {
            _position = position;
            _direction = direction;
            _snakePartInFront = snakePartInFront;
        }

        public void SetParentSnakeBody(SnakeBody snakeBody)
        {
            _parentSnakeBody = snakeBody;
        }

        public virtual Vector4Int GetPredictedPosition()
        {
            return _snakePartInFront.Position;
        }

        public virtual void UpdateSnakePart()
        {
            _position = GetPredictedPosition();
            _direction = _snakePartInFront.Direction;
        }
    }
}