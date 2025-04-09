using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace HMA_Addin
{
    //Automatically generate a schedule of columns.
    [Transaction(TransactionMode.Manual)]
    public class AnalyzeColumnsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            try
            {
                var columns = new FilteredElementCollector(doc)
                   .OfClass(typeof(FamilyInstance))
                   .OfCategory(BuiltInCategory.OST_StructuralColumns)
                   .Cast<FamilyInstance>()
                   .ToList();

                if (columns.Count == 0)
                {
                    TaskDialog.Show("Error", "No structural columns found in the model.");
                    return Result.Failed;
                }

                

                using (Transaction trans = new Transaction(doc,"Generate Column Schedule"))
                {
                    try
                    {
                        trans.Start();
                        //TaskDialog.Show("Debug", "Transaction started.");
                        ViewSchedule columnSchedule = ViewSchedule.CreateSchedule(doc, new ElementId(BuiltInCategory.OST_StructuralColumns));
                        //TaskDialog.Show("Debug", "Schedule created."); 
                        AddRegularFieldToSchedule(columnSchedule, new ElementId(BuiltInParameter.HOST_VOLUME_COMPUTED));
                        AddRegularFieldToSchedule(columnSchedule, new ElementId(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM));
                        AddRegularFieldToSchedule(columnSchedule, new ElementId(BuiltInParameter.INSTANCE_LENGTH_PARAM));

                        //TaskDialog.Show("Debug", "Fields added to schedule.");

                        ViewSheet columnsSheet = CreateSheet(doc, "Column Schedule");
                        columnsSheet.SheetNumber = "001";
                        columnsSheet.Name = "Column Schedule";

                        XYZ scheduleLocation = new XYZ(0.1, 0.9, 0);


                        trans.Commit();
                    }
                    
                    
                    catch (Exception)
                    {
                        trans.RollBack();
                        return Result.Cancelled;
                    }

                    TaskDialog.Show("Success", "Column schedule generated and placed on a sheet.");
                    return Result.Succeeded;
                }

            }
            catch (Exception exception)
            {
                message = exception.Message;
                return Result.Failed;
            }
        }
        public static void AddRegularFieldToSchedule(ViewSchedule schedule, ElementId paramId)
        {
            ScheduleDefinition definition = schedule.Definition;

            SchedulableField schedulableField =
                definition.GetSchedulableFields().FirstOrDefault<SchedulableField>(sf => sf.ParameterId == paramId);

            if (schedulableField != null)
            {
                definition.AddField(schedulableField);
            }
            else
            {
                TaskDialog.Show("Error", $"The parameter with ID {paramId} is not schedulable for this category.");
            }
        }

        private ViewSheet CreateSheet(Document doc, string sheetName)
        {
            FilteredElementCollector titleBlocks = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_TitleBlocks);

            FamilySymbol titleBlock = titleBlocks.FirstElement() as FamilySymbol;

            ViewSheet sheet = ViewSheet.Create(doc, titleBlock.Id);

            return sheet;
        }

    }
}
