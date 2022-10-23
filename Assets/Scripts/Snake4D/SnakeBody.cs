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
        }

        public void Add(ISnakePart snakePart)
        {
            snakePart.SetParentSnakeBody(this);
            _snakeParts.Add(snakePart);
        }

        public void Clear()
            => _snakeParts.Clear();

        public void UpdateSnakeBody()
        {
            // Update snake parts from the tail to the head
            // Because the GetPredictedPosition() includes a recursive call from the tail to the head, inquiring whether the head will move.
            for (int i = _snakeParts.Count - 1; i >= 0; i--)
            {
                _snakeParts[i].UpdateSnakePart();
            }
        }
    }
}