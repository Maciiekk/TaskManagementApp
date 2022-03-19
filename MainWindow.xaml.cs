using System;
using System.Collections.Generic;
using System.Data;
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
using Npgsql;

namespace APPToDo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
     
        LoginPage loginPage;
        List<Task> tasks = new List<Task>();
        bool Edit = false;
        public MainWindow(LoginPage loginPage)
        {
            this.loginPage = loginPage;
            InitializeComponent();
            LoadTasks();
        }
        Connection con = new Connection();
        public async void LoadTasks()
        {
            
            await con.ListData(tasks);
            ListViewTask.ItemsSource = tasks;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (loginPage != null)
                loginPage.Close();
            this.Close();
        }


        private void ListViewTask_GotFocus(object sender, RoutedEventArgs e)
        {
        ButtonEdit.IsEnabled = true;
        ButtonDelete.IsEnabled = true;
         
        }

        private void ButtonEdit_Clicked(object sender, RoutedEventArgs e)
        {
            Task t = tasks[ListViewTask.SelectedIndex];
            TextBoxName.Text = t.tasksname;
            TextBoxContent.Text = t.taskscontent;
            Edit= true;
            ButtonConfirm.Content = "Confirm";

        }

        private async void ButtonDelete_Clicked(object sender, RoutedEventArgs e)
        {
            Task t = tasks[ListViewTask.SelectedIndex];
            await con.DeleteTask(t.tasksid);
            tasks = new List<Task>();
            await con.ListData(tasks);
            ListViewTask.ItemsSource = tasks;
            ButtonConfirm.Content = "Create";
            ButtonEdit.IsEnabled = false;
            ButtonDelete.IsEnabled = false;
        }

        private async void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!Edit)
            {
               await con.CreateTask(TextBoxName.Text, TextBoxContent.Text);
                tasks = new List<Task>();
                await con.ListData(tasks);
                ListViewTask.ItemsSource = tasks;
            }
            else
            {
                Task t = tasks[ListViewTask.SelectedIndex];
                await con.EditTask(t.tasksid, TextBoxName.Text, TextBoxContent.Text);
                tasks = new List<Task>();
                await con.ListData(tasks);
                ListViewTask.ItemsSource = tasks;
                Edit = false;
                ButtonConfirm.Content = "Create";
                Focus();
                ButtonEdit.IsEnabled = false;
                ButtonDelete.IsEnabled = false;
                TextBoxName.Text = "Name";
                TextBoxContent.Text = "Content";
            }
        }
    }
}
  
