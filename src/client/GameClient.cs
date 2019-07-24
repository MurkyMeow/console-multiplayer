using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using ConsoleMultiplayer.Shared;
using ConsoleMultiplayer.Shared.Commands;

namespace ConsoleMultiplayer.Client {
  class GameClient {
    UdpClient udp;
    GameObject player;
    Dictionary<int, GameObject> objects = new Dictionary<int, GameObject>();

    public Task Connect(string host, int port, string view) {
      udp = new UdpClient();
      udp.Connect(host, port);
      var bytes = NetEntity<Join>.Encode(new Join(view));
      return udp.SendAsync(bytes, bytes.Length);
    }
    public Task Run() =>
      Task.WhenAll(new Task[] { Listen(), RunInputLoop() });
    async Task Listen() {
      while (true) {
        var res = await udp.ReceiveAsync();
        var br = new BinaryReader(new MemoryStream(res.Buffer));
        var obj = NetEntity<GameObject>.Decode(br);
        if (player == null) {
          player = obj;
        } else if (objects.ContainsKey(obj.ID)) {
          objects[obj.ID].Erase();
          objects[obj.ID] = obj;
        } else {
          objects.Add(obj.ID, obj);
        }
        obj.Draw();
      }
    }
    async Task RunInputLoop() {
      while (true) {
        var maybeDir = GetDir();
        if (maybeDir == null) continue;
        var dir = (Direction)maybeDir;
        player.Erase();
        player.Move(dir);
        player.Draw();
        var bytes = NetEntity<Move>.Encode(new Move(dir));
        await udp.SendAsync(bytes, bytes.Length);
      }
    }
    Direction? GetDir() {
      switch (Console.ReadKey(true).Key) {
        case ConsoleKey.W: return Direction.up;
        case ConsoleKey.A: return Direction.left;
        case ConsoleKey.S: return Direction.down;
        case ConsoleKey.D: return Direction.right;
        default: return null;
      }
    }
  }
}