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
    Player player;
    List<GameObject> objects = new List<GameObject>();

    public Task Connect(string host, int port, string view) {
      udp = new UdpClient();
      udp.Connect(host, port);
      var bytes = new Join(view).Encode();
      return udp.SendAsync(bytes, bytes.Length);
    }
    public Task Run() =>
      Task.WhenAll(new Task[] { Listen(), RunInputLoop() });
    async Task Listen() {
      while (true) {
        var res = await udp.ReceiveAsync();
        var br = new BinaryReader(new MemoryStream(res.Buffer));
        if (player == null) {
          player = NetEntity<Player>.Decode(br);
          Canvas.Draw(player);
        } else {
          var obj = NetEntity<GameObject>.Decode(br);
          var idx = objects.FindIndex(x => x.ID == obj.ID);
          if (idx == -1) {
            objects.Add(obj);
          } else {
            Canvas.Erase(objects[idx]);
            objects[idx] = obj;
          }
          Canvas.Draw(obj);
        }
      }
    }
    async Task RunInputLoop() {
      while (true) {
        var maybeDir = GetDir();
        if (maybeDir == null) continue;
        var dir = (Direction)maybeDir;
        Canvas.Erase(player);
        player.Move(dir);
        Canvas.Draw(player);
        var bytes = new Input(dir).Encode();
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