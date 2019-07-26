using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace ConsoleMultiplayer.Shared {
  enum Header : byte {
    gameobj,
    commandJoin,
    commandMove,
  }
  class NetVar : Attribute {}
  class NetHeader : Attribute {
    public readonly Header header;
    public NetHeader(Header header) => this.header = header;
  }
  abstract class NetEntity<T> {
    delegate void Encoder(T obj, BinaryWriter bw);
    delegate void Decoder(T obj, BinaryReader br);

    static readonly Header header;
    static readonly List<(Encoder, Decoder)> serializers = new List<(Encoder, Decoder)>();

    static NetEntity() {
      var t = typeof(T);
      header = (t.GetCustomAttribute(typeof(NetHeader)) as NetHeader).header;
      var fields = t.GetFields(
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
      );
      var netvar = typeof(NetVar);
      foreach (var field in fields) {
        if (field.GetCustomAttribute(netvar, true) == null) continue;
        var name = field.FieldType.IsEnum
          ? "Int32"
          : field.FieldType.Name;
        switch (name) {
          case "Int32":
            serializers.Add((
              (obj, bw) => bw.Write((int)field.GetValue(obj)),
              (obj, br) => field.SetValue(obj, br.ReadInt32())
            ));
            break;
          case "String":
            serializers.Add((
              (obj, bw) => bw.Write((string)field.GetValue(obj)),
              (obj, br) => field.SetValue(obj, br.ReadString())
            ));
            break;
          default: throw new Exception($"Unsupported type: {name}");
        }
      }
    }
    public static byte[] Encode(T obj) {
      var ms = new MemoryStream();
      var bw = new BinaryWriter(ms);
      bw.Write((byte)header);
      foreach (var (encode, _) in serializers) encode(obj, bw);
      return ms.ToArray();
    }
    public static T Decode(BinaryReader br) {
      br.BaseStream.Position = 1; // skip the 1-byte header
      var obj = (T)System.Activator.CreateInstance(typeof(T));
      foreach (var (_, decode) in serializers) decode(obj, br);
      return obj;
    }
  }
}