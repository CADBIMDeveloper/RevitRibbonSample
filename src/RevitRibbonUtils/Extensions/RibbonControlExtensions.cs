using System.Linq;
using Autodesk.Windows;

namespace AI.RevitReinforcementDimensioner.RevitRibbonUtils.Extensions
{
    internal static class RibbonControlExtensions
    {
        public static RibbonTab FindTabByTitle(this RibbonControl ribbonControl, string title)
        {
            return ribbonControl.Tabs.FirstOrDefault(x => x.Title == title);
        }
    }
}