using Amazon.SecretsManager.Extensions.Caching;
using Amazon.SecretsManager;
using param_web_app;
using param_web_app.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

// Get base data for configuring SSM and Secrets Manager
DemoConfiguration demoConfig = new();
builder.Configuration.Bind("DemoConfig", demoConfig);
builder.Services.AddSingleton(demoConfig);

// Add Systems Manager Parameter Store parameters for the indicated path.
// They will expire after the indicated time span. These are things
// that could change at any time
builder.Configuration.AddSystemsManager($"/{demoConfig.SsmPath}", TimeSpan.FromSeconds(demoConfig.SsmTimeToLive));

// Add different path for "common" parameters, that don't need to expire.
// Think of this like, "company name" or "current century"
builder.Configuration.AddSystemsManager($"/{demoConfig.SsmPath}-common");


// Add Secrets Manager caching client. This will cause secrets to expire after
// the indicated number of milliseconds
builder.Services.AddAWSService<IAmazonSecretsManager>();
builder.Services.AddSingleton(s => 
{
    var smClient = s.GetService<IAmazonSecretsManager>();
    SecretsManagerCache cache = new(smClient, new SecretCacheConfiguration
    {
        CacheItemTTL = demoConfig.SecretsCacheExpiry,
                
    });
    return cache;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
