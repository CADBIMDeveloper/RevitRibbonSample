using System;
using Autodesk.Windows;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

namespace RevitUtils.RibbonUtils
{
    public class Tab
    {
        private readonly RibbonTab ribbonTab;
        private readonly Ribbon ribbon;
        private readonly Autodesk.Revit.UI.Tab? systemTab;

        private Tab(Ribbon ribbon)
        {
            this.ribbon = ribbon;
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

        internal Ribbon Ribbon => ribbon;

        public Panel Panel(string panelTitle, RibbonItemNameConvention nameConvention = null)
        {
            var panels = systemTab == null
                             ? ribbon.GetRibbonPanels(ribbonTab.Id)
                             : ribbon.GetRibbonPanels(systemTab.Value);

            foreach (RibbonPanel panel in panels)
                if (panel.Name.Equals(panelTitle))
                {
                    panel.AddSeparator();
                    return new Panel(this, panel, nameConvention);
                }

            var ribbonPanel = systemTab == null
                ? ribbon.CreateRibbonPanel(ribbonTab.Name ?? ribbonTab.Title, panelTitle)
                : ribbon.CreateRibbonPanel(systemTab.Value, panelTitle);

            return new Panel(this, ribbonPanel, nameConvention);
        }
    }
}