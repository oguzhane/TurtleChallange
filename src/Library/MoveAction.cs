using System;

namespace Library
{
  public class MoveAction : BaseAction
  {
    public MoveAction(Game game) : base(game)
    {
    }

    public override void Invoke()
    {
      // int pxChange = 0;
      // int pyChange = 0;

      // switch (game.currDir)
      // {
      //   case Direction.North:
      //     pxChange = -1;
      //     break;
      //   case Direction.East:
      //     pyChange = 1;
      //     break;
      //   case Direction.South:
      //     pxChange = 1;
      //     break;
      //   default: // Direction.West
      //     pyChange = -1;
      //     break;
      // }
      // int newX = game.currPointX + pxChange;
      // int newY = game.currPointY + pyChange;
      var (newX, newY) = Helper.CalcNextMovementPoint((game.currPointX, game.currPointY), game.currDir);
      if (newX < 0 || newX >= game.board.Width || newY < 0 || newY >= game.board.Height) throw new OutOfBoardException();

      if (game.board.Grid[newX, newY].Role == CellRole.Mine) throw new MineHitException();

      game.currPointX = newX;
      game.currPointY = newY;
    }
  }
}