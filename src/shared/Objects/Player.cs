using System.Net;

namespace ConsoleMultiplayer.Shared {
  class Player : GameObject {
    public readonly IPEndPoint endPoint;

    public Player() {}
    public Player(IPEndPoint endPoint, int id, int x, int y, string view) : base(id, x, y, view) {
      this.endPoint = endPoint;
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
