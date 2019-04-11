using System;

namespace Library
{
  public abstract class BaseAction
  {
    internal readonly Game game;
    public BaseAction(Game game)
    {
      this.game = game;
    }
    public abstract void Invoke();
  }
}
