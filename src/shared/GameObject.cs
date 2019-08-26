using System;

namespace ConsoleMultiplayer.Shared {
  enum Direction {
    up, down, left, right,
  }
  [NetHeader(Header.commandJoin)]
  class GameObject : NetEntity<GameObject>, IDrawable {
    [NetVar] protected int id;
    [NetVar] protected int x;
    [NetVar] protected int y;
    [NetVar] protected string view;
    [NetVar] protected ConsoleColor color;

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
    public virtual bool Update() => true;
  }
}