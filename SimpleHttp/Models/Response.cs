using System.Net.Sockets;
using System.Text;

namespace SimpleHttp.Models;

class Response
{
    public Task SendOK(Socket client)
    {
        return  Task.CompletedTask;
    }

    public async Task SendResponse(Socket client,string requestedFile)
    {
        string content = await File.ReadAllTextAsync(requestedFile);
        StringBuilder header = new StringBuilder ();
        header.AppendLine (@"HTTP/1.1 200 OK"); 
        header.AppendLine ($@"Content-Length: {content.Length}");
        header.AppendLine (@"Content-Type: text/html");
        header.AppendLine(@"");
        header.AppendLine(@"");
        header.AppendLine (@content);
        client.Send(Encoding.UTF8.GetBytes(header.ToString()));
        client.Close();
    }

    public Task SendNotFound(Socket client)
    {
        return  Task.CompletedTask;
    }

    public Task SendNotImplement(Socket client)
    {
        return  Task.CompletedTask;
    }
}