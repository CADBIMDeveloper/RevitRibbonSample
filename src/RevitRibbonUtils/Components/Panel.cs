using System;
using System.Collections.Generic;
using System.Linq;
using AI.RevitReinforcementDimensioner.RevitRibbonUtils.Conventions;
using Autodesk.Revit.UI;

namespace AI.RevitReinforcementDimensioner.RevitRibbonUtils.Components
{
    public class Panel : RibbonToolItem
    {
        private readonly RibbonItemNameConvention nameConvention;

        internal Panel(Tab tab, RibbonPanel panel, RibbonItemNameConvention nameConvention)
        {
            Tab = tab;
            Source = panel;
            this.nameConvention = nameConvention;
        }

        public override bool IsVisible
        {
            get { return Source.Visible; }
            set { Source.Visible = value; }
        }

        internal override RibbonItemData Finish()
        {
            return null;
        }

        internal RibbonPanel Source { get; }

        internal Tab Tab { get; }

        public string Title => Source.Title;

        public string Name => Source.Name;

        /// <summary>
        /// Create new Stacked items at the panel
        /// </summary>
        /// <param name="action">Action where you must add items to the stacked panel</param>
        /// <returns>Panel where stacked items was created</returns>
        public Panel CreateStackedItems(Action<StackedItem> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            var stackedItem = new StackedItem(nameConvention);

            action.Invoke(stackedItem);

            if (stackedItem.ItemsCount < 2 || stackedItem.ItemsCount > 3)
                throw new InvalidOperationException("You must create 2 or three items in the StackedItems");

            RibbonItemData item1 = stackedItem.Buttons[0].Finish();
            RibbonItemData item2 = stackedItem.Buttons[1].Finish();

            IList<RibbonItem> ribbonItems;

            if (stackedItem.ItemsCount == 3)
            {
                RibbonItemData item3 = stackedItem.Buttons[2].Finish();
                ribbonItems = Source.AddStackedItems(item1, item2, item3);
            }
            else
                ribbonItems = Source.AddStackedItems(item1, item2);

            for (var i = 0; i < stackedItem.Buttons.Count; ++i)
                stackedItem.Buttons[i].DoPostProcessing(ribbonItems[i]);

            return this;
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <returns>Panel where button was created</returns>
        public Panel CreateButton<TExternalCommandClass>(string name, string text)
            where TExternalCommandClass : class, IExternalCommand
        {
            return CreateButton<TExternalCommandClass>(name, text, null);
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button was created</returns>
        public Panel CreateButton<TExternalCommandClass>(string name, string text, Action<Button> action)
            where TExternalCommandClass : class, IExternalCommand
        {
            Type commandClassType = typeof (TExternalCommandClass);

            return CreateButton(name, text, commandClassType, action);
        }

        /// <summary>
        /// Create push button on the panel using naming conventions
        /// </summary>
        /// <typeparam name="TExternalCommandClass"></typeparam>
        /// <param name="text">Text user will see</param>
        /// <returns>Panel where button was created</returns>
        public Panel CreateButton<TExternalCommandClass>(string text)
            where TExternalCommandClass : class, IExternalCommand
        {
            return CreateButton<TExternalCommandClass>(text, button => { });
        }

        /// <summary>
        /// Create push button on the panel using naming conventions
        /// </summary>
        /// <typeparam name="TExternalCommandClass"></typeparam>
        /// <param name="text"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Panel CreateButton<TExternalCommandClass>(string text, Action<Button> action)
            where TExternalCommandClass : class, IExternalCommand
        {
            if (nameConvention == null)
                throw new NameConventionNotSpecifiedException();

            var name = nameConvention.GetRibbonItemName<TExternalCommandClass>();

            return CreateButton<TExternalCommandClass>(name, text, action);
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="externalCommandType">Class which implements IExternalCommand interface. 
        /// This command will be execute when user push the button</param>
        /// <returns>Panel where button were created</returns>
        public Panel CreateButton(string name, string text, Type externalCommandType)
        {
            return CreateButton(name, text, externalCommandType, null);
        }

        /// <summary>
        /// Create push button on the panel
        /// </summary>
        /// <param name="name">Internal name of the button</param>
        /// <param name="text">Text user will see</param>
        /// <param name="externalCommandType">Class which implements IExternalCommand interface. 
        /// This command will be execute when user push the button</param>
        /// <param name="action">Additional action with whe button</param>
        /// <returns>Panel where button were created</returns>
        public Panel CreateButton(string name, string text, Type externalCommandType, Action<Button> action)
        {
            var button = new Button(name, text, externalCommandType);
            action?.Invoke(button);

            RibbonItemData buttonData = button.Finish();

            Source.AddItem(buttonData);

            return this;
        }

        public Panel CreatePullDownButton(string name, string text, Action<PulldownButton> action)
        {
            var button = new PulldownButton(name, text, nameConvention);

            action?.Invoke(button);

            RibbonItemData buttonData = button.Finish();

            var ribbonItem = Source.AddItem(buttonData) as Autodesk.Revit.UI.PulldownButton;

            button.BuildButtons(ribbonItem);

            return this;
        }

        public Panel CreateSplitButton(string name, string text, Action<StackedItem> itemsAction)
        {
            var buttonControl = new SplitButtonControl(this, name, text);

            var splitButton = buttonControl.Finish();

            var stackedItem = new StackedItem(nameConvention);
            
            itemsAction.Invoke(stackedItem);

            var pushButtons = stackedItem
                .Buttons
                .Select(x => x.Finish())
                .Cast<PushButtonData>()
                .Select(x => splitButton.AddPushButton(x))
                .ToList();

            splitButton.CurrentButton = pushButtons[stackedItem.GetDefaultButtonIndex()];

            return this;
        }

        /// <summary>
        /// Create separator on the panel
        /// </summary>
        /// <returns></returns>
        public Panel CreateSeparator()
        {
            Source.AddSeparator();
            return this;
        }

        public RibbonItem Find<TExternalCommandClass>()
            where TExternalCommandClass : class, IExternalCommand
        {
            return Find<TExternalCommandClass>(nameConvention);
        }

        public RibbonItem Find<TExternalCommandClass>(RibbonItemNameConvention convention)
            where TExternalCommandClass : class, IExternalCommand
        {
            if (convention == null)
                throw new NameConventionNotSpecifiedException();

            var itemName = convention.GetRibbonItemName<TExternalCommandClass>();

            return Find(itemName);
        }

        public RibbonItem Find(string itemName)
        {
            return Source.GetItems().FirstOrDefault(x => x.Name == itemName);
        }

        public void MoveToSystemTab<TExternalCommandClass>(string systemTabId, string systemPanelId)
            where TExternalCommandClass : class, IExternalCommand
        {
            var application = Tab.Ribbon.ControlledApplication;

            if (application == null)
                throw new InvalidOperationException("can't move to system tab, ribbon must be created from controlledApplication");

            application.ControlledApplication.ApplicationInitialized += (sender, e) =>
            {
                var ribbonControl = Autodesk.Windows.ComponentManager.Ribbon;

                var destTab = ribbonControl.Tabs.Single(x => x.Id == systemTabId);
                var destPanel = destTab.Panels.Single(x => x.Source.Id == systemPanelId);

                var sourceTab = ribbonControl.Tabs.Single(x => x.Id == Tab.Title);
                var sourcePanel = sourceTab.Panels.Single(x => x.Source.Name == Name);

                var sourceCommandId = $"CustomCtrl_%CustomCtrl_%{Tab.Title}%{Name}%{Find<TExternalCommandClass>().Name}";

                var sourceItem = sourcePanel.Source.Items.Single(x => x.Id == sourceCommandId);

                destPanel.Source.Items.Add(sourceItem);

                sourcePanel.Source.Items.Remove(sourceItem);

                if (!sourcePanel.Source.Items.Any())
                    sourceTab.Panels.Remove(sourcePanel);

                if (!sourceTab.Panels.Any())
                    ribbonControl.Tabs.Remove(sourceTab);
            };
        }
    }
}