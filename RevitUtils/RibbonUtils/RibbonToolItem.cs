using Autodesk.Revit.UI;

namespace RevitUtils.RibbonUtils
{
    public abstract class RibbonToolItem
    {
        internal RibbonItem RibbonItem { get; set; }
        
        public virtual bool IsVisible { get; set; }
    }
}