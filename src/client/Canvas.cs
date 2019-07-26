using System;
using System.Linq;
using ConsoleMultiplayer.Shared;

namespace ConsoleMultiplayer.Client {
  static class Canvas {
    static void Draw(string[] template, (int, int) pos, ConsoleColor color) {
      var (x, y) = pos;
      Console.ForegroundColor = color;
      foreach (var line in template) {
        Console.SetCursorPosition(x, y++);
        Console.WriteLine(string.Join("", line));
      }
    }
    static public void Draw(IDrawable drawable) {
      Draw(drawable.Template, drawable.Pos, drawable.Color);
    }
    static public void Erase(IDrawable drawable) {
      var empty = drawable.Template
        .Select(v => new String(' ', v.Length));
      Draw(empty.ToArray(), drawable.Pos, drawable.Color);
    }
  }
}
