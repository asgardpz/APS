using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SampleCode
{
    /// <summary>
    /// Login.xaml 的互動邏輯
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            TB_User.Text = Properties.Settings.Default.ID;
            TB_Name.Text = Properties.Settings.Default.NAME;
        }

        private void Btn_Set_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow();
            this.Close();
            newWindow.ShowDialog();
        }

        private void Btn_Logout_Click(object sender, RoutedEventArgs e)
        {
            Login newWindow = new Login();
            Properties.Settings.Default.ID = "";
            Properties.Settings.Default.NAME = "";
            this.Close();
            newWindow.ShowDialog();
        }

        private void Btn_Set_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow();
            newWindow.ShowDialog();
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            Add newWindow = new Add();
            newWindow.ShowDialog();
        }

        private void Btn_Report_Click(object sender, RoutedEventArgs e)
        {
            Report newWindow = new Report();
            newWindow.ShowDialog();
        }

        private void Btn_Chart_Click(object sender, RoutedEventArgs e)
        {
            Charts newWindow = new Charts();
            newWindow.ShowDialog();
        }
    }
}
