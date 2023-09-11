using Amazon.SecretsManager.Extensions.Caching;
using Amazon.SecretsManager;
using param_web_app;
using param_web_app.Configuration;
using Amazon.Extensions.Configuration.SystemsManager;

var builder = WebApplication.CreateBuilder(args);

// Listen on port 8080
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(8080);
    serverOptions.ListenAnyIP(80);
});


// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

// Get base data for configuring SSM and Secrets Manager
DemoConfiguration demoConfig = new();
builder.Configuration.Bind("DemoConfig", demoConfig);

// If you are running this in AppRunner, the environmet will supplied via environment variable.
// Everything else is 
var envName = Environment.GetEnvironmentVariable("ENVIRONMENT_NAME");
if (!string.IsNullOrEmpty(envName))
{
    demoConfig.Environment = envName;
}

builder.Services.AddSingleton(demoConfig);

// Add Systems Manager Parameter Store parameters for the indicated path.
// They will expire after the indicated time span. These are things
// that could change at any time

// This parameter processor allows you to intercept calls to the SSM parameter store
var parameterProcessor = new DemoParameterProcessor();

// This is the long form to add systems manager
builder.Configuration.AddSystemsManager(configSource =>
{
    configSource.Path = $"/demo-infrastructure-{demoConfig.Environment}";
    configSource.ReloadAfter = TimeSpan.FromSeconds(demoConfig.SsmTimeToLive);
    configSource.ParameterProcessor = parameterProcessor;
});

// Add different path for "common" parameters, that don't need to expire.
// Think of this like, "company name" or "current century"
builder.Configuration.AddSystemsManager($"/demo-infrastructure-{demoConfig.Environment}/common");


// Add Secrets Manager caching client. This will cause secrets to expire after
// the indicated number of milliseconds
builder.Services.AddAWSService<IAmazonSecretsManager>();

// The cache hook object allows you to write code when you retrieve something from Secrets manager or when the cache is refreshed
builder.Services.AddSingleton<ISecretCacheHook, DemoSecretCacheHook>();
builder.Services.AddSingleton(s => 
{
    var smClient = s.GetService<IAmazonSecretsManager>();
    var cacheHook = s.GetService<ISecretCacheHook>();
    SecretsManagerCache cache = new(smClient, new SecretCacheConfiguration
    {
        CacheItemTTL = demoConfig.SecretsCacheExpiry,   
        CacheHook = cacheHook
        
    });
    return cache;
});

var app = builder.Build();

// Hack - Inject ILogger into the parameter processor
parameterProcessor.Logger = app.Services.GetService<ILogger<DemoParameterProcessor>>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    app.UseHsts();

}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
