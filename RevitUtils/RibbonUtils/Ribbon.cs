using System;
using System.Collections.Generic;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using RevitUtils.Extensions;
using UIFramework;
using RibbonPanel = Autodesk.Revit.UI.RibbonPanel;

namespace RevitUtils.RibbonUtils
{
    public class Ribbon
    {
        private readonly UIControlledApplication controlledApplication;
        private readonly UIApplication uiApplication;
        private readonly RibbonControl ribbonControl;

        private Ribbon(UIControlledApplication application)
            : this()
        {
            controlledApplication = application;
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

        internal UIControlledApplication ControlledApplication => controlledApplication;

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
            return controlledApplication != null
                       ? controlledApplication.GetRibbonPanels(tabName)
                       : uiApplication.GetRibbonPanels(tabName);
        }

        internal IEnumerable<RibbonPanel> GetRibbonPanels(Autodesk.Revit.UI.Tab systemTab)
        {
            return controlledApplication != null
                       ? controlledApplication.GetRibbonPanels(systemTab)
                       : uiApplication.GetRibbonPanels(systemTab);
        }

        internal RibbonPanel CreateRibbonPanel(string tabName, string panelName)
        {
            return controlledApplication != null
                       ? controlledApplication.CreateRibbonPanel(tabName, panelName)
                       : uiApplication.CreateRibbonPanel(tabName, panelName);
        }

        internal RibbonPanel CreateRibbonPanel(Autodesk.Revit.UI.Tab systemTab, string panelName)
        {
            return controlledApplication != null
                       ? controlledApplication.CreateRibbonPanel(systemTab, panelName)
                       : uiApplication.CreateRibbonPanel(systemTab, panelName);
        }

        private Tab Tab(RibbonTab ribbonTab)
        {
            return new Tab(this, ribbonTab);
        }

        private RibbonTab CreateRibbonTab(string tabTitle)
        {
            if (controlledApplication != null)
                controlledApplication.CreateRibbonTab(tabTitle);
            else
                uiApplication.CreateRibbonTab(tabTitle);

            return ribbonControl.FindTabByTitle(tabTitle);
        }
    }
}