 using Amazon.CDK.AWS.SecretsManager;
using Amazon.CDK;
using Constructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.CDK.AWS.SSM;
using System.Collections;

namespace DemoInfrastructure.Constructs;

public class SecretsManagerConstruct : DemoConstructBase, IEnumerable<Secret>
{

    private List<Secret> Secrets {get;} = new ();
    
    public SecretsManagerConstruct(Construct scope, string id, SecretsManagerConstructProps props)
        : base(scope, id)
    {
        var secret1 = new Secret(this, "launchcode", new SecretProps
        {
            SecretName = $"launchcode-secret-{props.EnvironmentPostFix}",
            SecretStringValue = new SecretValue("12345"),
            Description = "Launch Code"
        });
        CreateConstructOutput(secret1.Node.Id, () => secret1.SecretName, "Launch Code Secret");
        Secrets.Add(secret1);

        var secret2 = new Secret(this, "secretplanlocation", new SecretProps
        {
            SecretName = $"secret-plan-location-secret-{props.EnvironmentPostFix}",
            SecretStringValue = new SecretValue("Basement of the Alamo"),
            Description = "Location of Secret Plan"
        });
        CreateConstructOutput(secret2.Node.Id, () => secret2.SecretName, "Secret Plan Location Secret");
        Secrets.Add(secret2);
    }

    public IEnumerator<Secret> GetEnumerator() => Secrets.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Secrets.GetEnumerator();
}
