using Amazon.CDK;
using Amazon.CDK.AWS.SSM;
using Constructs;
namespace DemoInfrastructure.Constructs;

public class SsmParameterConstruct : DemoConstructBase, IEnumerable<StringParameter>
{
    private List<StringParameter> Parameters {get;} = new();
    private string EnvironmentPostFix { get; set; }

    public string ParameterPath { get; }
    public SsmParameterConstruct(Construct scope, string id, SsmParameterConstructProps props) 
        : base(scope, id)
    {
        EnvironmentPostFix = props.EnvironmentPostFix;
        ParameterPath = props.ParameterPath;
        // Create Parameters
        var param1 = new StringParameter(this, "cityparam", new StringParameterProps
        {
            ParameterName = $"/{ParameterPath}/cityname",
            DataType = ParameterDataType.TEXT,
            StringValue = "Houston"
        });
        CreateConstructOutput(param1.Node.Id, () => param1.ParameterName, "City Parameter Name");
        Parameters.Add(param1);

        var param2 = new StringParameter(this, "stateparam", new StringParameterProps
        {
            ParameterName = $"/{ParameterPath}/statename",
            DataType = ParameterDataType.TEXT,
            StringValue = "Texas"
        });
        CreateConstructOutput(param2.Node.Id, () => param2.ParameterName, "State Parameter Name");
        Parameters.Add(param2);

        var param3 = new StringParameter(this, "languageparam", new StringParameterProps
        {
            ParameterName = $"/{ParameterPath}/languagename",
            DataType = ParameterDataType.TEXT,
            StringValue = "C#"
        });
        CreateConstructOutput(param3.Node.Id, () => param3.ParameterName, "Language Parameter Name");
        Parameters.Add(param3);

        var param4 = new StringParameter(this, "planetparam", new StringParameterProps
        {
            ParameterName = $"/{ParameterPath}/planetname",
            DataType = ParameterDataType.TEXT,
            StringValue = "Earth"
        });
        CreateConstructOutput(param4.Node.Id, () => param4.ParameterName, "Planet Parameter Name");
        Parameters.Add(param4);

        var param5 = new StringParameter(this, "commonsystemparam", new StringParameterProps
        {
            ParameterName = $"/{ParameterPath}-common/systemparameter",
            DataType = ParameterDataType.TEXT,
            StringValue = "SampleValue"
        });
        CreateConstructOutput(param5.Node.Id, () => param5.ParameterName, "Common Parameter Name");
        Parameters.Add(param5);
    }



    public IEnumerator<StringParameter> GetEnumerator() => Parameters.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Parameters.GetEnumerator();
}
