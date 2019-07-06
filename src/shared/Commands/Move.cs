using System.IO;

namespace ConsoleMultiplayer.Shared.Commands {
  class Move : Command {
    public override CommandType type => CommandType.move;
    public Direction dir;

    public Move(Direction dir) {
      this.dir = dir;
    }
    public Move(BinaryReader br) {
      dir = (Direction)br.ReadInt16();
    }
    protected override void Serialize(BinaryWriter bw) {
      bw.Write((short)dir);
    }
  }
}