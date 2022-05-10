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
    /// Report.xaml 的互動邏輯
    /// </summary>
    public partial class Report : Window
    {
        class COUNT
        {
            public string 工單號碼 { get; set; }
            public string 料號 { get; set; }
            public string 產出日期 { get; set; }
            public string 產出量 { get; set; }
        }

        public Report()
        {
            InitializeComponent();
            var today = DateTime.Today;
            DP_Start.Text = today.ToLongDateString();
            var tommorow = DateTime.Today.AddDays(1);
            DP_End.Text = tommorow.ToLongDateString();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            Select_DB();
        }

        public void Select_DB()
        {

            ObservableCollection<COUNT> data_desc;
            data_desc = new ObservableCollection<COUNT>();
            using (XLWorkbook wb2 = new XLWorkbook($@"./COUNT.xlsx"))
            {
                var ws2 = wb2.Worksheets.Worksheet("Report");
                var range2 = ws2.RangeUsed();
                for (int i = 2; i < range2.RowCount() + 2; i++)
                {
                    COUNT item = new COUNT
                    {
                        工單號碼 = ws2.Cell(i, 1).Value.ToString(),
                        料號 = ws2.Cell(i, 2).Value.ToString(),
                        產出日期 = ws2.Cell(i, 3).Value.ToString(),
                        產出量 = ws2.Cell(i, 4).Value.ToString(),
                    };
                    if (TB_PART_NO.Text != "")
                    {
                        if (TB_PART_NO.Text == item.料號 )
                        {
                            data_desc.Add(item);
                        }
                    }
                    if (TB_SHIP_NO.Text != "")
                    {
                        if (TB_SHIP_NO.Text == item.工單號碼)
                        {
                            data_desc.Add(item);
                        }
                    }
                    if (TB_SHIP_NO.Text == "" && TB_PART_NO.Text == "" )
                    {
                         data_desc.Add(item);
                    }

                }

            }
            Dag_TC_AQL_DESC.ItemsSource = data_desc;

        }


    }
}
