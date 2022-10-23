namespace Snake4D
{
    public sealed class SnakeHead : SnakePart
    {
        Vector4Int _sizeOfSpace => _parentSnakeBody.Parameters.Size;
        bool _canPassThroughWalls => _parentSnakeBody.Parameters.PassThroughWalls;

        public SnakeHead(Vector4Int position, Vector4Int direction) :
        base(position, direction, null)
        {

        }

        public void ChangeDirection(Vector4Int newDirection)
        {
            // POLISH : check if it is a direction vector [eg. (1 0 0 0) and its permutations]
            if (newDirection.magnitude != 1) throw new System.InvalidOperationException("newDirection magnitude should be 1!");

            _direction = newDirection;
        }

        public override Vector4Int GetPredictedPosition()
        {
            Vector4Int predictedPosition = _position + _direction;

            if (_canPassThroughWalls)
                return Utilities.WarpPositionWithinSpace(predictedPosition, _sizeOfSpace);

            return predictedPosition;
        }

        public override Vector4Int GetPredictedDirection()
        {
            return _direction;
        }

        public override void UpdateSnakePart()
        {
            _position = GetPredictedPosition();
            // TODO : Update direction from input
        }

        protected override bool WillSnakePartMove()
        {
            if (_direction == Vector4Int.zero)
                return false;

            return true;
        }
    }
}