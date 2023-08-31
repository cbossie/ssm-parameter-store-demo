using Amazon.CDK;
using Amazon.CDK.AWS.SSM;
using Constructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoInfrastructure.Constructs
{
    public class SsmParameterConstruct : Construct
    {
        private string EnvironmentPostFix { get; set; }

        private string ParameterPath => "demo-infrastructure" + (string.IsNullOrWhiteSpace(EnvironmentPostFix) ? string.Empty : $"-{EnvironmentPostFix}");

        public SsmParameterConstruct(Construct scope, string id, SsmParameterConstructProps props) 
            : base(scope, id)
        {
            EnvironmentPostFix = props.EnvironmentPostFix;

            // Create Parameters
            var param1 = new StringParameter(this, "cityparam", new StringParameterProps
            {
                ParameterName = $"/{ParameterPath}/cityname",
                Type = ParameterType.STRING,
                DataType = ParameterDataType.TEXT,
                StringValue = "Houston"
            });
            CreateParameterOutput(param1.Node.Id, param1, "City Name");

            var param2 = new StringParameter(this, "stateparam", new StringParameterProps
            {
                ParameterName = $"/{ParameterPath}/statename",
                Type = ParameterType.STRING,
                DataType = ParameterDataType.TEXT,
                StringValue = "Texas"
            });
            CreateParameterOutput(param2.Node.Id, param2, "State");

            var param3 = new StringParameter(this, "languageparam", new StringParameterProps
            {
                ParameterName = $"/{ParameterPath}/languagename",
                Type = ParameterType.STRING,
                DataType = ParameterDataType.TEXT,
                StringValue = "C#"
            });
            CreateParameterOutput(param3.Node.Id, param3, "Language");

            var param4 = new StringParameter(this, "planetparam", new StringParameterProps
            {
                ParameterName = $"/{ParameterPath}/planetname",
                Type = ParameterType.STRING,
                DataType = ParameterDataType.TEXT,
                StringValue = "Earth"
            });
            CreateParameterOutput(param4.Node.Id, param4, "Planet");

            var param5 = new StringParameter(this, "commonsystemparam", new StringParameterProps
            {
                ParameterName = $"/{ParameterPath}-common/systemparameter",
                Type = ParameterType.STRING,
                DataType = ParameterDataType.TEXT,
                StringValue = "SampleValue"
            });
            CreateParameterOutput(param5.Node.Id, param5, "Common Parameter - System");
        }

        private CfnOutput CreateParameterOutput(string id, StringParameter ssmparam, string description = null) => new(this, $"{id}-ssm", new CfnOutputProps 
        {
            Value = ssmparam.ParameterName,
            Description = $"{description} SSM Parameter"
        });


    }
}
