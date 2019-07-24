namespace ConsoleMultiplayer.Shared.Commands {
  [NetHeader(Header.commandJoin)]
  class Join : NetEntity<Join> {
    [NetVar] public string sprite;

    public Join() {}
    public Join(string sprite) {
      this.sprite = sprite;
    }
  }
  [NetHeader(Header.commandMove)]
  class Move : NetEntity<Move> {
    [NetVar] public Direction dir;

    public Move() {}
    public Move(Direction dir) {
      this.dir = dir;
    }
  }
}