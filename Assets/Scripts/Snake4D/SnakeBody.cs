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
            //! SHOULD CHECK FROM HEAD TO TAIL, AS THE HEAD MAY NOT BE MOVING

            //! Update snake prediction planning:
            //!
            //! Compare _snakePart in front predicted and current. 
            //!     if snake part in front doesnt move, self doesnt move as well, direction = 0
            //!     if snake part in front moves, self move into past position of front, direction = past direction of front  
            //! This is actually a RECURSIVE function~~
            //!   check snake part in front until front is null, 
            //!   then determine if move based on the direction
            //!   then return the (hasMoved) boolean back to the snake part that inquired it
            //!
            //! Still need to update the snake parts from the last one till the first~
            //! Cuz the recursive calls from the back and goes to the front, then comes back at the back

            // Update snake parts from the tail to the head
            // TODO : Because the GetPredictedPosition() includes a recursive call from the tail to the head, inquiring whether the head will move
            for (int i = _snakeParts.Count - 1; i >= 0; i--)
            {
                _snakeParts[i].UpdateSnakePart();
            }
        }
    }
}