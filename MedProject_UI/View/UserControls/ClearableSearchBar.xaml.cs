using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ClearableSearchBar.xaml
    /// </summary>
    public partial class ClearableSearchBar : UserControl
    {
        public event TextChangedEventHandler TextChanged;

        public ClearableSearchBar()
        {
            InitializeComponent();
        }

        private string myLabel;
        public string MyLabel
        {
            get { return myLabel; }
            set { myLabel = value; tbLabel.Text = myLabel; }
        }

        private double myWidth;
        public double MyWidth
        {
            get { return myWidth; }
            set { myWidth = value; tbSearchText.Width = myWidth; }
        }


        private void btnClearClick(object sender, RoutedEventArgs e)
        {
            tbSearchText.Clear();
            /*tbSearchText.Focus();*/
        }

        
        private void tbSearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbSearchText.Text))
            {
                tbLabel.Visibility = Visibility.Visible;
            }
            else
            {
                tbLabel.Visibility = Visibility.Hidden;
            }
            TextChanged.Invoke(this, e);
        }

        private void tbSearchText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[А-ЩЬЮЯЄІЇҐа-щьюяєіїґ0-9/' ]+$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
            base.OnPreviewTextInput(e);
        }
    }
}
