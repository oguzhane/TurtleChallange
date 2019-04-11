using System;

namespace Library
{
  public class Game
  {

    public Game() { }

    public Board board;

    public int currPointX
    {
      get;
      internal set;
    }
    public int currPointY
    {
      get;
      internal set;
    }
    public Direction currDir
    {
      get;
      internal set;
    }

    // returns true if turtle reaches the exit point, otherwise returns false
    public bool InvokeAction(BaseAction action)
    {
      if (action.game != this) throw new Exception("action cannot be invoke-able because of invalid game instance");
      action.Invoke();
      if (board.Grid[currPointX, currPointY].Role == CellRole.ExitPoint) return true;

      return false;
    }

    public void Init(Config config)
    {
      if (config == null) throw new NullReferenceException("config cannot be null");
      var boardHeight = config.BoardHeight;
      var boardWidth = config.BoardWidth;
      var (initX, initY) = config.InitialPoint;
      var (exitX, exitY) = config.ExitPoint;

      if (initX < 0 || initX >= boardWidth || initY < 0 || initY >= boardHeight) throw new Exception("Invalid initial point");
      /******
        Exit point is supposed to be on the edge of board. 
        However, i left this line commented because there is no information about it in the pdf file
      ******/
      // if (exitX > 0 && exitX < (boardHeight - 1) && exitY > 0 && exitY < (boardWidth -1 )) throw new Exception("Invalid exit point");

      board = new Board(boardWidth, boardHeight);
      currPointX = initX;
      currPointY = initY;
      currDir = config.InitialDirection;

      config.Mines.ForEach((p) =>
      {
        var (x, y) = p;
        board.Grid[x, y] = new Cell(x, y, CellRole.Mine); // it will throw out of index exception if x or y is invalid
      });
      if (board.Grid[exitX, exitY] != null) throw new Exception("Exit point cannot have a mine");
      board.Grid[exitX, exitY] = new Cell(exitX, exitY, CellRole.ExitPoint);

      // each member of rest of the grid is empty cell
      for (int i = 0; i < board.Width; i++)
        for (int j = 0; j < board.Height; j++)
          if (board.Grid[i, j] == null) board.Grid[i, j] = new Cell(i, j);
    }
  }
}