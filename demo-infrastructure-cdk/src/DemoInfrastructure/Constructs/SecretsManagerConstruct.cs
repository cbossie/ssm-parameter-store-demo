using Amazon.CDK.AWS.SecretsManager;
using Amazon.CDK;
using Constructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.CDK.AWS.SSM;

namespace DemoInfrastructure.Constructs
{
    public class SecretsManagerConstruct : Construct
    {
        public SecretsManagerConstruct(Construct scope, string id, SecretsManagerConstructProps props)
            : base(scope, id)
        {
            var secret1 = new Secret(this, "launchcode", new SecretProps
            {
                SecretName = $"launchcode-secret-{props.EnvironmentPostFix}",
                SecretStringValue = new SecretValue("12345"),
                Description = "Launch Code"
            });
            CreateSecretOutput(secret1.Node.Id, secret1, "Launch Code");


            var secret2 = new Secret(this, "secretplanlocation", new SecretProps
            {
                SecretName = $"secret-plan-location-secret-{props.EnvironmentPostFix}",
                SecretStringValue = new SecretValue("Basement of the Alamo"),
                Description = "Location of Secret Plan"
            });
            CreateSecretOutput(secret2.Node.Id, secret2, "Secret Plan Location");
        }

        private CfnOutput CreateSecretOutput(string id, Secret secret, string description = null) => new(this, $"{id}-secret", new CfnOutputProps
        {
            Value = secret.SecretName,
            Description = $"{description} Secrets Manager Secret"
        });

    }
}
