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

namespace SampleCode
{
    /// <summary>
    /// Register.xaml 的互動邏輯
    /// </summary>
    public partial class Register : Window
    {
        public class USER
        {
            public string 工號 { get; set; }
            public string 密碼 { get; set; }
            public string 姓名 { get; set; }
        }

        public Register()
        {
            InitializeComponent();

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var List = new List<USER>();
            string Error;
            Error = "";
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
                    List.Add(item);

                }
            }

            foreach (var list in List)
            {
                if(TB_User.Text == list.工號)
                {
                    Error = "工號重覆";
                }
            }
            if (Error == "")
            {
                USER user = new USER
                {
                    工號 = TB_User.Text,
                    密碼 = TB_Pass.Text,
                    姓名 = TB_Name.Text,
                };
                List.Add(user);

                //帳密
                string filepath2 = $@"./USER.xlsx";
                XLWorkbook workbook2 = new XLWorkbook();
                var sheet2 = workbook2.Worksheets.Add("Report");
                int rowIdx2 = 2;
                foreach (var item in List)
                {
                    int conlumnIndex2 = 1;
                    foreach (var jtem in item.GetType().GetProperties())
                    {
                        sheet2.Cell(rowIdx2, conlumnIndex2).Value = string.Concat("'", Convert.ToString(jtem.GetValue(item, null)));
                        conlumnIndex2++;
                    }
                    rowIdx2++;
                }
                workbook2.SaveAs(filepath2);
                MessageBox.Show("已儲存", "訊息", MessageBoxButton.OK, MessageBoxImage.Information);
                TB_User.Text = "";
                TB_Pass.Text = "";
                TB_Name.Text = "";
            }
            else
            {
                MessageBox.Show(Error, "錯誤訊息", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
