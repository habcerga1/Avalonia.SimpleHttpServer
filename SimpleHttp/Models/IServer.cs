using System.Net;

namespace SimpleHttp.Models;

public interface IServer
{
    public IServerSetting Setting { get; set; }
    public Task Start();  
    public Task Stop();
    public Task<bool> Reset();
}

