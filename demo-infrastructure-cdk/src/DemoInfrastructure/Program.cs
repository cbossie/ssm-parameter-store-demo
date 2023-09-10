using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoInfrastructure
{
    sealed class Program
    {


        public static void Main(string[] args)
        {
            var app = new App();
            string ecrRepositoryName = app.Node.TryGetContext("ecrRepository")?.ToString();

            string environmentPostfix = $"{app.Node.TryGetContext("environmentName")}" ?? "dev";
            string stackPrefix = $"{app.Node.TryGetContext("stackPrefix")}" ?? "DemoInfrastructureStack";

            string stackName = $"{stackPrefix}-{environmentPostfix}";

            string account = $"{app.Node.TryGetContext("account")}";
            string region = $"{app.Node.TryGetContext("region")}";
            _ = new DemoInfrastructureStack(app, stackName, new DemoInfrastructureStackProps
            {
                CreateAppRunner = ecrRepositoryName != null,
                EcrRepository = ecrRepositoryName,
                StackName = stackName,
                EnvironmentPostfix = environmentPostfix,
                Env = MakeEnv(account, region)
            });
            app.Synth();
        }


        private static Amazon.CDK.Environment MakeEnv(string account = null, string region = null)
        {
            return new Amazon.CDK.Environment
            {
                Account = account ??
                    System.Environment.GetEnvironmentVariable("CDK_DEPLOY_ACCOUNT") ??
                    System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT"),
                Region = region ??
                    System.Environment.GetEnvironmentVariable("CDK_DEPLOY_REGION") ??
                    System.Environment.GetEnvironmentVariable("CDK_DEFAULT_REGION")
            };
        }
    }
}
