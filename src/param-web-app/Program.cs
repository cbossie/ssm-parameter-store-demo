using Amazon.SecretsManager.Extensions.Caching;
using Amazon.SecretsManager;
using param_web_app;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

// Add Systems Manager Parameter Store, and refresh every 5 seconds
builder.Configuration.AddSystemsManager("/demo-test", TimeSpan.FromSeconds(5));

// Add Secrets Manager caching and refresh every 5 seconds
builder.Services.AddAWSService<IAmazonSecretsManager>();
builder.Services.AddSingleton(s => 
{
    var sm = s.GetService<IAmazonSecretsManager>();
    SecretsManagerCache cache = new(sm, new SecretCacheConfiguration
    {
        CacheItemTTL = 5000
    });
    return cache;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
