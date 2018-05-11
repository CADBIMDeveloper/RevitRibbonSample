using System.Windows.Media;
using Autodesk.Revit.UI;

namespace AI.RevitReinforcementDimensioner.RevitRibbonUtils.Components
{
    public class TextBox : RibbonToolItem
    {
        private readonly string name;
        private ImageSource textBoxImage;
        private string textBoxDescription;
        private string textBoxHelpUrl;

        public TextBox(string name)
        {
            this.name = name;
        }

        public TextBox SetImage(ImageSource image)
        {
            textBoxImage = image;

            return this;
        }

        public TextBox SetDescription(string description)
        {
            textBoxDescription = description;

            return this;
        }

        public TextBox SetHelpUrl(string url)
        {
            textBoxHelpUrl = url;

            return this;
        }
        
        internal override RibbonItemData Finish()
        {
            var textBoxData = new TextBoxData(name);

            if (textBoxImage != null)
                textBoxData.Image = textBoxImage;

            if (textBoxDescription != null)
                textBoxData.ToolTip = textBoxDescription;

            if (textBoxHelpUrl != null)
                textBoxData.SetContextualHelp(new ContextualHelp(ContextualHelpType.Url, textBoxHelpUrl));

            return textBoxData;
        }
    }
}