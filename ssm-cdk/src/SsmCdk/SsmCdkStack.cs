using Amazon.CDK;
using Constructs;
using Amazon.CDK.AWS.SSM;



namespace SsmCdk
{
    public class SsmCdkStack : Stack
    {
        internal SsmCdkStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            // The code that defines your stack goes here
            new Amazon.CDK.AWS.SSM.StringParameter(this, "param1", new StringParameterProps{
                ParameterName = "/demo-test/cityname",
                Type = ParameterType.STRING,
                DataType = ParameterDataType.TEXT,
                StringValue = "Houston"
            });

            new Amazon.CDK.AWS.SSM.StringParameter(this, "param2", new StringParameterProps{
                ParameterName = "/demo-test/statename",
                Type = ParameterType.STRING,
                DataType = ParameterDataType.TEXT,
                StringValue = "Texas"
            });

            new Amazon.CDK.AWS.SSM.StringParameter(this, "param3", new StringParameterProps{
                ParameterName = "/demo-test/languagename",
                Type = ParameterType.STRING,
                DataType = ParameterDataType.TEXT,
                StringValue = "C#"
            });


            new Amazon.CDK.AWS.SSM.StringParameter(this, "param4", new StringParameterProps{
                ParameterName = "/demo-test/planetname",
                Type = ParameterType.STRING,
                DataType = ParameterDataType.TEXT,
                StringValue = "Earth"
            });

        }
    }
}
