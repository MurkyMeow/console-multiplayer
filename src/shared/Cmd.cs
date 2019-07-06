using System.Net.Sockets;
using System.Threading.Tasks;
using ConsoleMultiplayer.Shared.Commands;

namespace ConsoleMultiplayer.Shared {
  class Cmd {
    UdpClient udp;

    public Cmd(UdpClient udp) {
      this.udp = udp;
    }
    public async Task Send(Command command) {
      var bytes = command.Encode();
      await udp.SendAsync(bytes, bytes.Length);
    }
  }
}