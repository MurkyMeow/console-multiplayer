namespace ConsoleMultiplayer.Shared.Commands {
  [NetworkType(NetType.commandJoin)]
  class Join : NetworkEntity<Join> {
    [NetworkVar] public string sprite;

    public Join() {}
    public Join(string sprite) {
      this.sprite = sprite;
    }
  }
}