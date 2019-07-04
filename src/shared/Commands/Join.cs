using System.IO;

namespace ConsoleMultiplayer.Shared.Commands {
  class Join : Command {
    public override CommandType type => CommandType.join;
    public string sprite;

    public Join(string sprite) {
      this.sprite = sprite;
    }
    public Join(BinaryReader br) {
      sprite = br.ReadString();
    }
    public override void Encode(BinaryWriter bw) {
      bw.Write(sprite);
    }
  }
}