using System.Linq;
using Autodesk.Revit.UI;

namespace AI.RevitReinforcementDimensioner.RevitRibbonUtils.Components
{
    public class SplitButtonControl
    {
        private readonly string name;
        private readonly Panel panel;
        private readonly string text;

        internal SplitButtonControl(Panel panel, string name, string text)
        {
            this.panel = panel;
            this.name = name;
            this.text = text;
        }

        public SplitButton Finish()
        {
            var splitButton = panel.Source.GetItems()
                .OfType<SplitButton>()
                .FirstOrDefault(x => x.Name == name);

            if (splitButton != null) return splitButton;

            var splitButtonData = new SplitButtonData(name, text);

            return panel.Source.AddItem(splitButtonData) as SplitButton;
        }
    }
}