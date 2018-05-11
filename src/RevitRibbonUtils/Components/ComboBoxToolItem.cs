using System.Windows.Media;

namespace AI.RevitReinforcementDimensioner.RevitRibbonUtils.Components
{
    public class ComboBoxToolItem
    {
        public ComboBoxToolItem(string name, string text, bool isDefault = false)
        {
            Name = name;
            Text = text;
            IsDefault = isDefault;
        }

        public string Name { get; }

        public string Text { get; }

        public bool IsDefault { get; }

        public ImageSource Image { get; private set; }

        public ComboBoxToolItem SetImage(ImageSource img)
        {
            Image = img;

            return this;
        }
    }
}