using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Amazon.CDK.AWS.SecretsManager;
using Amazon.CDK.AWS.SSM;

namespace DemoInfrastructure;

public class AppRunnerConstructProps : ConstructPropsBase
{
    public string EcrRepositoryName { get; set; }
    public IEnumerable<Secret> Secrets {get;set;}
    public IEnumerable<StringParameter> Parameters {get;set;}

    public string ParameterPathPrefix { get; set; }
}
