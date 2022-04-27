namespace Avalonia.SimpleHttpServer.Models;

public class ServerSettingsDto
{
    private string _name;
    private string _port;

    public string Name => _name;
    public string Port => _port;

    public ServerSettingsDto(string name,string port)
    {
        _name = name;
        _port = port;
    }
}