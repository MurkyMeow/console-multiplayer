using System;

namespace ConsoleMultiplayer.Shared {
  enum Direction {
    up, down, left, right,
  }
  [NetHeader(Header.commandJoin)]
  class GameObject : NetEntity<GameObject>, IDrawable {
    [NetVar] int id;
    [NetVar] int x;
    [NetVar] int y;
    [NetVar] string view;
    [NetVar] ConsoleColor color;

    public int ID => id;
    public (int, int) Pos => (x, y);
    public string[] Template => view.Split("\\n");
    public ConsoleColor Color => color;

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
  }
}