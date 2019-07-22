namespace ConsoleMultiplayer.Shared.Commands {
  class Join : Command {
    [NetworkVar] public string sprite;

    public Join() : base(CommandType.join) {}
    public Join(string sprite) : base(CommandType.join) {
      this.sprite = sprite;
    }
  }
}