using DemoInfrastructure.Constructs;

namespace DemoInfrastructure
{
    public class DemoInfrastructureStack : Stack
    {
        public string EnvironmentPostfix { get; set; }

        public DemoInfrastructureStack(Construct scope, string id, DemoInfrastructureStackProps props = null) : base(scope, id, props)
        {
            EnvironmentPostfix = props.EnvironmentPostfix;
            // Create SSM Parameters
            var parameters = new SsmParameterConstruct(this, "ssmParameters", new SsmParameterConstructProps
            {
                EnvironmentPostFix = EnvironmentPostfix
            });

            // Create Secrets Manager Values
            var secrets = new SecretsManagerConstruct(this, "secrets", new SecretsManagerConstructProps
            {
                EnvironmentPostFix = EnvironmentPostfix
            });

            // Create AppRunnerService
            var appRunner = new AppRunnerServiceConstruct(this, "apprunner", new AppRunnerConstructProps
            {
                EcrRepositoryName = props.EcrRepository,
                EnvironmentPostFix = EnvironmentPostfix,
                Secrets = secrets,
                Parameters = parameters,
                ParameterPathPrefix = parameters.ParameterPath
            });

        }
    }
}
