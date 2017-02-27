using System;
using System.Collections.Generic;
using Autodesk.Revit.UI;

namespace RevitUtils.RibbonUtils
{
    public class StackedItem : RibbonToolItem
    {
        private readonly RibbonItemNameConvention nameConvention;

        internal StackedItem(RibbonItemNameConvention nameConvention)
        {
            this.nameConvention = nameConvention;
            Buttons = new List<Button>(3);
        }

        public int ItemsCount => Buttons.Count;

        public IList<Button> Buttons { get; }

        public StackedItem CreateButton<TExternalCommandClass>(string name, string text)
            where TExternalCommandClass : class, IExternalCommand
        {
            Type commandClassType = typeof (TExternalCommandClass);

            return CreateButton(name, text, commandClassType, null);
        }

        public StackedItem CreateButton<TExternalCommandClass>(string name, string text, Action<Button> action)
            where TExternalCommandClass : class, IExternalCommand
        {
            Type commandClassType = typeof (TExternalCommandClass);

            return CreateButton(name, text, commandClassType, action);
        }

        public StackedItem CreateButton<TExternalCommandClass>(string text, Action<Button> action)
            where TExternalCommandClass : class, IExternalCommand
        {
            if (nameConvention == null)
                throw new NameConventionNotSpecifiedException();

            var name = nameConvention.GetRibbonItemName<TExternalCommandClass>();

            return CreateButton<TExternalCommandClass>(name, text, action);
        }

        public StackedItem CreateButton<TExternalCommandClass>(string text)
            where TExternalCommandClass : class, IExternalCommand
        {
            return CreateButton<TExternalCommandClass>(text, button => { });
        }

        public StackedItem CreateButton(string name, string text, Type externalCommandType)
        {
            return CreateButton(name, text, externalCommandType, null);
        }

        public StackedItem CreateButton(string name, string text, Type externalCommandType, Action<Button> action)
        {
            if (Buttons.Count == 3)
                throw new InvalidOperationException("You cannot create more than three items in the StackedItem");

            var button = new Button(name, text, externalCommandType);

            action?.Invoke(button);

            Buttons.Add(button);

            return this;
        }
    }
}