using System;

namespace Library
{
  public class Cell
  {
    private readonly int _x;
    public int X
    {
      get
      {
        return this._x;
      }
    }

    private readonly int _y;
    public int Y
    {
      get
      {
        return _y;
      }
    }

    public Cell(int x, int y, CellRole role = CellRole.Empty)
    {
      if (x < 0 || y < 0)
      {
        throw new Exception("x and y should be greater than 0");
      }
      this._x = x;
      this._y = y;
      this._role = role;
    }
    
    private readonly CellRole _role;
    public CellRole Role
    {
      get
      {
        return _role;
      }
    }
  }
}