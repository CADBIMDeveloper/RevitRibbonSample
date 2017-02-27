using System;
using Autodesk.Revit.UI;

namespace RevitUtils.RibbonUtils
{
    public abstract class RibbonItemNameConvention
    {
        public string GetRibbonItemName<T>() where T : class, IExternalCommand
        {
            return GetRibbonItemName(typeof(T));
        }

        public abstract string GetRibbonItemName(Type type);
    }
}