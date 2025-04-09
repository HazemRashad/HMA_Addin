using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace HMA_Addin
{
    //Automatically create structural columns at predefined locations.

    [Transaction(TransactionMode.Manual)]
    public class CreateColumnsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Level level = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Cast<Level>()
                .Where(l => l.Name.Contains("02")).FirstOrDefault();

            FamilySymbol columnType = new FilteredElementCollector(doc)
            .OfClass(typeof(FamilySymbol))
            .OfCategory(BuiltInCategory.OST_StructuralColumns)
            .Cast<FamilySymbol>()
            .FirstOrDefault();

            if (columnType == null)
            {
                TaskDialog.Show("Error", "No structural column type found.");
                return Result.Failed;
            }

            List<XYZ> placementPoints = new List<XYZ>
            {
                new XYZ(0, 0, 0),  
                new XYZ(10, 0, 0), 
                new XYZ(20, 0, 0)  
            };

            StructuralType type = StructuralType.Column;

            try
            {

                using (Transaction trans = new Transaction(doc, "Create Columns"))
                {
                    trans.Start();
                    try
                    {

                        foreach (XYZ point in placementPoints)
                        {
                            XYZ adjustedPoint = new XYZ(point.X, point.Y, level.Elevation);

                            FamilyInstance column = doc.Create.NewFamilyInstance(
                                adjustedPoint,
                                columnType,
                                level,
                                type
                            );
                        }

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.RollBack();
                        message = ex.Message;
                    }
                }
                    TaskDialog.Show("Success", "Columns created successfully.");
                    return Result.Succeeded;
            }
            catch (Exception exception)
            {
                message = exception.Message;
                return Result.Failed;
            }
        }

        
    }
}
