namespace Snake4D
{
    public enum GameMode
    {
        DimensionOne,
        DimensionTwo,
        DimensionThree,
        DimensionFour
    }

    public struct SnakeGameParameters
    {
        public GameMode GameMode;
        public Vector4Int Size;
    }
}