namespace TcpClientApp
{
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    class TcpClientApp
    {
        static async Task Main()
        {
            try
            {
                using TcpClient client = new TcpClient("192.168.159.105", 81);
                NetworkStream stream = client.GetStream();

                string message = "Hello, Server!";
                byte[] data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine($"Server response: {response}");
            }
            catch
            {
                Console.WriteLine($"There was an error connecting to the server.");
            }
            
        }
    }
}
