using System;
using AI.RevitReinforcementDimensioner.RevitRibbonUtils.Conventions;
using Autodesk.Windows;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

namespace AI.RevitReinforcementDimensioner.RevitRibbonUtils.Components
{
    public class Tab
    {
        private readonly RibbonTab ribbonTab;
        private readonly Autodesk.Revit.UI.Tab? systemTab;

        private Tab(Ribbon ribbon)
        {
            Ribbon = ribbon;
        }

        internal Tab(Ribbon ribbon, RibbonTab ribbonTab)
            : this(ribbon)
        {
            this.ribbonTab = ribbonTab;
        }

        internal Tab(Ribbon ribbon, Autodesk.Revit.UI.Tab systemTab)
            : this(ribbon)
        {
            this.systemTab = systemTab;
        }

        public bool Visible
        {
            get
            {
                if (ribbonTab == null)
                    return true;

                return ribbonTab.IsVisible;
            }
            set
            {
                if (ribbonTab != null)
                    ribbonTab.IsVisible = value;
            }
        }

        public string Title
        {
            get
            {
                if (systemTab.HasValue)
                {
                    switch (systemTab.Value)
                    {
                        case Autodesk.Revit.UI.Tab.AddIns:
                            return "Add-Ins";

                        case Autodesk.Revit.UI.Tab.Analyze:
                            return "Analyze";

                        default:
                            throw new NotSupportedException($"tab {systemTab.Value} is not supported now");
                    }
                }

                return ribbonTab.Title;
            }
        }

        public Ribbon Ribbon { get; }

        public Panel Panel(string panelTitle, RibbonItemNameConvention nameConvention = null, bool addSeparatorToExistingPanel = true)
        {
            var panels = systemTab == null
                             ? Ribbon.GetRibbonPanels(ribbonTab.Id)
                             : Ribbon.GetRibbonPanels(systemTab.Value);

            foreach (RibbonPanel panel in panels)
                if (panel.Name.Equals(panelTitle))
                {
                    if (addSeparatorToExistingPanel)
                        panel.AddSeparator();

                    return new Panel(this, panel, nameConvention);
                }
            
            var ribbonPanel = systemTab == null
                                  ? Ribbon.CreateRibbonPanel(ribbonTab.Name ?? ribbonTab.Title, panelTitle)
                                  : Ribbon.CreateRibbonPanel(systemTab.Value, panelTitle);

            return new Panel(this, ribbonPanel, nameConvention);
        }
    }
}