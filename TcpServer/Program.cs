namespace TcpServer
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    class TcpMultiClientServer
    {
        static async Task Main()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 81);
            server.Start();
            Console.WriteLine($"Server started on port {81}");

            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                Task.Run(() => HandleClientAsync(client)); // New task for each client
            }
        }

        static async Task HandleClientAsync(TcpClient client)
        {
            using (client)
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];

                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received from {client.Client.RemoteEndPoint}: {request}");

                string response = $"Hello, {client.Client.RemoteEndPoint}!";
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

                Console.WriteLine("Response sent.");
            }
        }
    }
}
