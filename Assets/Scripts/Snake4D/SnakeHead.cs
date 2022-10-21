namespace Snake4D
{
    public sealed class SnakeHead : SnakePart
    {
        public SnakeHead(Vector4Int position, Vector4Int direction) :
        base(position, direction, null)
        {

        }

        public override Vector4Int GetNextPosition()
        {
            // TODO : Handling if go through walls
            return _position + _direction;
        }

        public override void UpdateSnakePart()
        {
            // TODO : Handling if go through walls

            _position += _direction;
            // TODO : Update direction from input
        }
    }
}