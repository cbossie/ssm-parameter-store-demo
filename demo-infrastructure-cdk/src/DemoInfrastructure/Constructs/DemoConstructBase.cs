

namespace DemoInfrastructure;

public abstract class DemoConstructBase : Construct
{
    protected DemoConstructBase(Construct scope, string id) : base(scope, id)
    {
        StackName = Amazon.CDK.Aws.STACK_NAME;
    }

    protected string StackName { get; }

    protected CfnOutput CreateConstructOutput(string id, Func<string> valueExpression, string description) =>
          new (this, $"{id}-ssm", new CfnOutputProps
        {
            Value = valueExpression?.Invoke(),
            Description = description
        });

}
