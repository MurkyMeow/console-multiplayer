using System;
using System.IO;

namespace ConsoleMultiplayer.Shared {
  enum Direction {
    up, down, left, right,
  }
  class GameObject : NetworkEntity {
    int id;
    int x;
    int y;
    string view;
    ConsoleColor color;

    public int ID => id;

    public GameObject(int id, int x, int y, string view, ConsoleColor color = ConsoleColor.White) {
      this.id = id;
      this.x = x;
      this.y = y;
      this.view = view;
      this.color = color;
    }
    public GameObject(BinaryReader br) {
      id = br.ReadInt32();
      x = br.ReadInt32();
      y = br.ReadInt32();
      view = br.ReadString();
      color = (ConsoleColor)br.ReadInt16();
    }
    public override void Encode(BinaryWriter bw) {
      bw.Write(id);
      bw.Write(x);
      bw.Write(y);
      bw.Write(view);
      bw.Write((short)color);
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