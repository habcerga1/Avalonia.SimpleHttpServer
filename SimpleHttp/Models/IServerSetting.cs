namespace SimpleHttp.Models;

public interface IServerSetting
{
    int Port { get;  }
    
    string Name { get; }
    string RootDirectory { get; }
    string Uri { get; }
    
}