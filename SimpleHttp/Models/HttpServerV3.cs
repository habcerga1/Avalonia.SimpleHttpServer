using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleHttp.Models;

public class HttpServerV3 : Server
{
    private Socket server;
    
    public HttpServerV3(IServerSetting setting) : base(setting)
    {
        _cts = new CancellationTokenSource();
    }

    protected override Task Listen()
    {
        
        try
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), Setting.Port));
            server.Listen(100);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            throw;
        }

        Task.Run(() => Process());
        return Task.CompletedTask;
    }

    protected override Task Process()
    {
        while (!_cts.IsCancellationRequested)
        {
            try
            {
                Socket client = server.Accept();
                client.ReceiveTimeout = 10;
                client.SendTimeout = 10;

                Task.Run(() => HandleRequestAsync(client));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        return Task.CompletedTask;
    }
    
    protected async Task HandleRequestAsync(Socket clientSocket)
    {
        byte[] buffer = new byte[10240]; // 10 kb, just in case
        int receivedBCount = clientSocket.Receive(buffer); // Получаем запрос
        string strReceived = Encoding.UTF8.GetString(buffer, 0, receivedBCount);
 
        // Парсим запрос
        string httpMethod = strReceived.Substring(0, strReceived.IndexOf(" "));
 
        int start = strReceived.IndexOf(httpMethod) + httpMethod.Length + 1;
        int length = strReceived.LastIndexOf("HTTP") - start - 1;
        string requestedUrl = strReceived.Substring(start, length);

        string requestedFile = "";
        if (httpMethod.Equals("GET") || httpMethod.Equals("POST"))
            requestedFile = requestedUrl.Split('?')[0];
        else
        {
            new Response().SendNotImplement(clientSocket);
            return;
        }
        
        requestedFile = requestedFile.Replace("/", "/").Replace("\\..", ""); // Not to go back
        start = requestedFile.LastIndexOf('.') + 1;
        if (start > 0)
        {
            length = requestedFile.Length - start;
            string extension = requestedFile.Substring(start, length);
            if (new HttpMiMeTypes().ContainsKey("." + extension)) // Мы поддерживаем это расширение?
                if (File.Exists(Setting.RootDirectory + requestedFile)) // Если да
                    // ТО отсылаем запрашиваемы контент:
                    // TODO: тут просто отдаеться файл , нужно будет переделать
                    await new Response().SendResponse(clientSocket, Setting.RootDirectory + requestedFile);
                
                else
                    new Response().SendNotFound(clientSocket); // Мы не поддерживаем данный контент.
        }
        else
        {
            // Если файл не указан, пробуем послать index.html
            // Вы можете добавить больше(например "default.html")
            if (requestedFile.Substring(length - 1, 1) != "\\")
                requestedFile += "\\";
            if (File.Exists(Setting.RootDirectory + "index.html"))
                new Response().SendResponse(clientSocket, Setting.RootDirectory + "index.html").Start();
            else if (File.Exists(Setting.RootDirectory + "index.html"))
                new Response().SendResponse(clientSocket, Setting.RootDirectory + requestedFile);
            else
                new Response().SendOK(clientSocket);
        }
    }
}

