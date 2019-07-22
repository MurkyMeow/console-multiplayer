using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace ConsoleMultiplayer.Shared {
  class NetworkVar : Attribute {}
  abstract class NetworkEntity<T> {
    delegate void Encoder(T obj, BinaryWriter bw);
    delegate void Decoder(T obj, BinaryReader br);

    static Dictionary<Type, List<(Encoder, Decoder)>> mapping =
      new Dictionary<Type, List<(Encoder, Decoder)>>();

    static List<(Encoder, Decoder)> Serializers {
      get {
        var currentType = typeof(T);
        if (mapping.ContainsKey(currentType)) {
          return mapping[currentType];
        }
        var fields = currentType.GetFields(
          BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        );
        var vartype = typeof(NetworkVar);
        var serializers = new List<(Encoder, Decoder)>();
        foreach (var field in fields) {
          if (field.GetCustomAttribute(vartype, true) == null) continue;
          var name = field.FieldType.IsEnum
            ? "Int16"
            : field.FieldType.Name;
          switch (name) {
            case "Int32":
              serializers.Add((
                (obj, bw) => bw.Write((int)field.GetValue(obj)),
                (obj, br) => field.SetValue(obj, br.ReadInt32())
              ));
              break;
            case "Int16":
              serializers.Add((
                (obj, bw) => bw.Write((short)field.GetValue(obj)),
                (obj, br) => field.SetValue(obj, br.ReadInt16())
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
        return mapping[currentType] = serializers;
      }
    }
    public static byte[] Encode(T obj) {
      var ms = new MemoryStream();
      var bw = new BinaryWriter(ms);
      foreach (var (encode, _) in Serializers) encode(obj, bw);
      return ms.ToArray();
    }
    public static T Decode(BinaryReader br) {
      var obj = (T)System.Activator.CreateInstance(typeof(T));
      foreach (var (_, decode) in Serializers) decode(obj, br);
      return obj;
    }
  }
}