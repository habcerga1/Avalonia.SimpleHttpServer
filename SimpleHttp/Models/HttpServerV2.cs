using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleHttp.Models;

public class HttpServerV2 : Server
{
    private readonly string[] _indexFiles = { 
        "index.html", 
        "index.htm", 
        "default.html", 
        "default.htm" 
    };
    
    private HttpListener _listener;
    private IServerSetting _setting;
    private HttpMiMeTypes _httpMiMeTypes;
    
    public HttpServerV2(IServerSetting setting) : base(setting)
    {
        _setting = setting;
        _listener = new HttpListener();
        _cts = new CancellationTokenSource();
    }

    public override Task Start()
    {
        return base.Start();
    }
    protected override Task Init()
    {
        return base.Init();
    }

    protected override Task Listen()
    {
        TcpListener listener = new TcpListener (IPAddress.Parse("127.0.0.1"),_setting.Port);
        while (!_cts.IsCancellationRequested)
        {
            try
            {
                this.Process(listener);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        return Task.CompletedTask;
    }

    protected override async Task Process(TcpListener context)
    {
        TcpClient client = context.AcceptTcpClient ();
        NetworkStream stream = client.GetStream ();

        byte[] sendBytes = await File.ReadAllBytesAsync(_setting.RootDirectory + "index.html");
        stream.WriteAsync(sendBytes, 0, sendBytes.Length);
        stream.Close ();
        client.Close ();
    }
    
    public static string  ToString (NetworkStream stream)
    {
        MemoryStream memoryStream = new MemoryStream ();
        byte[] data = new byte[256];
        int size;
        do {
            size = stream.Read (data, 0, data.Length);
            if (size == 0) {
                Console.WriteLine ("client disconnected...");
                Console.ReadLine ();
                return  null; 
            } 
            memoryStream.Write (data, 0, size);
        } while ( stream.DataAvailable); 
        return Encoding.UTF8.GetString (memoryStream.ToArray ());
    }
}