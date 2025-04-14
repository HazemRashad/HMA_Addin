using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace HMA_Addin
{
    [Transaction(TransactionMode.Manual)]
    public class RibbonManager : IExternalApplication
    {
        string path = Assembly.GetExecutingAssembly().Location;
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab("HMA Tools");
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("HMA Tools", "StrColumns Manager");

            #region Buttons

            PushButtonData helloButtonData = new PushButtonData(
                "About",
                "About Us",
                path,
                "HMA_Addin.HelloFromHMA"
            );
            PushButton helloButton = ribbonPanel.AddItem(helloButtonData) as PushButton;
            helloButton.LargeImage = PushImage("HMA_Addin.Icons.ITI logo.png");

            PushButtonData createColumnButtonData = new PushButtonData(
                "StrColumnsCreate",
                "Create",
                path,
                "HMA_Addin.CreateColumnsCommand"
            );
            PushButton createColumnButton = ribbonPanel.AddItem(createColumnButtonData) as PushButton;
            createColumnButton.LargeImage = PushImage("HMA_Addin.Icons.add.png");

            PushButtonData removeColumnButtonData = new PushButtonData(
                "StrColumnsRemove",
                "Remove",
                path,
                "HMA_Addin.RemoveColumnsCommand"
            );
            PushButton removeColumnButton = ribbonPanel.AddItem(removeColumnButtonData) as PushButton;
            removeColumnButton.LargeImage = PushImage("HMA_Addin.Icons.remove.png");

            PushButtonData modifyColumnButtonData = new PushButtonData(
                "StrColumnsModify",
                "Move",
                path,
                "HMA_Addin.ModifyColumnsCommand"
            );
            PushButton modifyColumnButton = ribbonPanel.AddItem(modifyColumnButtonData) as PushButton;
            modifyColumnButton.LargeImage = PushImage("HMA_Addin.Icons.modify.png");

            //"annotate" button
            PushButtonData annotateColumnButtonData = new PushButtonData(
                "Add Tags",
                "Annotate",
                path,
                "HMA_Addin.AnnotateColumnsCommand"
            );
            PushButton annotateColumnButton = ribbonPanel.AddItem(annotateColumnButtonData) as PushButton;
            annotateColumnButton.LargeImage = PushImage("HMA_Addin.Icons.annotate.png");

            //"analyze" button
            PushButtonData analyzeColumnButtonData = new PushButtonData(
                "Create Sheets",
                "Analyze",
                path,
                "HMA_Addin.AnalyzeColumnsCommand"
            );
            PushButton analyzeColumnButton = ribbonPanel.AddItem(analyzeColumnButtonData) as PushButton;
            analyzeColumnButton.LargeImage = PushImage("HMA_Addin.Icons.analyze.png");
            #endregion

            return Result.Succeeded;
        }
        private BitmapImage PushImage(string resourcePath)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
                {
                    if (stream == null)
                        throw new Exception($"Image not found: {resourcePath}");

                    var image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    return image;
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", $"Failed to load image: {ex.Message}");
                return null;
            }
        }
    }

}