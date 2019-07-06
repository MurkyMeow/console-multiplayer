using System;
using ConsoleMultiplayer.Client;
using ConsoleMultiplayer.Server;

namespace ConsoleMultiplayer {
  class Program {
    static void Main(string[] args) {
      Console.WriteLine("(h)ost or (c)onnect?");
      var answer = Console.ReadLine();
      var port = 5000;
      if (answer == "h") {
        var server = new GameServer();
        server.Listen(port).Wait();
      } else if (answer == "c") {
        var client = new GameClient();
        client.Connect("127.0.0.1", port);
        client.Run().Wait();
      }
    }
  }
}
