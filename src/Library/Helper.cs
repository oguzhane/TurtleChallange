using System;

namespace Library
{
  public static class Helper
  {
    // Detects the next location by looking current direction and current location 
    public static (int, int) CalcNextMovementPoint((int, int) currPoint, Direction dir)
    {
      var (currX, currY) = currPoint;
      int pxChange = 0;
      int pyChange = 0;

      switch (dir)
      {
        case Direction.North:
          pxChange = -1;
          break;
        case Direction.East:
          pyChange = 1;
          break;
        case Direction.South:
          pxChange = 1;
          break;
        default: // Direction.West
          pyChange = -1;
          break;
      }
      int newX = currX + pxChange;
      int newY = currY + pyChange;

      return (newX, newY);
    }
  }
}