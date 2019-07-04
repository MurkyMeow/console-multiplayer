using System.IO;

namespace ConsoleMultiplayer.Shared.Commands {
  class Move : Command {
    public override CommandType type => CommandType.move;
    Direction dir;

    public Move(Direction dir) {
      this.dir = dir;
    }
    public Move(BinaryReader br) {
      dir = (Direction)br.ReadInt16();
    }
    public override void Encode(BinaryWriter bw) {
      bw.Write((short)dir);
    }
  }
}