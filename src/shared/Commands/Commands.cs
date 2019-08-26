namespace ConsoleMultiplayer.Shared.Commands {
  [NetHeader(Header.commandJoin)]
  class Join : NetEntity<Join> {
    [NetVar] public readonly string sprite;

    public Join() {}
    public Join(string sprite) {
      this.sprite = sprite;
    }
  }
  [NetHeader(Header.input)]
  class Input : NetEntity<Input> {
    [NetVar] public readonly Direction dir;

    public Input() {}
    public Input(Direction dir) {
      this.dir = dir;
    }
  }
}