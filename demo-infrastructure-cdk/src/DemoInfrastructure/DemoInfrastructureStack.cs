using Amazon.CDK;
using Constructs;
using Amazon.CDK.AWS.SSM;
using Amazon.CDK.AWS.SecretsManager;
using DemoInfrastructure;
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
            _ = new SsmParameterConstruct(this, "ssmParameters", new SsmParameterConstructProps
            {
                EnvironmentPostFix = EnvironmentPostfix
            });

            // Create Secrets Manager Values
            _ = new SecretsManagerConstruct(this, "secrets", new SecretsManagerConstructProps
            {
                EnvironmentPostFix = EnvironmentPostfix
            });
        }
    }
}
