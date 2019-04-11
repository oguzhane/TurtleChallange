using System;
using System.Collections.Generic;
using System.IO;
using Library;
using YamlDotNet.Serialization;

namespace App
{
  class Program
  {

    // Even I had a chance to use c# attributes in order to map yaml attrs. to Config object instance
    // I wanted to avoid that because we should try not to use 3rd library as a dependency for our main library
    static Config ReadConfigFromYamlFile(string fileName)
    {
      if (!File.Exists(fileName)) fileName = Path.Combine(Directory.GetCurrentDirectory(), fileName);
      if (!File.Exists(fileName)) throw new Exception("settings file doesnt exist");

      var input = new StringReader(File.ReadAllText(fileName));

      var deserializer = new DeserializerBuilder()
          .Build();
      Dictionary<object, object> conf = deserializer.Deserialize(input) as Dictionary<object, object>;

      // Dictionary<object, object> conf
      var c = new Config()
      {
        BoardHeight = int.Parse(conf["M"].ToString()),
        BoardWidth = int.Parse(conf["N"].ToString()),
      };
      var initPoint = conf["init-point"] as List<Object>;
      var initDirVal = conf["init-dir"].ToString();
      var exitPoint = conf["exit-point"] as List<Object>;
      var mines = conf["mines"] as List<Object>;

      c.InitialPoint = (int.Parse(initPoint[0] as string), int.Parse(initPoint[1] as string));
      c.ExitPoint = (int.Parse(exitPoint[0] as string), int.Parse(exitPoint[1] as string));

      switch (initDirVal)
      {
        case "N":
          c.InitialDirection = Direction.North;
          break;
        case "E":
          c.InitialDirection = Direction.East;
          break;
        case "S":
          c.InitialDirection = Direction.South;
          break;
        case "W":
          c.InitialDirection = Direction.West;
          break;
        default:
          throw new Exception("Invalid init-dir value");
      }
      c.Mines = new List<(int, int)>();
      foreach (List<Object> mine in mines)
      {
        c.Mines.Add((int.Parse(mine[0] as string), int.Parse(mine[1] as string)));
      }
      return c;
    }

    public static List<BaseAction> GetActionsFromFile(string fileName, Game g)
    {
      if (!File.Exists(fileName)) fileName = Path.Combine(Directory.GetCurrentDirectory(), fileName);
      if (!File.Exists(fileName)) throw new Exception("actions file doesnt exist");

      List<BaseAction> actions = new List<BaseAction>();
      using (StreamReader sr = File.OpenText(fileName))
      {
        string s = String.Empty;
        while ((s = sr.ReadLine()) != null)
        {
          if (string.IsNullOrWhiteSpace(s)) continue;
          if (s == "move") actions.Add(new MoveAction(g));
          else if (s == "rotate") actions.Add(new RotateAction(g));
          else throw new Exception("Invalid action");
        }
      }
      return actions;
    }

    public static void PrintDangerState(Game g)
    {
      var (newX, newY) = Helper.CalcNextMovementPoint((g.currPointX, g.currPointY), g.currDir);
      if (newX < 0 || newX >= g.board.Width || newY < 0 || newY >= g.board.Height)
      {
        Console.WriteLine("  -> You are in danger, Don't get out!");
        return;
      }

      if (g.board.Grid[newX, newY].Role == CellRole.Mine)
      {
        Console.WriteLine("  -> You are in danger, Don't hit a mine!");
        return;
      }
    }

    static void Main(string[] args)
    {
      Game g = new Game();
      if (args.Length != 2)
      {
        Console.WriteLine("Usage: app <settings-file> <movements-file>");
        return;
      }

      var config = ReadConfigFromYamlFile(args[0]);
      var actions = GetActionsFromFile(args[1], g);
      try
      {
        g.Init(config);
      }
      catch (System.Exception ex)
      {
        Console.Write($"ERR: ${ex.ToString()}");
        return;
      }

      int seqNum = 0;
      foreach (BaseAction action in actions)
      {
        bool reachedExit = false;
        try
        {
          reachedExit = g.InvokeAction(action);
          Console.WriteLine($"Sequence {++seqNum}: Success!");
          if (reachedExit)
          {
            Console.WriteLine("  -> Exist is found");
            Console.WriteLine("  -> CONGRATULATIONS!");
            return;// THE REST OF THE ACTIONS WILL BE DISCARDED
          }
          PrintDangerState(g);
        }
        catch (OutOfBoardException)
        {
          Console.WriteLine($"Sequence {++seqNum}: You cannot get out of the board!");
        }
        catch (MineHitException)
        {
          Console.WriteLine($"Sequence {++seqNum}: Mine hit!");
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Sequence {++seqNum}: ERR: {ex.Message}");
        }
      }
    }
  }
}
