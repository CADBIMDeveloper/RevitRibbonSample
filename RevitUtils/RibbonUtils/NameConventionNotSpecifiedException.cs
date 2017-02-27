using System;

namespace RevitUtils.RibbonUtils
{
    public class NameConventionNotSpecifiedException : InvalidOperationException
    {
        public NameConventionNotSpecifiedException()
            : base("Name convention was not specified!")
        {
            
        }
    }
}