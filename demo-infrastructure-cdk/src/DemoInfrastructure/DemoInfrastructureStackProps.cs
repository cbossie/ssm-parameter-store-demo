using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoInfrastructure
{
    public class DemoInfrastructureStackProps : StackProps
    {
        public bool CreateAppRunner { get; set; }
        public string EcrRepository { get; set; }
        public string EnvironmentPostfix { get; set; }
    }
}
