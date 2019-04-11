using System;
using System.Collections.Generic;

namespace Library
{
  // Configuration for a Game
  public class Config
  {
    public int BoardHeight { get; set; }
    public int BoardWidth { get; set; }

    public List<(int, int)> Mines { get; set; }

    public (int, int) ExitPoint { get; set; }

    public (int, int) InitialPoint { get; set; }

    public Direction InitialDirection { get; set; }
  }
}