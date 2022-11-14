using logoforprinter;

namespace logoforprinter.Models
{
  public class Logo
  {
    public static void PrintLogo(string direction)
    {
      Console.Write(" ╔══╗  ╔══════╗  ╔══════╗  ╔══════╗     ▄▄▄▄▄▄   ▄▄▄▄▄▄   ▄  ▄      ▄  ▄▄▄▄▄▄▄  ▄▄▄▄▄▄▄  ▄▄▄▄▄▄   \n" +
                    " ║  ║  ║  ╔╗  ║  ║  ╔═══╣  ║  ╔╗  ║  F  █     █  █     █  █  █▀▄    █     █     █        █     █  \n" +
                    " ║  ║  ║  ║║  ║  ║  ║ ╦═╗  ║  ║║  ║  O  █▄▄▄▄▄▀  █▄▄▄▄▄▀  █  █  ▀▄  █     █     █▀▀▀     █▄▄▄▄▄▀  \n" +
                    " ║  ║  ║  ╚╝  ║  ║  ╚═╝ ║  ║  ╚╝  ║  R  █        █   ▀▄   █  █    ▀▄█     █     █        █   ▀▄   \n" +
                    " ║  ║  ╚══════╝  ╚══════╣  ╚══════╝     ▀        ▀     ▀  ▀  ▀      ▀     ▀     ▀▀▀▀▀▀▀  ▀     ▀  ");
      Console.ForegroundColor = ConsoleColor.DarkYellow; Console.Write("["); Console.ResetColor(); Console.Write($"Ver {Program.Version}");
      Console.ForegroundColor = ConsoleColor.DarkYellow; Console.Write("]\n"); Console.ResetColor();
      Console.Write(" ║  ╚══════════════════════>>>");
      //Console.ForegroundColor = ConsoleColor.Magenta; Console.Write("cpcl"); Console.ResetColor();
      DirectionRight(direction);
      Console.Write(">>>═════════════════════════════════════════════════════════════════════╗\n" +
                    " ╚════════════════════════════════════════════════════════<<<");
      //Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("prim"); Console.ResetColor();
      DirectionLeft(direction);
      Console.Write("<<<══════════════════════════════════════╝\n");
    }

    public static void DirectionRight(string direction)
    {
      if (direction == "t2i")
      {
        Console.ForegroundColor = ConsoleColor.Magenta; Console.Write("cpcl"); Console.ResetColor();
      }

      else
      {
        Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("prim"); Console.ResetColor();
      }
    }

    public static void DirectionLeft(string direction)
    {
      if (direction == "i2t")
      {
        Console.ForegroundColor = ConsoleColor.Magenta; Console.Write("cpcl"); Console.ResetColor();
      }

      else
      {
        Console.ForegroundColor = ConsoleColor.Cyan; Console.Write("prim"); Console.ResetColor();
      }
    }
  }
}