using System;
using System.Threading.Tasks;
using ConsoleMultiplayer.Client;
using ConsoleMultiplayer.Server;

namespace ConsoleMultiplayer {
  class Program {
    static async Task Main(string[] args) {
      Console.WriteLine("(h)ost or (c)onnect?");
      var answer = Console.ReadLine();
      var port = 5000;
      if (answer == "h") {
        var server = new GameServer();
        await server.Listen(port);
      } else if (answer == "c") {
        Console.CursorVisible = false;
        var client = new GameClient();
        Console.WriteLine("What would you like to look like?");
        string view = Console.ReadLine();
        await client.Connect("127.0.0.1", port, view);
        Console.Clear();
        await client.Run();
      }
    }
  }
}
