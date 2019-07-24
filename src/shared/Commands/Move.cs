namespace ConsoleMultiplayer.Shared.Commands {
  [NetworkType(NetType.commandMove)]
  class Move : NetworkEntity<Move> {
    [NetworkVar] public Direction dir;

    public Move() {}
    public Move(Direction dir) {
      this.dir = dir;
    }
  }
}