using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace SimpleHttp.Models;

public abstract class Server : IServer
{
    protected bool _isStarted;
    protected CancellationTokenSource _cts;
    private IServerSetting _setting;
    public bool IsStarted => _isStarted;

    public Server()
    {
        _cts = new CancellationTokenSource();
    }

    public IServerSetting Setting
    {
        get => _setting;
        set => _setting = value;
    }

    public Server(IServerSetting setting)
    {
        Setting = setting;
    }

    public virtual async Task Start()
    {
        if (!_isStarted)
        {
            _isStarted = true;
            this.Init();
        }
    }

    protected virtual Task Listen()
    {
        return Task.CompletedTask;
    }


    protected virtual Task Process(TcpListener context)
    {
        return Task.CompletedTask;
    }
    
    protected virtual Task Process()
    {
        return Task.CompletedTask;
    }



    public virtual async Task Stop()
    {
        _cts.Cancel();
        _isStarted = false;
    }

    public virtual async Task<bool> Reset()
    {
        throw new NotImplementedException();
    }

    protected virtual async Task Init()
    {
        try
        {
            this.Listen().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            throw;
        }
    }
}