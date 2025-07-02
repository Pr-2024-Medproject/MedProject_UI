using MedProject_UI.Models;
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
    /// Interaction logic for VisitSelector.xaml
    /// </summary>
    public partial class VisitSelector : Window
    {
        public Visit SelectedVisit { get; private set; }

        public VisitSelector(List<Visit> visits, string patientFullName = null)
        {
            InitializeComponent();
            lbVisitDates.ItemsSource = visits.OrderByDescending(v => v.StartDate).ToList();
            lbVisitsLabel.Text = $"Перелік візитів пацієнта {patientFullName ?? ""}";
        }

        private void btnSelectVisit_Click(object sender, RoutedEventArgs e)
        {
            if (lbVisitDates.SelectedItem is Visit selected)
            {
                SelectedVisit = selected;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть візит.", "Увага", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
