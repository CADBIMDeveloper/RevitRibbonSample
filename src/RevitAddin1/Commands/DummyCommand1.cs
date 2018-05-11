using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitAddin1.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class DummyCommand1 : IExternalCommand, IExternalCommandAvailability
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return Result.Succeeded;
        }

        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            var activeView = applicationData.ActiveUIDocument?.ActiveGraphicalView;

            return activeView?.ViewType == ViewType.FloorPlan;
        }
    }
}