using System;

namespace AI.RevitReinforcementDimensioner.RevitRibbonUtils.Conventions
{
    public class NameConventionNotSpecifiedException : InvalidOperationException
    {
        public NameConventionNotSpecifiedException()
            : base("Name convention was not specified!")
        {
            
        }
    }
}