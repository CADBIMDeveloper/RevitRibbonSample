using Autodesk.Revit.UI;
using RevitAddin1.Properties;
using RevitUtils.RibbonUtils;

namespace RevitAddin1
{
    internal class App : IExternalApplication
    {
        private Panel panel;

        public Result OnStartup(
            UIControlledApplication application)
        {

            panel = Ribbon.GetApplicationRibbon(application)
                .Tab("Мой tab")
                .Panel("Моя панелька", new CamelCaseToUnderscoreWithPrefixNameConvention(Resources.CompanyName))
                .CreateButton<Command>("Just test", button =>
                    button
                        .SetLargeImage(Resources.UndefinedIcon32)
                        .SetSmallImage(Resources.UndefinedIcon16));
            
            panel.MoveToSystemTab<Command>("Collaborate", "central_file_shr");

            return Result.Succeeded;
        }


        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}