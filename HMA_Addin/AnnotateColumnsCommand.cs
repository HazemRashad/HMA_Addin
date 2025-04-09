using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace HMA_Addin
{
    //Automatically add tags and dimensions to columns.
    [Transaction(TransactionMode.Manual)]
    public class AnnotateColumnsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {

                List<Reference> selectedColumnsRef = uidoc.Selection.PickObjects(ObjectType.Element, "Select columns to annotate.").ToList();

                List<ElementId> selectedColumnsId = new List<ElementId>();
                                foreach (Reference reference in selectedColumnsRef) selectedColumnsId.Add(reference.ElementId);

                List<Element> selectedColumns = selectedColumnsRef
                   .Select(r => doc.GetElement(r)).Where(e => e is FamilyInstance && e.Location is LocationPoint)
                   .ToList();

                if (selectedColumns.Count == 0)
                {
                    TaskDialog.Show("Error", "No valid columns with LocationPoint found.");
                    return Result.Failed;
                }

                using (Transaction trans = new Transaction(doc, "Annotate Selected Columns"))
                {
                    trans.Start();
                    try
                    {
                        #region dimensions
                        //if (selectedColumns.Count > 1)
                        //{
                        //    for (int i = 0; i < selectedColumns.Count - 1; i++)
                        //    {
                        //        LocationPoint loc1 = selectedColumns[i].Location as LocationPoint;
                        //        LocationPoint loc2 = selectedColumns[i + 1].Location as LocationPoint;

                        //        double offset = 2;
                        //        XYZ startPoint = new XYZ(loc1.Point.X, loc1.Point.Y - offset, loc1.Point.Z);
                        //        XYZ endPoint = new XYZ(loc2.Point.X, loc2.Point.Y - offset, loc2.Point.Z);
                        //        Line dimensionLine = Line.CreateBound(startPoint, endPoint);

                        //        ReferenceArray refArray = new ReferenceArray();
                        //        Reference ref1 = GetGeometricReference(selectedColumns[i]);
                        //        Reference ref2 = GetGeometricReference(selectedColumns[i + 1]);

                        //        if (ref1 != null && ref2 != null)
                        //        {
                        //            refArray.Append(ref1);
                        //            refArray.Append(ref2);

                        //            try
                        //            {
                        //                Dimension newDimension = doc.Create.NewDimension(uidoc.ActiveView, dimensionLine, refArray);
                        //            }
                        //            catch (Exception ex)
                        //            {
                        //                TaskDialog.Show("Error", $"Failed to create dimension: {ex.Message}");
                        //            }
                        //        }
                        //        else
                        //        {
                        //            TaskDialog.Show("Error", "Could not find valid geometric references for one or both columns.");
                        //        }

                        //    }
                        //}

                        #endregion

                        #region tags
                        foreach (Element column in selectedColumns)
                            {
                                LocationPoint location = (LocationPoint)column.Location;
                                XYZ tagPosition = new XYZ(location.Point.X + 1, location.Point.Y + 2, location.Point.Z);
                                IndependentTag tag = IndependentTag.Create(doc, doc.ActiveView.Id, new Reference(column), true,
                                                     TagMode.TM_ADDBY_CATEGORY, TagOrientation.Horizontal, tagPosition);
                            }
                            #endregion

                            TaskDialog.Show("Success", $"Annotated {selectedColumns.Count} columns.");
                        
                            trans.Commit();
                            return Result.Succeeded;
                    }

                    catch (Exception)
                    {
                        trans.RollBack();
                        TaskDialog.Show("Canceled", "No columns were selected.");
                        return Result.Cancelled;
                    }
                }
                
            }
            catch (Exception exception)
            {
                message = exception.Message;
                return Result.Failed;   
            }
        }

        //private Reference GetGeometricReference(Element element)
        //{
        //    Options options = new Options();
        //    GeometryElement geometryElement = element.get_Geometry(options);

        //    foreach (var Obj in geometryElement)
        //    {
        //        Solid solid = Obj as Solid;
        //        Reference faceRef = solid.Faces.get_Item(0).Reference;
        //    }

        //    throw new Exception("No geometric reference found for the element.");
        //}
    }
}
