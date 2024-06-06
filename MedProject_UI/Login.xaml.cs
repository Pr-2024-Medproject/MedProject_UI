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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void BTN_Login_Click(object sender, RoutedEventArgs e)
        {
            //if (TB_Login.Text == "Anastasiia" && TB_Pass.Password == "1806")
            //{
            //    MainWindow main = new MainWindow();
            //    this.Close();
            //    main.Show();
            //}
            //else {
            //    MessageBox.Show("Введений пароль або логін не правильні!", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Warning);
            //}

            MainWindow main = new MainWindow();
            this.Close();
            main.Show();

        }
    }
}
