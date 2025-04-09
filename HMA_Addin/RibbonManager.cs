using System;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace HMA_Addin
{
    [Transaction(TransactionMode.Manual)]
    public class RibbonManager : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            
            // Add a new ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel("StrColumns Manager");

            // Create a push button in the ribbon panel
            PushButtonData helloButtonData = new PushButtonData(
                "About", // Button name
                "About Us", // Button text
                @"C:\Users\Ahmed Moenes\AppData\Roaming\Autodesk\Revit\Addins\2022\HMA_Addin.dll", // Path to DLL
                "HMA_Addin.HelloFromHMA" // Fully qualified class name
            );
            // Set the large image shown on the button
            Uri uriImage = new Uri(@"C:\Users\Ahmed Moenes\Desktop\ITI logo.png", UriKind.Absolute);
            BitmapImage largeImage = new BitmapImage(uriImage);
            PushButton helloButton = ribbonPanel.AddItem(helloButtonData) as PushButton;
            helloButton.LargeImage = largeImage;

            PushButtonData createColumnButtonData = new PushButtonData(
                "StrColumnsCreate",
                "Create",
                @"C:\Users\Ahmed Moenes\AppData\Roaming\Autodesk\Revit\Addins\2022\HMA_Addin.dll",
                "HMA_Addin.CreateColumnsCommand" 
            );
            Uri createImage = new Uri(@"C:\Users\Ahmed Moenes\Desktop\add.png", UriKind.Absolute);
            BitmapImage createLargeImage = new BitmapImage(createImage);
            PushButton createColumnButton = ribbonPanel.AddItem(createColumnButtonData) as PushButton;
            createColumnButton.LargeImage = createLargeImage;

            PushButtonData removeColumnButtonData = new PushButtonData(
                "StrColumnsRemove",
                "Remove",
                @"C:\Users\Ahmed Moenes\AppData\Roaming\Autodesk\Revit\Addins\2022\HMA_Addin.dll",
                "HMA_Addin.RemoveColumnsCommand"
            );
            Uri removeImage = new Uri(@"C:\Users\Ahmed Moenes\Desktop\remove.png", UriKind.Absolute);
            BitmapImage removeLargeImage = new BitmapImage(removeImage);
            PushButton removeColumnButton = ribbonPanel.AddItem(removeColumnButtonData) as PushButton;
            removeColumnButton.LargeImage = removeLargeImage;

            PushButtonData modifyColumnButtonData = new PushButtonData(
                "StrColumnsModify",
                "Move",
                @"C:\Users\Ahmed Moenes\AppData\Roaming\Autodesk\Revit\Addins\2022\HMA_Addin.dll",
                "HMA_Addin.ModifyColumnsCommand"
            );
            Uri modifyImage = new Uri(@"C:\Users\Ahmed Moenes\Desktop\modify.png", UriKind.Absolute);
            BitmapImage modifyLargeImage = new BitmapImage(modifyImage);
            PushButton modifyColumnButton = ribbonPanel.AddItem(modifyColumnButtonData) as PushButton;
            modifyColumnButton.LargeImage = modifyLargeImage;

            PushButtonData annotateColumnButtonData = new PushButtonData(
                "Add Tags",
                "Annotate",
                @"C:\Users\Ahmed Moenes\AppData\Roaming\Autodesk\Revit\Addins\2022\HMA_Addin.dll",
                "HMA_Addin.AnnotateColumnsCommand"
            );
            Uri annotateImage = new Uri(@"C:\Users\Ahmed Moenes\Desktop\annotate.png", UriKind.Absolute);
            BitmapImage annotateLargeImage = new BitmapImage(annotateImage);
            PushButton annotateColumnButton = ribbonPanel.AddItem(annotateColumnButtonData) as PushButton;
            annotateColumnButton.LargeImage = annotateLargeImage;

            PushButtonData analyzeColumnButtonData = new PushButtonData(
                "Create Sheets",
                "Analyze",
                @"C:\Users\Ahmed Moenes\AppData\Roaming\Autodesk\Revit\Addins\2022\HMA_Addin.dll",
                "HMA_Addin.AnalyzeColumnsCommand"
            );
            Uri analyzeImage = new Uri(@"C:\Users\Ahmed Moenes\Desktop\analyze.png", UriKind.Absolute);
            BitmapImage analyzeLargeImage = new BitmapImage(analyzeImage);
            PushButton analyzeColumnButton = ribbonPanel.AddItem(analyzeColumnButtonData) as PushButton;
            analyzeColumnButton.LargeImage = analyzeLargeImage;

            return Result.Succeeded;
        }
    }

}