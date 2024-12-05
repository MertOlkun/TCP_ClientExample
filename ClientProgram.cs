using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

public class MyServer
{

    private static int port = 8080;
    private static TcpListener listener;
    private static Thread thread;
    private static int clientId = 0;
    private static string htmlContent =File.ReadAllText("index.html");


    private static void ClientThread(Object client)
    {
        NetworkStream netstream = ((TcpClient)client).GetStream();
        Console.WriteLine("Request made");
        clientId +=1;
        
        Byte[] hello = System.Text.Encoding.UTF8.GetBytes(clientId.ToString() + htmlContent);
        netstream.Write(hello,0,hello.Length);
        netstream.Close();
    }

    private static void Listen()
    {
        listener.Start();
        Console.WriteLine("Listening on: " + port.ToString());

        while (true)
        {
            Console.WriteLine("Waiting for connection....");
            Console.WriteLine("Client No: " + clientId);
            TcpClient client = listener.AcceptTcpClient();
            ThreadPool.QueueUserWorkItem(ClientThread, client);
        }
    }

    static void Main(string[] args)
    {

        listener = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), port);
        thread = new Thread(new ThreadStart(Listen));
        thread.Start();

        TcpClient tcpClient;
        NetworkStream stream = null;

        tcpClient = new TcpClient("127.0.0.1", port);
        Console.WriteLine("Connection was established....");

        stream = tcpClient.GetStream();
        Byte[] response = new Byte[tcpClient.ReceiveBufferSize];
        stream.Read(response, 0, (int)tcpClient.ReceiveBufferSize);

        String returnData = Encoding.UTF8.GetString(response);
        Console.WriteLine("Server Response " + returnData);

        tcpClient.Close();
        stream.Close();
    }
}

