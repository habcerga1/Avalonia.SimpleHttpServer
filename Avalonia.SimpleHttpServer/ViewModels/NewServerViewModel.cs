using System;
using Avalonia.SimpleHttpServer.Models;

namespace Avalonia.SimpleHttpServer.ViewModels;

public class NewServerViewModel
{
    public BS<string> Name { get; set; }        // Server name
    public BS<string> Port { get; set; }        // Server port
    
    public BS<ServerSettingsDto> CreateBtn { get; set; }

    public event Action<ServerSettingsDto> ServerCreated; 

    public NewServerViewModel()
    {
        Name = new BS<string>();
        Port = new BS<string>();
        CreateBtn = new BS<ServerSettingsDto>(OnCommandParam);
    }
    
    private void OnCommandParam(ServerSettingsDto obj)
    {
        if (ServerCreated != null)
        {
            ServerCreated.Invoke(new ServerSettingsDto(Name.Value,Port.Value));
        }
    }
}