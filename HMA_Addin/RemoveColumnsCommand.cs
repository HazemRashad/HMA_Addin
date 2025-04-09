using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace HMA_Addin
{
    //Automatically delete columns and their associated annotations.
    [Transaction(TransactionMode.Manual)]
    public class RemoveColumnsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<Reference> selectedColumnsRef = uidoc.Selection.PickObjects(ObjectType.Element, "Select columns to remove.").ToList();
            List<ElementId> selectedColumnsId = new List<ElementId>();

            foreach (Reference reference in selectedColumnsRef)
            {
                selectedColumnsId.Add(reference.ElementId);
            }

            try
            {
                using (Transaction trans = new Transaction(doc, "Remove Selected Columns with dependents"))
                {
                    trans.Start();
                        try
                        {
                            foreach (ElementId selectedColumnId in selectedColumnsId)
                            {
                                doc.Delete(selectedColumnId);
                            }
                            trans.Commit();
                            TaskDialog.Show("Success", "Selected columns removed successfully.");
                        }
                        catch (Exception ex)
                        {
                            trans.RollBack();
                            message = ex.Message;
                        }
                        return Result.Succeeded;
                }
            }

            catch (Exception exception)
            {

                message = exception.Message;
                return Result.Failed;
            }
        }

    }
}

