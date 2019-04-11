using System;

namespace Library
{
  public class RotateAction : BaseAction
  {
    public RotateAction(Game game) : base(game)
    {
    }

    public override void Invoke()
    {
      switch (game.currDir)
      {
        case Direction.North:
          game.currDir = Direction.East;
          return;
        case Direction.East:
          game.currDir = Direction.South;
          return;
        case Direction.South:
          game.currDir = Direction.West;
          return;
        case Direction.West:
          game.currDir = Direction.North;
          return;
      }
    }
  }
}