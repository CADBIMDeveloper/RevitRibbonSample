using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace RevitUtils.Extensions
{
    public static class FilteredElementCollectorExtensions
    {
        public static FilteredElementCollector OfRevitClass<TElement>(this FilteredElementCollector collector) where TElement : Element
        {
            return collector
                .OfClass(typeof(TElement));
        }

        public static IEnumerable<TElement> OfRevitType<TElement>(this FilteredElementCollector collector) where TElement : Element
        {
            return collector
                .OfRevitClass<TElement>()
                .OfType<TElement>();
        }
    }
}