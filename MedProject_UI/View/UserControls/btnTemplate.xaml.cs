using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
    /// Interaction logic for btnTemplate.xaml
    /// </summary>
    public partial class btnTemplate : UserControl
    {
        public event RoutedEventHandler btnClick;
        public btnTemplate()
        {
            InitializeComponent();
        }

        private string myBtnText;
        public string MyBtnText
        {
            get { return myBtnText; }
            set { myBtnText = value; btnAdd.Content = myBtnText; }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            btnClick.Invoke(this, e);
        }
    }
}
