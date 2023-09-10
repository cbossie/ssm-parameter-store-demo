using Constructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoInfrastructure.Constructs
{
    public class SsmParameterConstructProps : ConstructPropsBase
    {
        public string ParameterPath => "demo-infrastructure" + (string.IsNullOrWhiteSpace(EnvironmentPostFix) ? string.Empty : $"-{EnvironmentPostFix}");
    }
}
