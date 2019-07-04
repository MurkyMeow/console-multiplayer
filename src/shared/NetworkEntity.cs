using System.IO;

namespace ConsoleMultiplayer.Shared {
  abstract class NetworkEntity {
    public abstract void Encode(BinaryWriter bw);

    public byte[] Encode() {
      var ms = new MemoryStream();
      var bw = new BinaryWriter(ms);
      Encode(bw);
      return ms.ToArray();
    }
  }
}