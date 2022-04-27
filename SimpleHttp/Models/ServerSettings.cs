namespace SimpleHttp.Models;

public class ServerSettings : IServerSetting
{
    private int _port;
    private string _name;
    private string _rootDirectory;
    private string _uri;
    
    public int Port => _port;
    public string Name => _name;
    public string RootDirectory => AppDomain.CurrentDomain.BaseDirectory + "www/" + Name + "/";
    public string Uri  => "http://localhost:" + this.Port.ToString() + "/" + Name + "/";


    public ServerSettings(string name, int port)
    {
        _name = name;
        _port = port;
    }
    
}

