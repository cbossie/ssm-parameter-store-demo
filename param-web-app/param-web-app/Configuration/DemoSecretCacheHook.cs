using Amazon.SecretsManager.Extensions.Caching;
using Amazon.SecretsManager.Model;

namespace param_web_app.Configuration;

public class DemoSecretCacheHook : ISecretCacheHook
{
    private ILogger Logger { get; }
    public DemoSecretCacheHook(ILogger<DemoSecretCacheHook> logger)
    {
        Logger = logger;
    }
    public object Get(object cachedObject)
    {
        Logger.LogInformation($"Get - {GetSecretMessage(cachedObject)}");
        return cachedObject;
    }

    public object Put(object o)
    {
        Logger.LogInformation($"Put - {GetSecretMessage(o)}");
        return o;
    }

    private string GetSecretMessage(object o) => o switch
    {
        DescribeSecretResponse dresponse => $"Secret Name: {dresponse.Name}",
        GetSecretValueResponse svresponse => $"Secret Name: {svresponse.Name}, Value: {svresponse.SecretString}",
        _ => $"Unknown Type Operation - {o.GetType().FullName}"
    };
}
