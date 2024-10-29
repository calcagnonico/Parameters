using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Interop;
using System;

namespace Parameters
{
    public partial class ParameterScannerWPF : Window
    {
        private Document doc;
        private UIDocument uiDoc;
        private UIApplication uiApp;
        private ExternalEvent externalEvent;
        private GenericExternalEventHandler eventHandler;

        public ParameterScannerWPF(Document doc, UIApplication uiApp, UIDocument uiDoc)
        {
            this.doc = doc;
            this.uiApp = uiApp;
            this.uiDoc = uiDoc;
            this.eventHandler = new GenericExternalEventHandler();
            this.externalEvent = ExternalEvent.Create(eventHandler);
            InitializeComponent();
            CenterPosition();
        }

        private void btn_Select_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "")
            {
                lblError.Content = "Parameter name must be filled in";
                lblError.Foreground = System.Windows.Media.Brushes.Red;
                lblError.Visibility = System.Windows.Visibility.Visible;
                return;
            }

            try
            {
                eventHandler.ActionToExecute = SelectElements;
                externalEvent.Raise();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void btn_Isolate_Click(object sender, RoutedEventArgs e)
        {
            UIDocument uiDoc = uiApp.ActiveUIDocument;

            if (doc.ActiveView.IsInTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate))
            {
                doc.ActiveView.DisableTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate);
                uiDoc.RefreshActiveView();
                return;
            }

            ICollection<ElementId> selElements = uiDoc.Selection.GetElementIds();
            if (selElements == null)
            {
                return;
            }

            try
            {
                eventHandler.ActionToExecute = IsolateElements;
                externalEvent.Raise();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private void SelectElements(UIApplication app)
        {
            string paraName = txtName.Text;
            string paraValue = txtValue.Text;

            FilteredElementCollector paraCollector = new FilteredElementCollector(doc);
            bool exists = paraCollector.WhereElementIsNotElementType()
            .Any(e => e.LookupParameter(paraName) != null);

            if (!exists)
            {
                lblError.Content = "No elements found with that parameter";
                lblError.Foreground = System.Windows.Media.Brushes.Red;
                lblError.Visibility = System.Windows.Visibility.Visible;
                return;
            }

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> filterElements = collector
            .WhereElementIsNotElementType()
            .ToElements()
            .Where(e =>
            {
                Parameter parameter = e.LookupParameter(paraName);

                if (parameter == null)
                {
                    return false;
                }

                if (parameter.AsString() == paraValue)
                {
                    return true;
                }

                if (string.IsNullOrEmpty(paraValue) && (parameter.AsString() == null || parameter.AsString() == ""))
                {
                    return true;
                }

                return false;
            })
            .ToList();

            if (filterElements.Count == 0)
            {
                lblError.Content = "No elements contain that value in the parameter";
                lblError.Foreground = System.Windows.Media.Brushes.Red;
                lblError.Visibility = System.Windows.Visibility.Visible;
                return;
            }

            lblError.Visibility = System.Windows.Visibility.Visible;
            lblError.Foreground = System.Windows.Media.Brushes.Green;
            lblError.Content = filterElements.Count + " Elements selected";


            using (Transaction trans = new Transaction(uiApp.ActiveUIDocument.Document, "Select Elements"))
            {
                trans.Start();

                List<ElementId> elementIds = filterElements.Select(e => e.Id).ToList();
                uiApp.ActiveUIDocument.Selection.SetElementIds(elementIds);

                trans.Commit();
            }
        }

        private void IsolateElements(UIApplication app)
        {
            View actView = uiApp.ActiveUIDocument.ActiveView;
            ICollection<ElementId> selElements = uiApp.ActiveUIDocument.Selection.GetElementIds();

            if (selElements.Count == 0)
            {
                return;
            }

            using (Transaction trans = new Transaction(actView.Document, "Isolate Elements"))
            {
                trans.Start();
                actView.IsolateElementsTemporary(selElements);
                trans.Commit();
            }
        }

        private void ClearErrorMsg(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (lblError.Visibility == System.Windows.Visibility.Visible)
            {
                lblError.Foreground = System.Windows.Media.Brushes.Red;
                lblError.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void txtBoxIntro(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_Select_Click(sender, e);
                e.Handled = true;
            }
        }

        private void CenterPosition()
        {
            Process rvtProcess = Process.GetCurrentProcess();
            IntPtr rvtHandle = rvtProcess.MainWindowHandle;
            WindowInteropHelper helper = new WindowInteropHelper(this);
            helper.Owner = rvtHandle;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }
}
