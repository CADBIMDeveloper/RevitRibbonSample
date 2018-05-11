using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;

namespace AI.RevitReinforcementDimensioner.RevitRibbonUtils.Components
{
    public class ComboBoxTool : RibbonToolItem
    {
        private readonly string name;
        private readonly List<ComboBoxToolItem> items = new List<ComboBoxToolItem>();
        private EventHandler<ComboBoxCurrentChangedEventArgs> currentChangeHandler;

        public ComboBoxTool(string name)
        {
            this.name = name;
        }

        public ComboBoxTool OnCurrentChange(EventHandler<ComboBoxCurrentChangedEventArgs> handler)
        {
            currentChangeHandler = handler;

            return this;
        }

        public ComboBoxTool AddComboBoxItem(ComboBoxToolItem item)
        {
            items.Add(item);

            return this;
        }

        public ComboBoxTool AddComboBoxItems(IEnumerable<ComboBoxToolItem> comboBoxToolItems)
        {
            items.AddRange(comboBoxToolItems);

            return this;
        }

        internal override RibbonItemData Finish()
        {
            return new ComboBoxData(name);
        }

        internal override void DoPostProcessing(RibbonItem ribbonItem)
        {
            var combo = (ComboBox) ribbonItem;

            var members = items
                .Select(x => new ComboBoxMemberData(x.Name, x.Text) {Image = x.Image})
                .ToList();

            var comboBoxItems = combo.AddItems(members);

            var defaultItem = items.FirstOrDefault(x => x.IsDefault);

            if (defaultItem != null)
            {
                var index = items.IndexOf(defaultItem);

                combo.Current = comboBoxItems[index];
            }

            if (currentChangeHandler != null)
                combo.CurrentChanged += currentChangeHandler;
        }
    }
}