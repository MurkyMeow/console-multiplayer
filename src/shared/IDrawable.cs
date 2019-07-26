using System;

namespace ConsoleMultiplayer.Shared {
  interface IDrawable {
    (int, int) Pos { get; }
    string[] Template { get; }
    ConsoleColor Color { get; }
  }
}
