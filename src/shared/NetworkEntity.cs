using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace ConsoleMultiplayer.Shared {
  enum NetType {
    gameobj,
    commandJoin,
    commandMove,
  }
  class NetworkVar : Attribute {}
  class NetworkType : Attribute {
    public readonly NetType type;
    public NetworkType(NetType type) => this.type = type;
  }
  abstract class NetworkEntity<T> {
    delegate void Encoder(T obj, BinaryWriter bw);
    delegate void Decoder(T obj, BinaryReader br);

    static readonly NetType type;
    static readonly List<(Encoder, Decoder)> serializers = new List<(Encoder, Decoder)>();

    static NetworkEntity() {
      var t = typeof(T);
      type = (t.GetCustomAttribute(typeof(NetworkType)) as NetworkType).type;
      var fields = t.GetFields(
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
      );
      var netvar = typeof(NetworkVar);
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
      bw.Write((short)type);
      foreach (var (encode, _) in serializers) encode(obj, bw);
      return ms.ToArray();
    }
    public static T Decode(BinaryReader br) {
      if (br.BaseStream.Position == 0) { // the type havent been read yet
        br.ReadInt16();
      }
      var obj = (T)System.Activator.CreateInstance(typeof(T));
      foreach (var (_, decode) in serializers) decode(obj, br);
      return obj;
    }
  }
}