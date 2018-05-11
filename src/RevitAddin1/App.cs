using AI.RevitReinforcementDimensioner.RevitRibbonUtils;
using AI.RevitReinforcementDimensioner.RevitRibbonUtils.Conventions;
using Autodesk.Revit.UI;
using RevitAddin1.Commands;
using RevitAddin1.Properties;

namespace RevitAddin1
{
    internal class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            Ribbon.GetApplicationRibbon(application)
                .Tab("My tab")
                .Panel("My panel", new CamelCaseToUnderscoreWithPrefixNameConvention(Resources.CompanyName))
                .CreateButton<DummyCommand>("Just test", button =>
                    button
                        .SetLargeImage(Resources.UndefinedIcon32)
                        .SetSmallImage(Resources.UndefinedIcon16)
                        .SetHelpUrl("https://github.com/CADBIMDeveloper/RevitRibbonSample"))
                .CreateSplitButton("MY_SPLIT_BTN", "Split\nbutton",
                    splitButton =>
                        splitButton
                            .CreateButton<DummyCommand1>("Split button 1",
                                button => button
                                    .SetLargeImage(Resources.Crazy32)
                                    .SetSmallImage(Resources.Crazy16)
                                    .SetLongDescription("I'm available only in plan views")
                                    .SetAvailability<DummyCommand1>()
                                    .SetDefault())
                            .CreateButton<DummyCommand2>("Split button 2",
                                button => button
                                    .SetLargeImage(Resources.Biohazard_32)
                                    .SetSmallImage(Resources.Biohazard_16)))
                .CreatePullDownButton("MY_PULLDOWN_BTN", "Pulldown\nbutton",
                    pulldownButton =>
                        pulldownButton
                            .CreateButton<DummyCommand3>("Command 3")
                            .CreateButton<DummyCommand4>("Command 4")
                            .SetLargeImage(Resources.metro32)
                            .SetSmallImage(Resources.metro16))
                .CreateSeparator()
                .CreateStackedItems(item =>
                    item
                        .CreateButton<DummyCommand5>("Command 5",
                            button =>
                                button
                                    .SetLargeImage(Resources.exportDWG_32)
                                    .SetSmallImage(Resources.exportDWG_16))
                        .CreateButton<DummyCommand6>("Command 6",
                            button => 
                                button
                                    .SetLargeImage(Resources.export_32)
                                    .SetSmallImage(Resources.export_16))
                        .CreateButton<DummyCommand7>("Command 7",
                            button => 
                                button
                                    .SetLargeImage(Resources.acad_import_32)
                                    .SetSmallImage(Resources.acad_import_16)));
            
            return Result.Succeeded;
        }


        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}