using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace Parameters
{
    [Transaction(TransactionMode.Manual)]
    internal class ParameterScanner : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;
            View activeview = doc.ActiveView;

            if (!(activeview.ViewType == ViewType.FloorPlan || activeview.ViewType == ViewType.CeilingPlan || activeview.ViewType == ViewType.ThreeD))
            {
                TaskDialog.Show("Error", "This function must be executed in Floor Plans, RCPs, or 3D Views");
                return Result.Cancelled;
            }

            try
            {
                ParameterScannerWPF wnd = new ParameterScannerWPF(doc, uiApp, uiDoc);
                wnd.Show();
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = $"An unexpected error occurred: {ex.Message}";
                return Result.Failed;
            }
        }
    }
}




