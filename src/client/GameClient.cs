using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ConsoleMultiplayer.Shared;
using ConsoleMultiplayer.Shared.Commands;

namespace ConsoleMultiplayer.Client {
  class GameClient {
    Cmd cmd;
    UdpClient udp;
    GameObject player;
    Dictionary<int, GameObject> objects = new Dictionary<int, GameObject>();

    public void Connect(string host, int port) {
      udp = new UdpClient();
      udp.Connect(host, port);
      cmd = new Cmd(udp);
    }
    public Task Run() =>
      Task.WhenAll(new Task[] { Listen(), RunInputLoop() });
    async Task Listen() {
      await cmd.Send(new Join("#"));
      while (true) {
        var res = await udp.ReceiveAsync();
        var br = new BinaryReader(new MemoryStream(res.Buffer));
        var obj = new GameObject(br);
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
        await cmd.Send(new Move(dir));
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