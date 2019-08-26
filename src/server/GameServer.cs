using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ConsoleMultiplayer.Shared;
using ConsoleMultiplayer.Shared.Commands;

namespace ConsoleMultiplayer.Server {
  class GameServer {
    UdpClient udp;

    Random rand = new Random();
    List<Player> players = new List<Player>();
    List<GameObject> objects = new List<GameObject>();

    Task Send(IPEndPoint ep, byte[] data) =>
      udp.SendAsync(data, data.Length, ep);
    Task Broadcast(byte[] data) =>
      Task.WhenAll(players.Select(plr => Send(plr.endPoint, data)));
    void Tick(object state) {
      var ms = new MemoryStream();
      var bw = new BinaryWriter(ms);
      foreach (var obj in objects.Concat(players)) {
        if (obj.Update()) bw.Write(obj.Encode());
      }
      if (ms.Position > 0) Broadcast(ms.ToArray());
    }
    public async Task Listen(int port, int tickrate) {
      this.udp = new UdpClient(port);
      new Timer(Tick, null, 0, 1000 / tickrate);
      while (true) {
        var res = await udp.ReceiveAsync();
        var br = new BinaryReader(new MemoryStream(res.Buffer));
        var header = (Header)br.ReadByte();
        switch (header) {
          case Header.commandJoin:
            AddPlayer(NetEntity<Join>.Decode(br), res.RemoteEndPoint);
            break;
          case Header.input:
            MovePlayer(NetEntity<Input>.Decode(br), res.RemoteEndPoint);
            break;
          default: throw new Exception($"Unsupported header: {header}");
        }
      }
    }
    void AddPlayer(Join join, IPEndPoint endPoint) {
      var newPlayer = new Player(
        endPoint,
        id: players.Count,
        x: rand.Next(0, 10),
        y: rand.Next(0, 10),
        view: join.sprite
      );
      players.Add(newPlayer);
      Broadcast(newPlayer.Encode());
      // Send existing players
      foreach (var player in players) {
        if (player != newPlayer) {
          Send(endPoint, player.Encode());
        }
      }
    }
    public void MovePlayer(Input move, IPEndPoint endPoint) {
      var player = players.Find(x => x.endPoint.Port == endPoint.Port);
      player.Move(move.dir);
    }
  }
}