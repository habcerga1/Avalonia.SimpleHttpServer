using System;
using Avalonia.SimpleHttpServer.Models;
using SimpleHttp.Models;

namespace Avalonia.SimpleHttpServer.ViewModels;

public class ServerViewModel
{
    public BS<string> Name { get; set; }                      // Server Name
    public BS<string> Port { get; set; }                      // Server Port
    
    public BS<string> Status { get; set; }                    // Is running
    
    public bool IsRunning { get; private set; }               // Is running

    private IServer _server;                                 // Server core
    
    public ServerViewModel(string name,string port)
    {
        Name = new BS<string>();
        Port = new BS<string>();
        Status = new BS<string>();
        Name.Value = name;
        Port.Value = port;
        _server = new HttpServerV3(new ServerSettings(name,Int32.Parse(port)));
        IsRunning = false;
    }

    public void StartServer()
    {
        Status.Value = "Running";
        IsRunning = true;
        _server.Start();
    }
    
    public void StopServer()
    {
        Status.Value = "Stopped";
        IsRunning = false;
        _server.Stop();
    }
    
    
    
}