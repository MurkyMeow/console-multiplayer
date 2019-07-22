using System;

namespace ConsoleMultiplayer.Shared.Commands {
  enum CommandType {
    join,
    move,
  }
  abstract class Command : NetworkEntity<Command> {
    [NetworkVar] CommandType type;

    public Command(CommandType type) {
      this.type = type;
    }
    public byte[] Encode() {
      switch (type) {
        case CommandType.join: return NetworkEntity<Join>.Encode((Join)this);
        case CommandType.move: return NetworkEntity<Move>.Encode((Move)this);
        default: throw new Exception($"Unhandled command type: {type}");
      }
    }
  }
}