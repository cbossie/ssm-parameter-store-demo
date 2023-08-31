# SSM Parameter and Secrets Manager Demo - ASP.NET Core
## Purpose
This repository shows a simple example of how to use AWS Systems Manager Parameter Store and AWS Secrets Manager in an ASP.NET Core 6.0 web app.

This repo involves two simple projects:
1. Directory: demo-infrastructure-cdk  
A CDK project that you can run to create the infrastructure needed to run this demo. It includes several AWS Systems Manager Parameter Store paramters, and several AWS Secrets Manager secrets.

1. Directory: param-web-app
An ASP.NET Core 6.0 web app that demonstrates two ways to incorporate these into your application.

## Deploying CDK
To deploy the CDK project, there are several [prerequisites that you need to follow](https://docs.aws.amazon.com/cdk/v2/guide/getting_started.html). This assumes that you have CDK installed, and have the appropriate credentials to deploy.

The project has three parameters that need to be supplied in order for this to run. They are:
- account
- region
- environment

You can supply these in several ways. If you are building and deploying locally, you can use a `cdk.context.json` file in the same directory that `cdk.json` resides (demo-infrastucture-cdk). When you run a `cdk synth` or `cdk deploy`, it will pick those values up automatically.

*Note: this file will be excluded by the .gitignore file so you won't check environment specific information into your source control*

The file will look something like
```json
{
    "account":"0000000000",
    "region":"us-east-2",
    "environmentName":"stg"
}
```

- **account** - the account you will be deploying to
- **region** - the region you will be deploying to
- **environmentName** - A postfix that will be included in all the resource names so that you can deploy this multiple times in the same AWS account. If you don't supply this, it will default to "dev".

If you want to deploy from the command line, you can also supply the context on the command line. For instance:

```bash
cdk deploy --context account=0000000000 --context environmentName=stg --context region=us-east-2
```

You can also supply environment variables for account and region if desired.

- account: CDK_DEPLOY_ACCOUNT or CDK_DEFAULT_ACCOUNT
- region: CDK_DEPLOY_REGION or CDK_DEFAULT_REGION

When you run "cdk deploy" you will need to ensure that you have permissions to deploy this stack to the specified account and region. You can always do this with the "--profile xxxx" command line switch, where xxxx is a credentials profile that has the correct permissions.

## Configuring the Web Application

The ASP.NET Core 6.0 Web Application includes functionality that will use the [.NET Core configuration provider for AWS Systems Manager](https://aws.amazon.com/blogs/developer/net-core-configuration-provider-for-aws-systems-manager/) and the [AWS Secrets Manager Caching client for .NET](https://github.com/aws/aws-secretsmanager-caching-net) in the same solution.

To configure the application you can modify the `appsettings.json` and the `appsetting.Development.json` file(s) to make sure ASP.NET Core is pointing at the right AWS resources. An example file (only the relevant part is shown). If you leave these values as defaults, they will match up with the default configuraton of the CDK project.

```json
"DemoConfig": {
    "environment": "stg",
    "ssmTimeToLive": 30,
    "secretsCacheExpiry": 30000
```
- **environment** - This should match the environmentName value in your CDK project.
- **ssmTimeToLive** - This is how long values will be kept in the SSM provider. After this many seconds, the values will be reloaded.
- **secretsCacheExpiry** - How many miliseconds that the secrets will remain in memory before being purged.

## Testing the Web Application ##

When you run the ASP.NET Core Web Application, you will need to ensure that the application runs with appropriate AWS credentials. If you are testing locally (which is ultimately the intention), ensure that you choose a local AWS profile with the right permissions.

To test the application, run it locally as you would normally with any ASP.NET Core Web App. E.g. ```dotnet run```, or click the "run" button in Visual Studio.

### Configure SSM Parameter Store
Configuring SSM parameter Store is simple in this case. In `Program.cs` you can use the appropriate extension method to add Systems Manager Parameter store as a configuration provider.

First, you get the configuration from `appsettings.json` and bind them to a POCO. This is just to make accessing those values neater.

```csharp
    builder.Configuration.Bind("DemoConfig", demoConfig);
    builder.Services.AddSingleton(demoConfig);
```

Then, you add two lines of code (comments not included) to include two separate paths. The first will expire all parameters after 30 seconds, while the second will hold the value for the lifetime of the application.

```csharp
    // Add Systems Manager Parameter Store parameters for the indicated path.
    // They will expire after the indicated time span. These are things
    // that could change at any time
    builder.Configuration.AddSystemsManager($"/demo-infrastructure-{demoConfig.Environment}",
TimeSpan.FromSeconds(demoConfig.SsmTimeToLive));

    // Add different path for "common" parameters, that don't need to expire.
    // Think of this like, "company name" or "current century"
    builder.Configuration.AddSystemsManager($"/demo-infrastructure-{demoConfig.Environment}-common");
```

### Test SSM Parameter Store
URL Path: /ParameterStore

This uses the file `param-web-app/param-web-app/Pages/ParameterStore.cshtml.cs`. What it does is very simple. It injects the Configuration provider into the constructor and binds it to a POCO configuration object to hold the values (SsmModel). Then in the `ParameterStore.cshtml` file, it simply iterates through the properties on the object and displays them on the screen.

```csharp
        ...
        public SsmModel SsmData{ get; set; } = new SsmModel();


        public ParameterStoreModel(IConfiguration config)
        {
            // Load up the configuration from Systems Manager
            config.Bind(SsmData);
        }
        ...
```

All of the parameters (except "SystemParameter") will refresh after the specified period. So, if you update one of them in the AWS console, refresh the page after 30 seconds (if you use the default value), and you will see that the value changes. Before that, however, the value will stay the same.

### Configure Secrets Manager
Configuring Secrets Manager Cache Client is also simple. In `Program.cs` you can create a singleton instance of the SecretsManagerCacheClient, and configure the TTL (30 seconds in our default example). It requires an instance of `IAmazonSecretsManager` (a Secrets Manager Client), so that also has to be injected. Then when you resolve the SecretsManagerCache for the first time, it will create the instance for you to use.

```csharp
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
```

As with Parameter Store, you also get the configuration from `appsettings.json` and bind them to a POCO.

### Test Secrets Manager
URL Path: /SecretsManager

This uses the file `param-web-app/param-web-app/Pages/SecretsManager.cshtml.cs`. What it does is very simple. First, it injecs the instance of `DemoConfiguration` and the `SecretsManagerCache` objects into the constructor.

```csharp
        ...
        public SecretsManagerModel(DemoConfiguration demoConfig, SecretsManagerCache smc)
        {
            SmCache = smc;
            DemoConfig = demoConfig;
        }
        ...
```

Then, in the `OnGet` method, it simply retrieves two secret values ("LaunchCode" and "SecretPlanLocation", of course) from the SecretsManager Cache Client. It populates these two values into string properties that members of the page model class.

```csharp
        public async Task OnGet()
        {
            LaunchCode = await SmCache.GetSecretString($"launchcode-secret-{DemoConfig.Environment}");
            SecretPlanLocation = await SmCache.GetSecretString($"secret-plan-location-secret-{DemoConfig.Environment}");
        }
```

When you load the page you will see the secret values displayed. To test, all you have to do is to change one of the values in the AWS console. Then once it has been 30 seconds (default) since you accessed that value, you will see the new value displayed.

### Considerations
For this behavior, there is a subtle difference in how they expire.
1. With Parameter Store, all of the values are retrieved every 30 seconds, and all are refreshed after 30 seconds.
2. With Secrets Manager, values are only accessed from AWS if requested, at which time the clock starts on that value.

Why is this important? Because this creates a very different access pattern between the two services. If you never access a secret, it will never be retrieved from Secrets manager, while the values will be retrieved from Parameter Store regardless of whether or not you ever use it.

That has implications. First, if you only infrequently access secrets, the first time you access it, will take a little bit longer, since it makes a call to AWS. Second, if there is any sort of network/credential issue, you will fail to retrieve the secret. However, if your average time between access for a secret is longer than the cache expiry time, then on average, you will have the latest value.

With parameter store, you will have guaranteed low access time, because all of the parameters will be prefetched. However, you will, on average, get a value that is halfway through its TTL time. That means if your parameters change frequently, and your application is sensitive to that, you will need to tune your application accordingly.



