using System;
using System.Drawing;
using System.Linq;
using Autodesk.Revit.UI;
using RevitUtils.Native;

namespace RevitUtils.RibbonUtils
{
    public class SplitButtonControl
    {
        private readonly Bitmap image;
        private readonly Bitmap largeImage;
        private readonly RibbonItemNameConvention nameConvention;
        private readonly string name;
        private readonly Panel panel;
        private readonly string text;

        internal SplitButtonControl(Panel panel, string name, string text, Bitmap image, Bitmap largeImage, RibbonItemNameConvention nameConvention)
        {
            this.panel = panel;
            this.name = name;
            this.text = text;
            this.image = image;
            this.largeImage = largeImage;
            this.nameConvention = nameConvention;
        }

        public SplitButton GetSplitButton()
        {
            SplitButton splitButton = panel.Source.GetItems()
                .OfType<SplitButton>()
                .FirstOrDefault(x => x.Name == name);

            if (splitButton != null) return splitButton;

            var splitButtonData = new SplitButtonData(name, text)
                {
                    LargeImage = BitmapSourceConverter.ConvertFromImage(largeImage),
                    Image = BitmapSourceConverter.ConvertFromImage(image)
                };

            return panel.Source.AddItem(splitButtonData) as SplitButton;
        }

        public SplitButtonControl CreatePushButton<TExternalCommandClass>(
            string buttonName, string buttonText,
            Bitmap buttonImage, Bitmap buttonLargeImage,
            Type availabilityControlType = null,
            string longDescription = null,
            string helpUrl = null)
            where TExternalCommandClass : class, IExternalCommand
        {
            SplitButton splitButton = GetSplitButton();
            if (splitButton != null)
            {
                if (splitButton.GetItems().Any(x => x.Name == buttonName))
                    return this;

                Type commandClassType = typeof (TExternalCommandClass);
                var data = new PushButtonData(buttonName, buttonText, commandClassType.Assembly.Location, commandClassType.FullName)
                    {
                        Image = BitmapSourceConverter.ConvertFromImage(buttonImage),
                        LargeImage = BitmapSourceConverter.ConvertFromImage(buttonLargeImage)
                    };

                if (!string.IsNullOrEmpty(longDescription))
                    data.LongDescription = longDescription;

                if (!string.IsNullOrEmpty(helpUrl))
                    data.SetContextualHelp(new ContextualHelp(ContextualHelpType.Url, helpUrl));

                if (availabilityControlType != null)
                {
                    if (commandClassType.Assembly.Location != availabilityControlType.Assembly.Location)
                        throw new InvalidOperationException();

                    data.AvailabilityClassName = availabilityControlType.FullName;
                }

                splitButton.CurrentButton = splitButton.AddPushButton(data);
            }
            return this;
        }

        public SplitButtonControl CreatePushButton<TExternalCommandClass>(
            string buttonText,
            Bitmap buttonImage, Bitmap buttonLargeImage,
            Type availabilityControlType = null,
            string longDescription = null,
            string helpUrl = null)
            where TExternalCommandClass : class, IExternalCommand
        {
            if (nameConvention == null)
                throw new NameConventionNotSpecifiedException();

            var buttonName = nameConvention.GetRibbonItemName<TExternalCommandClass>();

            return CreatePushButton<TExternalCommandClass>(buttonName, buttonText, buttonImage, buttonLargeImage, availabilityControlType, longDescription, helpUrl);
        }

        public SplitButtonControl CreatePushButton<TExternalCommandClass, TExternalAvailabilityCommandClass>(
            string buttonText,
            Bitmap buttonImage, Bitmap buttonLargeImage,
            string longDescription = null,
            string helpUrl = null)
            where TExternalCommandClass : class, IExternalCommand
            where TExternalAvailabilityCommandClass : class, IExternalCommandAvailability
        {
            var availabilityType = typeof (TExternalAvailabilityCommandClass);

            return CreatePushButton<TExternalCommandClass>(buttonText, buttonImage, buttonLargeImage, availabilityType, longDescription, helpUrl);
        }

        public SplitButtonControl SetDefault(int index)
        {
            SplitButton splitButton = GetSplitButton();

            PushButton item = splitButton?.GetItems().Skip(index).FirstOrDefault();

            if (item != null)
                splitButton.CurrentButton = item;

            return this;
        }

        public SplitButtonControl SetDefault()
        {
            SplitButton splitButton = GetSplitButton();

            PushButton item = splitButton?.GetItems().LastOrDefault();

            if (item != null)
                splitButton.CurrentButton = item;

            return this;
        }

        internal void SetHelpUrl(string helpUrl)
        {
            if (!string.IsNullOrEmpty(helpUrl))
            {
                var button = GetSplitButton();

                button
                    .SetContextualHelp(new ContextualHelp(ContextualHelpType.Url, helpUrl));
            }
        }
    }
}