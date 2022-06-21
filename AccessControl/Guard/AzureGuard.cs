using System.Text;
using System.Text.Json;
using Swan;
using Swan.Formatters;

namespace AccessControl.Guard;

public class AccessAttempt
{
    public AccessAttempt(string id, byte[] image)
    {
        Id = id;
        Image = image;
    }
    [JsonProperty("id")]
    public string Id { get; }
    [JsonProperty("image")]
    public byte[] Image { get; }
}

public class AzureGuard : IGuard
{
    private readonly HttpClient _client = new();
    private readonly string _url;
    
    public AzureGuard(string domain, string code)
    {
        _url = $"{domain}/api/Guard?code={code}";
    }

    public bool Allows(string id, byte[] image)
    {
        var requestContent = new StringContent(new AccessAttempt(id, image).ToJson(), Encoding.UTF8, "application/json");
        var response = _client.PatchAsync(_url, requestContent);
        var result = JsonSerializer.Deserialize<bool>(response.Result.Content.ReadAsStream());

        return result;
    }
}