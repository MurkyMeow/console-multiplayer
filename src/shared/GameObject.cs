using System;

namespace ConsoleMultiplayer.Shared {
  enum Direction {
    up, down, left, right,
  }
  class GameObject : NetworkEntity<GameObject> {
    [NetworkVar] int id;
    [NetworkVar] int x;
    [NetworkVar] int y;
    [NetworkVar] string view;
    [NetworkVar] ConsoleColor color;

    public int ID => id;

    public GameObject() {}
    public GameObject(int id, int x, int y, string view, ConsoleColor color = ConsoleColor.White) {
      this.id = id;
      this.x = x;
      this.y = y;
      this.view = view;
      this.color = color;
    }
    public void Move(Direction dir) {
      switch (dir) {
        case Direction.right: x++; break;
        case Direction.left: x--; break;
        case Direction.down: y++; break;
        case Direction.up: y--; break;
      }
    }
    public void Draw() {
      Console.ForegroundColor = color;
      Console.SetCursorPosition(x, y);
      Console.Write(view);
    }
    public void Erase() {
      Console.SetCursorPosition(x, y);
      Console.Write(new String(' ', view.Length));
    }
  }
}