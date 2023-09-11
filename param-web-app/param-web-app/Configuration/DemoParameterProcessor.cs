using Amazon.Extensions.Configuration.SystemsManager;
using Amazon.SimpleSystemsManagement.Model;
using System.Runtime.CompilerServices;

namespace param_web_app.Configuration;

public class DemoParameterProcessor : IParameterProcessor
{
    public ILogger? Logger { get; set; }

    public string GetKey(Parameter parameter, string path)
    {
        LogOperation(parameter, path);
        return parameter.Name.Split('/').LastOrDefault() ?? parameter.Name;
    }

    public string GetValue(Parameter parameter, string path)
    {
        LogOperation(parameter, path);
        return parameter.Value;
    }

    public bool IncludeParameter(string path)
    {
        return true;
    }

    public bool IncludeParameter(Parameter parameter, string path)
    {
        LogOperation(parameter, path);
        return true;
    }

    private void LogOperation(Parameter paramter, string path, [CallerMemberName] string operation = "na") => Logger?.LogInformation($"Op: {operation}, Parameter: ({paramter.Name},{paramter.Value}) [Path: {path}]");

}
