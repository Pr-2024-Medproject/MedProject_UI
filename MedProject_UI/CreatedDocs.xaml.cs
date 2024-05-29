using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MedProject_UI
{
    /// <summary>
    /// Interaction logic for CreatedDocs.xaml
    /// </summary>
    public partial class CreatedDocs : Window
    {
        public CreatedDocs()
        {
            InitializeComponent();
        }

        public CreatedDocs(int parientNumberID)
        {
            InitializeComponent();
        }

        public CreatedDocs(string patientName)
        {
            InitializeComponent();

            PatientName.Content = $"У пацієнта {patientName} існують такі документи:";

            Button testBtnDoc = new Button();

            testBtnDoc.Click += new RoutedEventHandler(GetExactDoc);
            testBtnDoc.Content = "Виписка з медичної карти";

            testBtnDoc.Name = $"{patientName}_testDoc";
            testBtnDoc.Style = (Style)Resources["CategoryButtonTemplate"];

            testBtnDoc.FontFamily = new FontFamily("Roboto");
            testBtnDoc.FontSize = 20;

            testBtnDoc.Margin = new Thickness(10, 10, 10, 10);
            testBtnDoc.Width = 600.0;
            testBtnDoc.Height = 50.0;


            ListOfDocs.Items.Add(testBtnDoc);
        }

        private void GetExactDoc(object sender, RoutedEventArgs e)
        {

        }
    }
}
