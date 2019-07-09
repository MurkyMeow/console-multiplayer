using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using ConsoleMultiplayer.Shared;
using ConsoleMultiplayer.Shared.Commands;

namespace ConsoleMultiplayer.Server {
  using Players = Dictionary<IPEndPoint, GameObject>;
  class GameServer {
    UdpClient udp;

    Random rand = new Random();
    Players players = new Players();

    Task Send(IPEndPoint ep, byte[] data) =>
      udp.SendAsync(data, data.Length, ep);
    Task Broadcast(byte[] data) =>
      Task.WhenAll(players.Select(x => Send(x.Key, data)));

    public async Task Listen(int port) {
      udp = new UdpClient(port);
      while(true) {
        var res = await udp.ReceiveAsync();
        var br = new BinaryReader(new MemoryStream(res.Buffer));
        var type = (CommandType)br.ReadInt16();
        switch (type) {
          case CommandType.join: AddPlayer(new Join(br), res.RemoteEndPoint); break;
          case CommandType.move: MovePlayer(new Move(br), res.RemoteEndPoint); break;
        }
      }
    }
    void AddPlayer(Join join, IPEndPoint sender) {
      var newPlayer = new GameObject(
        id: players.Count,
        x: rand.Next(0, 10),
        y: rand.Next(0, 10),
        view: join.sprite
      );
      players.Add(sender, newPlayer);
      Broadcast(newPlayer.Encode());
      // Send existing players
      foreach (var (_, player) in players) {
        if (player != newPlayer) Send(sender, player.Encode());
      }
    }
    public void MovePlayer(Move move, IPEndPoint sender) {
      var player = players[sender];
      player.Move(move.dir);
      Broadcast(player.Encode());
    }
  }
}