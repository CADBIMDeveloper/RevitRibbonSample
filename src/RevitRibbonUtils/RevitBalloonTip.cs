using Autodesk.Internal.InfoCenter;
using Autodesk.Windows;

namespace AI.RevitReinforcementDimensioner.RevitRibbonUtils
{
    public class RevitBalloonTip
    {
        public void Show(string category, string title, string text)
        {
            var balloonItem = new ResultItem
                {
                    Category = category,
                    Title = title,
                    TooltipText = text,
                    IsFavorite = false,
                    IsNew = false
                };

            ComponentManager.InfoCenterPaletteManager.ShowBalloon(balloonItem);
        }
    }
}