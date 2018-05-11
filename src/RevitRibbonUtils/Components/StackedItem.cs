using System;
using System.Collections.Generic;
using System.Linq;
using AI.RevitReinforcementDimensioner.RevitRibbonUtils.Conventions;
using Autodesk.Revit.UI;

namespace AI.RevitReinforcementDimensioner.RevitRibbonUtils.Components
{
    public class StackedItem : RibbonToolItem
    {
        private readonly RibbonItemNameConvention nameConvention;

        internal StackedItem(RibbonItemNameConvention nameConvention)
        {
            this.nameConvention = nameConvention;
            Buttons = new List<RibbonToolItem>();
        }

        public int ItemsCount => Buttons.Count;

        public IList<RibbonToolItem> Buttons { get; }

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

        public StackedItem CreateTextBox(string name, Action<TextBox> action)
        {
            var textBox = new TextBox(name);

            action?.Invoke(textBox);

            Buttons.Add(textBox);

            return this;
        }

        public StackedItem CreateComboBox(string name, Action<ComboBoxTool> action)
        {
            var comboBox = new ComboBoxTool(name);

            action?.Invoke(comboBox);

            Buttons.Add(comboBox);

            return this;
        }

        public StackedItem CreateButton(string name, string text, Type externalCommandType, Action<Button> action)
        {
            var button = new Button(name, text, externalCommandType);

            action?.Invoke(button);

            Buttons.Add(button);

            return this;
        }

        internal int GetDefaultButtonIndex()
        {
            return Buttons
                .Select((x, i) => new
                {
                    Index = i,
                    IsDefault = (x as Button)?.IsDefault ?? false
                })
                .FirstOrDefault(x => x.IsDefault)?.Index ?? Buttons.Count - 1;
        }

        internal override RibbonItemData Finish()
        {
            return null;
        }
    }
}