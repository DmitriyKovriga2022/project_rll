using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExitMethods
{
    public static ExitDirection GetOppositeDirection(ExitDirection direction)
    {
        switch (direction)
        {
            case ExitDirection.Up: return ExitDirection.Down;
            case ExitDirection.Down: return ExitDirection.Up;
            case ExitDirection.Right: return ExitDirection.Left;
            case ExitDirection.Left: return ExitDirection.Right;
            default: return direction;
        }
    }

    public static bool AreDirectionsOpposite(ExitDirection firstDirection, ExitDirection secondDirection)
    {
        return firstDirection == GetOppositeDirection(secondDirection);
    }

    
    public static Vector2Int ConvertDirectioToVector2(ExitDirection dir)
    {
        switch (dir)
        {
            case ExitDirection.Up: return Vector2Int.up;
            case ExitDirection.Down: return Vector2Int.down;
            case ExitDirection.Left: return Vector2Int.left;
            case ExitDirection.Right: return Vector2Int.right;
            default: return Vector2Int.zero;
        }
    }
}
