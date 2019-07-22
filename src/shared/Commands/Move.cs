namespace ConsoleMultiplayer.Shared.Commands {
  class Move : Command {
    [NetworkVar] public Direction dir;

    public Move() : base(CommandType.move) {}
    public Move(Direction dir) : base(CommandType.move) {
      this.dir = dir;
    }
  }
}