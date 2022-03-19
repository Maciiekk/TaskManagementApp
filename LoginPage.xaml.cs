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

namespace APPToDo
{
    /// <summary>
    /// Logika interakcji dla klasy LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        
        public LoginPage()
        {
            InitializeComponent();
        }
        Connection con = new Connection();
        DataControler dataControler = new DataControler();
        RegisterPage registerPage;
        private async void Button_Click_Login(object sender, RoutedEventArgs e)
        {

            if (dataControler.IsValidEmail(TextBoxEmail.Text))
            {
                if (await con.Login(TextBoxEmail.Text, TextBoxPassword.Text))
                {
                    MainWindow mainwindow = new MainWindow(this);
                    mainwindow.Show();
                    TextBoxWarning.Text = "";
                    this.Hide();
                }
                else
                {
                    TextBoxWarning.Text = "Incorrect combination of email and password";
                }
            }
            else
            {
                TextBoxWarning.Text = "Incorrect  email address";
            }

        }

        private void Button_Click_Open_Register_Window(object sender, RoutedEventArgs e)
        {
           if(registerPage == null)
             registerPage = new RegisterPage(this);
            
            registerPage.Show();
            TextBoxWarning.Text = "";
            this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(registerPage != null)
                registerPage.Close();
            this.Close();
        }

    }
}
