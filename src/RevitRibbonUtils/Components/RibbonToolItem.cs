using Autodesk.Revit.UI;

namespace AI.RevitReinforcementDimensioner.RevitRibbonUtils.Components
{
    public abstract class RibbonToolItem
    {
        public virtual bool IsVisible { get; set; }
        
        internal abstract RibbonItemData Finish();

        internal virtual void DoPostProcessing(RibbonItem ribbonItem)
        {
            
        }
    }
}