using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace HMA_Addin
{
    //Displays a simple message box.
    [Transaction(TransactionMode.Manual)]
    public class HelloFromHMA : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("Hello", "Hello from HMA Plugin!'\nPlugin is created by:\nAhmed Moenes\nMostafa Nabil\nHazem Rashad");
            return Result.Succeeded;
        }
    }
}
