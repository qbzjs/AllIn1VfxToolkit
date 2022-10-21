using UnityEngine;

namespace Snake4D
{
    public static class Utilities
    {
        public static Vector4Int GenerateRandomPosition(Dimension dimension, Vector4Int size)
        {
            int x = Random.Range(0, size.x);
            if (dimension == Dimension.DimensionOne) return new Vector4Int(x);

            int y = Random.Range(0, size.y);
            if (dimension == Dimension.DimensionTwo) return new Vector4Int(x, y);

            int z = Random.Range(0, size.z);
            if (dimension == Dimension.DimensionThree) return new Vector4Int(x, y, z);

            int w = Random.Range(0, size.w);
            if (dimension == Dimension.DimensionFour) return new Vector4Int(x, y, z, w);

            return new Vector4Int();
        }

        public static Vector4Int GenerateRandomDirection(Dimension dimension)
        {
            int value = 1;
            if (Random.value > 0.5) value = -1;

            if (dimension == Dimension.DimensionOne)
            {
                return new Vector4Int(value);
            }

            if (dimension == Dimension.DimensionTwo)
            {
                if (Random.value > 0.5) return new Vector4Int(value, 0);
                else return new Vector4Int(0, value);
            }

            if (dimension == Dimension.DimensionThree)
            {
                int randomChoice = Random.Range(0, 3);
                if (randomChoice == 0) return new Vector4Int(value, 0, 0);
                else if (randomChoice == 1) return new Vector4Int(0, value, 0);
                else if (randomChoice == 2) return new Vector4Int(0, 0, value);
            }

            if (dimension == Dimension.DimensionFour)
            {
                int randomChoice = Random.Range(0, 4);
                if (randomChoice == 0) return new Vector4Int(value, 0, 0, 0);
                else if (randomChoice == 1) return new Vector4Int(0, value, 0, 0);
                else if (randomChoice == 2) return new Vector4Int(0, 0, value, 0);
                else if (randomChoice == 3) return new Vector4Int(0, 0, 0, value);
            }

            Debug.LogError($"Utilities.GenerateRandomDirection(): Something went wrong. [Debug Info] dimension: {dimension}");
            return new Vector4Int();
        }
    }
}