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

        public static Vector3Int GameSpaceToWorldSpace(Vector3Int gameSpaceVector)
        {
            return new Vector3Int(gameSpaceVector.x, gameSpaceVector.z, gameSpaceVector.y);
        }

        public static bool IsPositionWithinSpace(Vector4Int position, Vector4Int sizeOfSpace)
        {
            if (position.x < 0) return false;
            if (sizeOfSpace.x > 0 && position.x > sizeOfSpace.x - 1) return false;

            if (position.y < 0) return false;
            if (sizeOfSpace.y > 0 && position.y > sizeOfSpace.y - 1) return false;

            if (position.z < 0) return false;
            if (sizeOfSpace.z > 0 && position.z > sizeOfSpace.z - 1) return false;

            if (position.w < 0) return false;
            if (sizeOfSpace.w > 0 && position.w > sizeOfSpace.w - 1) return false;

            return true;
        }

        public static Vector4Int WarpPositionWithinSpace(Vector4Int position, Vector4Int sizeOfSpace)
        {
            Vector4Int warpedPosition = position;

            if (position.x < 0) warpedPosition.x = sizeOfSpace.x - Mathf.Abs(position.x);
            if (sizeOfSpace.x > 0 && position.x > sizeOfSpace.x - 1) warpedPosition.x = position.x - sizeOfSpace.x;

            if (position.y < 0) warpedPosition.y = sizeOfSpace.y - Mathf.Abs(position.y);
            if (sizeOfSpace.y > 0 && position.y > sizeOfSpace.y - 1) warpedPosition.y = position.y - sizeOfSpace.y;

            if (position.z < 0) warpedPosition.z = sizeOfSpace.z - Mathf.Abs(position.z);
            if (sizeOfSpace.z > 0 && position.z > sizeOfSpace.z - 1) warpedPosition.z = position.z - sizeOfSpace.z;

            if (position.w < 0) warpedPosition.w = sizeOfSpace.w - Mathf.Abs(position.w);
            if (sizeOfSpace.w > 0 && position.w > sizeOfSpace.w - 1) warpedPosition.w = position.w - sizeOfSpace.w;

            return warpedPosition;
        }
    }
}