namespace Snake4D
{
    public class SnakePart : ISnakePart
    {
        public SnakeBody ParentSnakeBody => _parentSnakeBody;
        public Vector4Int Position => _position;
        public Vector4Int Direction => _direction;
        public ISnakePart SnakePartInFront => _snakePartInFront;
        public bool WillMove => WillSnakePartMove();

        protected SnakeBody _parentSnakeBody;
        protected Vector4Int _position;
        protected Vector4Int _direction;
        protected Vector4Int _previousPosition;
        protected Vector4Int _previousDirection;

        ISnakePart _snakePartInFront;

        public SnakePart(Vector4Int position, Vector4Int direction, SnakePart snakePartInFront)
        {
            _position = position;
            _direction = direction;

            _previousPosition = position;
            _previousDirection = direction;

            _snakePartInFront = snakePartInFront;
        }

        public void SetParentSnakeBody(SnakeBody snakeBody)
        {
            _parentSnakeBody = snakeBody;
        }

        public virtual Vector4Int GetPredictedPosition()
        {
            if (_snakePartInFront.WillMove)
                return _snakePartInFront.Position;

            return _position;
        }

        public virtual Vector4Int GetPredictedDirection()
        {
            if (_snakePartInFront.WillMove)
                return _snakePartInFront.Direction;

            return Vector4Int.zero;
        }

        public virtual Vector4Int GetPreviousPosition() => _previousPosition;
        public virtual Vector4Int GetPreviousDirection() => _previousDirection;

        public virtual void UpdateSnakePart()
        {
            _previousPosition = _position;
            _previousDirection = _direction;

            _position = GetPredictedPosition();
            _direction = GetPredictedDirection();
        }

        protected virtual bool WillSnakePartMove()
        {
            if (_snakePartInFront.WillMove)
                return true;

            return false;
        }
    }
}