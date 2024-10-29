using Autodesk.Revit.UI;
using System.Reflection;

namespace Parameters
{
    internal class ExternalApplication : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab("Parameters");
            string path = Assembly.GetExecutingAssembly().Location;
            RibbonPanel parametersPanel = application.CreateRibbonPanel("Parameters", "Search");
            PushButtonData psButton = new PushButtonData("Parameter Scanner", "Parameter\nScanner", path, "Parameters.ParameterScanner");
            psButton.LongDescription = "This tool allows users to enter a parameter and its value to search for and select matching elements within the Revit model. It effectively selects the matching elements. Additionally, users can isolate these elements, allowing them to focus on the selected filtered items.";
            psButton.ToolTip = "This tool enables users to select elements by parameter and value, with the option to isolate the selected elementscd..";
            PushButton psPushButton = parametersPanel.AddItem(psButton) as PushButton;
            psPushButton.LargeImage = IconManager.getImg(Properties.Resources.RibbonIcon.GetHbitmap());
            return Result.Succeeded;
        }
    }
}
