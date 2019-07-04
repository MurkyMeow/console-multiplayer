using System;
using System.IO;

namespace ConsoleMultiplayer.Shared.Commands {
  enum CommandType {
    join,
    move,
  }
  abstract class Command : NetworkEntity {
    public abstract CommandType type { get; }

    public static Command Parse(BinaryReader br) {
      var type = (CommandType)br.ReadInt16();
      switch (type) {
        case CommandType.join: return new Join(br);
        case CommandType.move: return new Move(br);
        default: throw new Exception($"Unknown command type: {type}");
      }
    }
  }
}