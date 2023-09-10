using System;
using Constructs;
using Amazon.CDK.AWS.ECR;
using Amazon.CDK.AWS.AppRunner;
using Repository = Amazon.CDK.AWS.ECR.Repository;
using AppRunner = Amazon.CDK.AWS.AppRunner.Alpha;
using Amazon.CDK.AWS.AppRunner.Alpha;
using Amazon.CDK.AWS.IAM;
using System.Linq;

namespace DemoInfrastructure;

public class AppRunnerServiceConstruct : DemoConstructBase
{
    public AppRunnerServiceConstruct(Construct scope, string id, AppRunnerConstructProps props) : base(scope, id)
    {

        // Create the ECR Repo for the AppRunner Service
        IRepository ecrRepository = Repository.FromRepositoryAttributes(this, "repo", new RepositoryAttributes
        {
            RepositoryName = props.EcrRepositoryName,
            RepositoryArn = GetEcrRepositoryArn(this, props.EcrRepositoryName)
        }) ;


        // ECR Repository
        CreateConstructOutput("ecrRepository", () => ecrRepository.RepositoryUri, "ECR Repository URI");

        // Secrets Manager Policy
        Policy ssmPolicy = new Policy(this, "ssmpolicy", new PolicyProps
        {
            PolicyName = $"{Aws.STACK_NAME}-{props.EnvironmentPostFix}-ssmpolicy"
        });

        // Create the AppRunner Service
        Service svc = new(this, "apprunnerservice", new AppRunner.ServiceProps() 
        {
            AutoDeploymentsEnabled = true,
            ServiceName = $"{StackName}",
            Source = new EcrSource(new EcrProps 
            {
                Repository = ecrRepository,
                ImageConfiguration = new ImageConfiguration
                {
                    Port = 8080
                }
            })
        });

        svc.AddEnvironmentVariable("ENVIRONMENT_NAME", props.EnvironmentPostFix);

        // Outputs
        CreateConstructOutput("apprunnerSvcOp", () => svc.ServiceUrl, "AppRunner Service URL");

        // Add SSM to instance role
        svc.AddToRolePolicy(new PolicyStatement(new PolicyStatementProps 
        {
            Sid = "SsmStatement",
            Effect = Effect.ALLOW,
            Resources = props.Parameters.Select(a => a.ParameterArn).ToArray(),
            Actions = new[] { "ssm:GetParameter" } 
        }));

        svc.AddToRolePolicy(new PolicyStatement(new PolicyStatementProps
        {
            Sid = "SsmStatementPath",
            Effect = Effect.ALLOW,
            Resources = new[] { $"arn:aws:ssm:{Aws.REGION}:{Aws.ACCOUNT_ID}:parameter/{props.ParameterPathPrefix}*" },
            Actions = new[] { "ssm:GetParametersByPath", "ssm:GetParameter" }
        }));

        // Add Secrets to instance role
        svc.AddToRolePolicy(new PolicyStatement(new PolicyStatementProps
        {
            Sid = "SecretsStatement",
            Effect = Effect.ALLOW,
            Resources = props.Secrets.Select(a => a.SecretArn).ToArray(),
            Actions = new[] { "secretsmanager:GetSecretValue", "secretsmanager:DescribeSecret" }
        }));

        // Grant acces to the the ECR Repo for App Runner Service
        ecrRepository.GrantPull(svc);
     }



    private string GetEcrRepositoryArn(Construct scope, string ecrRepositoryName) => Repository.ArnForLocalRepository(ecrRepositoryName, scope);
    

}
