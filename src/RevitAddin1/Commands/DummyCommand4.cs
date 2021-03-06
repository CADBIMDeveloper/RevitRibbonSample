using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitAddin1.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class DummyCommand4 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return Result.Succeeded;
        }
    }
}