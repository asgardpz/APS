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
using ClosedXML.Excel;
using System.Reflection;
using System.Data;
using System.Collections.ObjectModel;

namespace SampleCode
{
    /// <summary>
    /// Login.xaml 的互動邏輯
    /// </summary>
    public partial class Login : Window
    {
        public class USER
        {
            public string 工號 { get; set; }
            public string 密碼 { get; set; }
            public string 姓名 { get; set; }
        }

        public Login()
        {
            InitializeComponent();
        }

        //登入
        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ID = "";
            Properties.Settings.Default.NAME = "";
            if (TB_User.Text =="" || PB_Pass.Password == "")
            {
                MessageBox.Show("帳號/密碼未輸入", "錯誤訊息", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                using (XLWorkbook wb = new XLWorkbook($@"./USER.xlsx"))
                {
                    var ws = wb.Worksheets.Worksheet("Report");
                    var range = ws.RangeUsed();
                    for (int i = 2; i < range.RowCount() + 2; i++)
                    {
                        USER item = new USER
                        {
                            工號 = ws.Cell(i, 1).Value.ToString(),
                            密碼 = ws.Cell(i, 2).Value.ToString(),
                            姓名 = ws.Cell(i, 3).Value.ToString(),
                        };
                        if (TB_User.Text == item.工號 && PB_Pass.Password == item.密碼)
                        {
                            Properties.Settings.Default.ID = item.工號;
                            Properties.Settings.Default.NAME = item.姓名;
                        }
                    }

                }
                if (Properties.Settings.Default.ID != "" && Properties.Settings.Default.NAME != "")
                {
                    Window1 newWindow = new Window1();
                    this.Close();
                    newWindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show("帳號/密碼錯誤", "錯誤訊息", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

        }

        //註冊
        private void Btn_Register_Click(object sender, RoutedEventArgs e)
        {
            Register newWindow = new Register();
            newWindow.ShowDialog();
        }
    }
}
