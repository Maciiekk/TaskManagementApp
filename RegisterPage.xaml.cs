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
    /// Logika interakcji dla klasy RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Window
    {
        LoginPage loginPage;
        public RegisterPage(LoginPage loginPage)
        {
            InitializeComponent();
            this.loginPage = loginPage;
            
        }
        Connection con = new Connection();
        DataControler dataControler = new DataControler();
        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            loginPage.Show();
            TextBoxWarning.Text = "";
           this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            loginPage.Close();
            this.Close();
        }

        private async void Button_Click_Register(object sender, RoutedEventArgs e)
        {
            if (dataControler.IsValidEmail(TextBoxEmail.Text))
            {
                if(TextBoxPassword.Text.Length>=0)
                {
                    if (TextBoxPassword.Text.Equals(TextBoxPasswordConfirm.Text))
                    {
                        await con.Register(TextBoxEmail.Text, TextBoxPassword.Text, TextBoxPasswordConfirm.Text);
                    }
                    else
                    {
                        TextBoxWarning.Text = "passwords must be the same";
                    }
                }
                else
                {
                    TextBoxWarning.Text = "password must be at least 8 characters long";
                }
          
               
            }
            else
            {
                TextBoxWarning.Text = "Incorrect email adres";
            }
        }
    }
}
