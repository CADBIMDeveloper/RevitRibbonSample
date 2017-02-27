using System;
using System.Collections.Generic;
using Autodesk.Revit.UI;

namespace RevitUtils.RibbonUtils
{
    public class PulldownButton : Button
    {
        private readonly RibbonItemNameConvention nameConvention;

        internal PulldownButton(string name, string text, RibbonItemNameConvention nameConvention) :
            base(name, text, null)
        {
            this.nameConvention = nameConvention;
        }

        public int ItemsCount => Buttons.Count;

        public IList<Button> Buttons { get; } = new List<Button>();

        public PulldownButton CreateButton<TExternalCommandClass>(string name, string text)
            where TExternalCommandClass : class, IExternalCommand
        {
            Type commandClassType = typeof (TExternalCommandClass);

            return CreateButton(name, text, commandClassType, null);
        }

        public PulldownButton CreateButton<TExternalCommandClass>(string name, string text, Action<Button> action)
            where TExternalCommandClass : class, IExternalCommand
        {
            Type commandClassType = typeof (TExternalCommandClass);

            return CreateButton(name, text, commandClassType, action);
        }

        public PulldownButton CreateButton<TExternalCommandClass>(string text, Action<Button> action)
            where TExternalCommandClass : class, IExternalCommand
        {
            if (nameConvention == null)
                throw new NameConventionNotSpecifiedException();

            var name = nameConvention.GetRibbonItemName<TExternalCommandClass>();

            return CreateButton<TExternalCommandClass>(name, text, action);
        }

        public PulldownButton CreateButton<TExternalCommandClass>(string text)
            where TExternalCommandClass : class, IExternalCommand
        {
            return CreateButton<TExternalCommandClass>(text, x => { });
        }

        public PulldownButton CreateButton(string name, string text, Type externalCommandType)
        {
            return CreateButton(name, text, externalCommandType, null);
        }

        public PulldownButton CreateButton(string name, string text, Type externalCommandType, 
            Action<Button> action)
        {
            var button = new Button(name, text, externalCommandType);

            action?.Invoke(button);

            Buttons.Add(button);

            return this;
        }

        internal override ButtonData Finish()
        {
            var pulldownButtonData = new PulldownButtonData(Name, Text);

            if (LargeImage != null)
                pulldownButtonData.LargeImage = LargeImage;

            if (SmallImage != null)
                pulldownButtonData.Image = SmallImage;

            if (Description != null)
                pulldownButtonData.LongDescription = Description;

            if (ContextualHelp != null)
                pulldownButtonData.SetContextualHelp(ContextualHelp);

            return pulldownButtonData;
        }

        internal void BuildButtons(Autodesk.Revit.UI.PulldownButton pulldownButton)
        {
            foreach (Button button in Buttons)
                pulldownButton.AddPushButton(button.Finish() as PushButtonData);
        }
    }
}