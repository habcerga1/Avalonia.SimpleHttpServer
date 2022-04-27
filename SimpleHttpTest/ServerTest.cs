using System.Net;
using System.Net.Sockets;
using System.Text;
using SimpleHttp.Models;

namespace SimpleHttpTest;


public class ServerTest
{
    private int port = 10100;
    private IServer _server;
    private HttpClient _client;
    private string requestUri = "http://127.0.0.1";
    
    [SetUp]
    public async Task Setup()
    {
        _client = new HttpClient();
        _server = new HttpServerV3(new ServerSettings("site1", port));
        await _server.Start();
    }
    
    [Test]
    public async Task ResponseStatusCode()
    {
        await Task.Delay(2000);
        
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:10100");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        var str = response.StatusCode.ToString();
        Assert.AreEqual(response.StatusCode.ToString(),"OK");
    }

    [Test]
    public async Task ResponseBodyAsync()
    {
        await Task.Delay(2000);
        
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:10100");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream responseStream = response.GetResponseStream();
        using (var reader = new StreamReader(responseStream))
        {
            string? buffer = await reader.ReadToEndAsync();
            Assert.IsNotEmpty(buffer);
        }
    }
}