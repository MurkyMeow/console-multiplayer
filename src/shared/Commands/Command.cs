using System.IO;

namespace ConsoleMultiplayer.Shared.Commands {
  enum CommandType {
    join,
    move,
  }
  abstract class Command : NetworkEntity {
    protected abstract void Serialize(BinaryWriter bw);
    public abstract CommandType type { get; }
    public override void Encode(BinaryWriter bw) {
      bw.Write((short)type);
      Serialize(bw);
    }
  }
}