using System;
using System.Drawing;
using System.Windows.Media;
using AI.RevitReinforcementDimensioner.RevitRibbonUtils.Utils;
using Autodesk.Revit.UI;

namespace AI.RevitReinforcementDimensioner.RevitRibbonUtils.Components
{
    public class Button : RibbonToolItem
    {
        private readonly string assemblyLocation;
        private readonly string className;
        private string availabilityClassName;
        protected readonly string Name;
        protected readonly string Text;
        protected ContextualHelp ContextualHelp;
        protected string Description;
        protected ImageSource LargeImage;
        protected ImageSource SmallImage;

        public Button(string name, string text, Type externalCommandType)
        {
            Name = name;
            Text = text;

            if (externalCommandType == null) return;

            className = externalCommandType.FullName;
            assemblyLocation = externalCommandType.Assembly.Location;
        }

        public bool IsDefault { get; private set; }
        
        public Button SetLargeImage(ImageSource largeImage)
        {
            LargeImage = largeImage;
            return this;
        }

        public Button SetLargeImage(Bitmap largeImage)
        {
            LargeImage = BitmapSourceConverter.ConvertFromImage(largeImage);
            return this;
        }

        public Button SetSmallImage(ImageSource smallImage)
        {
            SmallImage = smallImage;
            return this;
        }

        public Button SetSmallImage(Bitmap smallImage)
        {
            SmallImage = BitmapSourceConverter.ConvertFromImage(smallImage);
            return this;
        }

        public Button SetAvailability<T>()
            where T : IExternalCommandAvailability
        {
            var availabilityCommandType = typeof (T);

            if (availabilityCommandType.Assembly.Location != assemblyLocation)
                throw new InvalidOperationException("Command and CommandAvailability classes must be located in the same assembly");

            availabilityClassName = availabilityCommandType.FullName;

            return this;
        }

        public Button SetLongDescription(string description)
        {
            Description = description;

            return this;
        }

        public Button SetContextualHelp(ContextualHelpType contextualHelpType, string helpPath)
        {
            ContextualHelp = new ContextualHelp(contextualHelpType, helpPath);

            return this;
        }

        public Button SetHelpUrl(string url)
        {
            ContextualHelp = new ContextualHelp(ContextualHelpType.Url, url);

            return this;
        }

        public Button SetDefault()
        {
            IsDefault = true;

            return this;
        }

        internal override RibbonItemData Finish()
        {
            var pushButtonData = new PushButtonData(Name, Text, assemblyLocation, className);

            if (LargeImage != null)
                pushButtonData.LargeImage = LargeImage;

            if (SmallImage != null)
                pushButtonData.Image = SmallImage;

            if (Description != null)
                pushButtonData.LongDescription = Description;

            if (ContextualHelp != null)
                pushButtonData.SetContextualHelp(ContextualHelp);

            if (!string.IsNullOrEmpty(availabilityClassName))
                pushButtonData.AvailabilityClassName = availabilityClassName;
            
            return pushButtonData;
        }
    }
}