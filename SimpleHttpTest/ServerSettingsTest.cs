using SimpleHttp.Models;

namespace SimpleHttpTest;

public class ServerSettingsTest
{
    private ServerSettings _settings;
    
    [SetUp]
    public void Setup()
    {
        _settings = new ServerSettings("site1", 10001);
    }

    [Test]
    public async Task ReadFromIndexFile()
    {
        Assert.IsNotNull(await File.ReadAllTextAsync(_settings.RootDirectory + "index.html"));
    }
    
    [Test]
    public async Task IsFileExist()
    {
        Assert.IsTrue(File.Exists(_settings.RootDirectory + "index.html"));
    }
    
    [Test]
    public void CheckUriPath()
    {
        Assert.AreEqual("http://localhost:10001/site1/",_settings.Uri);
    }
}