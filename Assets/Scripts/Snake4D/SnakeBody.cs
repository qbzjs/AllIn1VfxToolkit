using System.Collections.Generic;

namespace Snake4D
{
    public class SnakeBody
    {
        public SnakeHead SnakeHead => _snakeParts.Count > 0 ? _snakeParts[0] as SnakeHead : null;
        public int Count => _snakeParts.Count;
        public SnakeGameParameters Parameters => _parameters;

        List<ISnakePart> _snakeParts = new List<ISnakePart>();
        SnakeGameParameters _parameters;

        public SnakeBody(SnakeGameParameters parameters)
        {
            _parameters = parameters;
            SpawnSnakeHead();
        }

        public void UpdateSnakeBody()
        {
            // Update snake parts from the tail to the head
            // Because the GetPredictedPosition() includes a recursive call from the tail to the head, inquiring whether the head will move.
            for (int i = _snakeParts.Count - 1; i >= 0; i--)
            {
                _snakeParts[i].UpdateSnakePart();
            }
        }

        public List<Vector4Int> GetCurrentPositions()
        {
            List<Vector4Int> currentPositions = new List<Vector4Int>();
            foreach (ISnakePart snakePart in _snakeParts)
            {
                currentPositions.Add(snakePart.Position);
            }
            return currentPositions;
        }

        public List<Vector4Int> GetPreviousPositions()
        {
            List<Vector4Int> previousPositions = new List<Vector4Int>();
            foreach (ISnakePart snakePart in _snakeParts)
            {
                previousPositions.Add(snakePart.GetPreviousPosition());
            }
            return previousPositions;
        }

        public List<Vector4Int> GetCurrentDirections()
        {
            List<Vector4Int> currentDirections = new List<Vector4Int>();
            foreach (ISnakePart snakePart in _snakeParts)
            {
                currentDirections.Add(snakePart.Direction);
            }
            return currentDirections;
        }

        public List<Vector4Int> GetPreviousDirections()
        {
            List<Vector4Int> previousDirections = new List<Vector4Int>();
            foreach (ISnakePart snakePart in _snakeParts)
            {
                previousDirections.Add(snakePart.GetPreviousDirection());
            }
            return previousDirections;
        }

        public void GrowTail()
        {
            ISnakePart currentTail = _snakeParts[_snakeParts.Count - 1];
            SnakePart newTail = new SnakePart(currentTail.GetPreviousPosition(), currentTail.GetPreviousDirection(), currentTail);
            AddSnakePartToBody(newTail);
        }

        private void SpawnSnakeHead()
        {
            if (_snakeParts.Count > 0) _snakeParts.Clear();

            Vector4Int randomPosition = Utilities.GenerateRandomPosition(_parameters.Dimension, _parameters.Size);
            Vector4Int randomDirection = Utilities.GenerateRandomDirection(_parameters.Dimension);

            SnakeHead snakeHead = new SnakeHead(randomPosition, randomDirection);
            AddSnakePartToBody(snakeHead);
        }

        private void AddSnakePartToBody(ISnakePart snakePart)
        {
            snakePart.SetParentSnakeBody(this);
            _snakeParts.Add(snakePart);
        }
    }
}