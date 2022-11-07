using UnityEngine;

namespace Snake4D
{
    public sealed class SnakeHead : SnakePart
    {
        Vector4Int _sizeOfSpace => _parentSnakeBody.Parameters.Size;
        bool _canPassThroughWalls => _parentSnakeBody.Parameters.PassThroughWalls;

        Vector4Int _userInputDirection;

        public SnakeHead(Vector4Int position, Vector4Int userInputDirection) :
        base(position, userInputDirection, null)
        {
            _userInputDirection = userInputDirection;
        }

        public void ChangeUserInputDirection(Vector4Int newDirection)
        {
            // Already covers all possible cases as Vector4Int is limited to integers only
            if (newDirection.magnitude != 1) throw new System.InvalidOperationException("newDirection magnitude should be 1!");

            // Cannot go in the opposite direction.
            // For normalized vectors, dot product == -1 => Opposite direction.
            if (Vector4Int.Dot(_userInputDirection, newDirection) == -1) return;

            _userInputDirection = newDirection;
        }

        public override Vector4Int GetPredictedPosition()
        {
            Vector4Int predictedPosition = _position + _userInputDirection;

            if (_canPassThroughWalls)
                return Utilities.WarpPositionWithinSpace(predictedPosition, _sizeOfSpace);

            return predictedPosition;
        }

        protected override bool WillSnakePartMove()
        {
            if (_userInputDirection == Vector4Int.zero)
                return false;

            return true;
        }
    }
}