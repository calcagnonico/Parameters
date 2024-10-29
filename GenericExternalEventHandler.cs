using Autodesk.Revit.UI;
using System;

namespace Parameters
{
    public class GenericExternalEventHandler : IExternalEventHandler
    {
        public Action<UIApplication> ActionToExecute { get; set; }

        public void Execute(UIApplication app)
        {
            ActionToExecute?.Invoke(app);
        }
        public string GetName()
        {
            return "Generic External Event Handler";
        }
    }
}