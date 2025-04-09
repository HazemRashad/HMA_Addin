using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace HMA_Addin
{
    //Automatically modify properties of existing columns.
    [Transaction(TransactionMode.Manual)]
    public class ModifyColumnsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<Reference> selectedColumnsRef = uidoc.Selection.PickObjects(ObjectType.Element, "Select columns to move.").ToList();
            List<ElementId> selectedColumnsId = new List<ElementId>();
            List<Element> selectedColumns = new List<Element>();
            List<LocationPoint> location = new List<LocationPoint>();
            List<XYZ> oldLocations = new List<XYZ>();
            List<XYZ> newLocations = new List<XYZ>();
            string info = "";

            try
            {
                using (Transaction trans = new Transaction(doc, "Move Selected Columns"))
                {
                    trans.Start();

                    try
                    {
                        foreach (Reference reference in selectedColumnsRef)
                        {
                            selectedColumnsId.Add(reference.ElementId);
                            selectedColumns.Add(doc.GetElement(reference));
                            location.Add(doc.GetElement(reference).Location as LocationPoint);
                            oldLocations.Add(location.Last().Point);
                            newLocations.Add(new XYZ(oldLocations.Last().X + 10, oldLocations.Last().Y + 10, oldLocations.Last().Z));
                            ElementTransformUtils.MoveElement(doc, selectedColumnsId.Last(), newLocations.Last() - oldLocations.Last());
                            info += $"Column moved from {oldLocations.Last()} to {newLocations.Last()}\n";
                        }

                        TaskDialog.Show("Status",info);

                        trans.Commit();
                    }

                    catch (Exception ex)
                    {
                        trans.RollBack();
                        message = ex.Message;
                    }

                    TaskDialog.Show("Success", "Selected column moved successfully.");
                }

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
