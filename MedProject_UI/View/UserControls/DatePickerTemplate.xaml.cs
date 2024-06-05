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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedProject_UI.View.UserControls
{
    /// <summary>
    /// Interaction logic for DatePickerTemplate.xaml
    /// </summary>
    public partial class DatePickerTemplate : UserControl
    {
        public event SelectionChangedEventHandler DateSelectionChange;
        public DatePickerTemplate()
        {
            InitializeComponent();
        }

        private void customDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            
            DateSelectionChange.Invoke(this, e);
        }
    }
}
