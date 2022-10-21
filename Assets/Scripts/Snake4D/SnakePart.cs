namespace Snake4D
{
    public class SnakePart : ISnakePart
    {
        public Vector4Int Position => _position;
        public Vector4Int Direction => _direction;
        public ISnakePart SnakePartInFront => _snakePartInFront;

        protected Vector4Int _position;
        protected Vector4Int _direction;

        ISnakePart _snakePartInFront;

        public SnakePart(Vector4Int position, Vector4Int direction, SnakePart snakePartInFront)
        {
            _position = position;
            _direction = direction;
            _snakePartInFront = snakePartInFront;
        }

        public virtual Vector4Int GetNextPosition()
        {
            return _snakePartInFront.Position;
        }

        public virtual void UpdateSnakePart()
        {
            _position = _snakePartInFront.Position;
            _direction = _snakePartInFront.Direction;
        }
    }
}