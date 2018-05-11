using System;
using System.Collections.Generic;
using AI.RevitReinforcementDimensioner.RevitRibbonUtils.Extensions;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using UIFramework;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;
using Tab = AI.RevitReinforcementDimensioner.RevitRibbonUtils.Components.Tab;

namespace AI.RevitReinforcementDimensioner.RevitRibbonUtils
{
    public class Ribbon
    {
        private readonly UIApplication uiApplication;
        private readonly RibbonControl ribbonControl;

        private Ribbon(UIControlledApplication application)
            : this()
        {
            ControlledApplication = application;
        }

        private Ribbon(UIApplication application)
            : this()
        {
            uiApplication = application;
        }

        private Ribbon()
        {
            ribbonControl = RevitRibbonControl.RibbonControl;
            if (ribbonControl == null)
                throw new NotSupportedException("Could not initialize Revit ribbon control");
        }

        public UIControlledApplication ControlledApplication { get; }

        public static Ribbon GetApplicationRibbon(UIControlledApplication application)
        {
            return new Ribbon(application);
        }

        public static Ribbon GetApplicationRibbon(UIApplication application)
        {
            return new Ribbon(application);
        }

        public Tab Tab(string tabTitle)
        {
            var ribbonTab = ribbonControl.FindTabByTitle(tabTitle);
            if (ribbonTab != null)
                return new Tab(this, ribbonTab);

            ribbonTab = CreateRibbonTab(tabTitle);

            return Tab(ribbonTab);
        }
        
        public Tab Tab(Autodesk.Revit.UI.Tab systemTab)
        {
            return new Tab(this, systemTab);
        }

        public Tab GetModifyTab()
        {
            return Tab(ribbonControl.FindTab(ContextualTabHelper.IdModifyTab));
        }

        public bool HasTab(string tabTitle)
        {
            return ribbonControl.FindTabByTitle(tabTitle) != null;
        }

        internal IEnumerable<RibbonPanel> GetRibbonPanels(string tabName)
        {
            return ControlledApplication != null
                       ? ControlledApplication.GetRibbonPanels(tabName)
                       : uiApplication.GetRibbonPanels(tabName);
        }

        internal IEnumerable<RibbonPanel> GetRibbonPanels(Autodesk.Revit.UI.Tab systemTab)
        {
            return ControlledApplication != null
                       ? ControlledApplication.GetRibbonPanels(systemTab)
                       : uiApplication.GetRibbonPanels(systemTab);
        }

        internal RibbonPanel CreateRibbonPanel(string tabName, string panelName)
        {
            return ControlledApplication != null
                       ? ControlledApplication.CreateRibbonPanel(tabName, panelName)
                       : uiApplication.CreateRibbonPanel(tabName, panelName);
        }

        internal RibbonPanel CreateRibbonPanel(Autodesk.Revit.UI.Tab systemTab, string panelName)
        {
            return ControlledApplication != null
                       ? ControlledApplication.CreateRibbonPanel(systemTab, panelName)
                       : uiApplication.CreateRibbonPanel(systemTab, panelName);
        }

        private Tab Tab(RibbonTab ribbonTab)
        {
            return new Tab(this, ribbonTab);
        }

        private RibbonTab CreateRibbonTab(string tabTitle)
        {
            if (ControlledApplication != null)
                ControlledApplication.CreateRibbonTab(tabTitle);
            else
                uiApplication.CreateRibbonTab(tabTitle);

            return ribbonControl.FindTabByTitle(tabTitle);
        }
    }
}