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

        public override Vector4Int GetPredictedPosition()
        {
            Vector4Int predictedPosition = _position + _direction;

            if (_canPassThroughWalls)
                return Utilities.WarpPositionWithinSpace(predictedPosition, _sizeOfSpace);

            // returns out of bounds position
            // TODO : Handling on the game side!
            return predictedPosition;
        }

        public override void UpdateSnakePart()
        {
            _position = GetPredictedPosition();
            // TODO : Update direction from input
        }
    }
}