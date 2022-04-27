namespace SimpleHttp.Models;

public class HttpMiMeTypes
{
    private IDictionary<string,string> Values { get; set; }

    public HttpMiMeTypes()
    {
        Values = new Dictionary<string, string>();
        SetDefaultValues();
    }

    /// <summary>
    /// Set default values
    /// </summary>
    private void SetDefaultValues()
    {
        Values.Add(new KeyValuePair<string, string>(".html","text/html"));
    }

    /// <summary>
    /// Add mime type to collection
    /// </summary>
    /// <param name="key">mime name exmp: ".html"</param>
    /// <param name="value">mime value exmp: "text/html"</param>
    /// <returns></returns>
    public async Task<bool> AddValueAsync(string key,string value)
    {
        return Values.TryAdd(key, value) ? true : false;
    }

    /// <summary>
    /// Get mime types
    /// </summary>
    /// <returns>All mime types in collection</returns>
    public async Task<IDictionary<string, string>> GetValuesAsync()
    {
        return Values;
    }

    public bool IsContains(string key,out string value)
    {
        return Values.TryGetValue(key,out value);
    }
    
    public bool ContainsKey(string key)
    {
        return Values.ContainsKey(key);
    }
    
    public string? GetExtensions(string value)
    {
        return Values.FirstOrDefault(a=> a.Value == value).Key;
    }
    
}