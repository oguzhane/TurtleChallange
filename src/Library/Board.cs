using System;

namespace Library
{
  public class Board
  {
    private readonly Cell[,] _grid;
    public Cell[,] Grid
    {
      get
      {
        return _grid;
      }
    }

    private readonly int _height;
    public int Height
    {
      get
      {
        return _height;
      }
    }

    private readonly int _width;
    public int Width
    {
      get
      {
        return _width;
      }
    }

    public Board(int width, int height)
    {
      if (height < 0 || width < 0)
      {
        throw new Exception("height and width should be greater than 0");
      }
      this._height = height;
      this._width = width;
      this._grid = new Cell[this._width, this._height];
    }
  }
}