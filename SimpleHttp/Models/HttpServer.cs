using System.Diagnostics;
using System.Net;

namespace SimpleHttp.Models;


public class HttpServer : Server
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
    
    public HttpServer(IServerSetting setting) : base(setting)
    {
    }
    
    public virtual async Task Start()
    {
        if (!_isStarted)
        {
            _isStarted = true;
            this.Init();
        }
    }

    private Task Listen()
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add(_setting.Uri);

        try
        {
            _listener.Start();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        while (true)
        {
            try
            {
                HttpListenerContext context = _listener.GetContext();
                this.Process(context);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
        return Task.CompletedTask;
    }
    

    private void Process(HttpListenerContext context)
    {
        string filename = WebUtility.UrlDecode(context.Request.Url.AbsolutePath.Substring(1));
        Console.WriteLine(filename);

        if (string.IsNullOrEmpty(filename))
        {
            foreach (string indexFile in _indexFiles)
            {
                if (File.Exists(Path.Combine(_setting.RootDirectory, indexFile)))
                {
                    filename = indexFile;
                    break;
                }
            }
        }

        filename = Path.Combine(_setting.RootDirectory, filename);

        if (File.Exists(filename))
        {
            try
            {
                Stream input = new FileStream(filename, FileMode.Open);
                // Check is MIME exist and then set else set "application/octet-stream";
                context.Response.ContentType = _httpMiMeTypes.IsContains(Path.GetExtension(filename), out string mime) ? mime : "application/octet-stream";
                context.Response.ContentLength64 = input.Length;

                if (context.Request.HttpMethod != "HEAD")
                {
                    byte[] buffer = new byte[1024 * 16];
                    int nbytes;
                    while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        context.Response.SendChunked = input.Length > 1024 * 16;
                        context.Response.OutputStream.Write(buffer, 0, nbytes);
                    }
                }
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.OutputStream.Flush();
            }
            catch
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }

        context.Response.OutputStream.Close();
    }
    

    public override async Task Stop()
    {
        base._cts.Cancel();
        _listener.Stop();
        base._isStarted = false;
    }

    public override async Task<bool> Reset()
    {
        throw new NotImplementedException();
    }

    private async Task Init()
    {
        try
        {
            this.Listen();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            throw;
        }
    }
}