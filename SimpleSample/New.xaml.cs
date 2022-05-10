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
    /// New.xaml 的互動邏輯
    /// </summary>
    public partial class New : Window
    {
        public class Node
        {
            public string Name { get; set; }
            public string Image { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
        }

        class APS
        {
            public string 工單號碼 { get; set; }
            public string 料號 { get; set; }
            public string 工作站 { get; set; }
            public string 產出日期 { get; set; }
            public string 產出量 { get; set; }
        }

        class COUNT
        {
            public string 工單號碼 { get; set; }
            public string 料號 { get; set; }
            public string 產出日期 { get; set; }
            public string 產出量 { get; set; }
        }

        public int Total;

        public New()
        {
            InitializeComponent();
            Total = 1;
            TB_ORDER.Text = Total.ToString();
            var today = DateTime.Today;
            DP_Start.Text = today.ToLongDateString();
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if (TB_PART.Text != "" && TB_COUNT.Text !="")
            {
                Set_APS();
                Total += 1;
                TB_ORDER.Text = Total.ToString();
            }
            
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {

            if (TB_PART.Text != "" && TB_COUNT.Text != "")
            {
                Set_APS();
                TB_ORDER.Text = Total.ToString();
            }
        }

        public void Set_APS()
        {
            var nodes = new List<Node>();
            using (XLWorkbook wb = new XLWorkbook($@"./Node.xlsx"))
            {
                var ws = wb.Worksheets.Worksheet("Report");
                var range = ws.RangeUsed();
                for (int i = 2; i < range.RowCount() + 2; i++)
                {
                    Node item = new Node
                    {
                        Name = ws.Cell(i, 1).Value.ToString(),
                        Image = ws.Cell(i, 2).Value.ToString(),
                        X = Double.Parse(ws.Cell(i, 3).Value.ToString()),
                        Y = Double.Parse(ws.Cell(i, 4).Value.ToString())
                    };
                    if (item.Name != "*開始" && item.Name != "*結束")
                    {
                        nodes.Add(item);
                    }
                    
                }

            }

            ObservableCollection<APS> data_desc;
            data_desc = new ObservableCollection<APS>();
            ObservableCollection<COUNT> data_count;
            data_count = new ObservableCollection<COUNT>();

            if (Total > 1) 
            {
                using (XLWorkbook wb = new XLWorkbook($@"./APS.xlsx"))
                {
                    var ws = wb.Worksheets.Worksheet("Report");
                    var range = ws.RangeUsed();
                    for (int i = 2; i < range.RowCount() + 2; i++)
                    {
                        APS item = new APS
                        {
                            工單號碼 = ws.Cell(i, 1).Value.ToString(),
                            料號 = ws.Cell(i, 2).Value.ToString(),
                            工作站 = ws.Cell(i, 3).Value.ToString(),
                            產出日期 = ws.Cell(i, 4).Value.ToString(),
                            產出量 = ws.Cell(i, 5).Value.ToString(),
                        };
                        data_desc.Add(item);
                    }

                }

                using (XLWorkbook wb = new XLWorkbook($@"./COUNT.xlsx"))
                {
                    var ws = wb.Worksheets.Worksheet("Report");
                    var range = ws.RangeUsed();
                    for (int i = 2; i < range.RowCount() + 2; i++)
                    {
                        COUNT item = new COUNT
                        {
                            工單號碼 = ws.Cell(i, 1).Value.ToString(),
                            料號 = ws.Cell(i, 2).Value.ToString(),
                            產出日期 = ws.Cell(i, 3).Value.ToString(),
                            產出量 = ws.Cell(i, 4).Value.ToString(),
                        };
                        data_count.Add(item);
                    }

                }
            }

            COUNT count = new COUNT
            {
                工單號碼 = "APS000" + TB_ORDER.Text,
                料號 = TB_PART.Text,
                產出日期 = DP_Start.SelectedDate.Value.ToString("yyyy/MM/dd"),
                產出量 = TB_COUNT.Text,
            };
            data_count.Add(count);

            foreach (var node in nodes)
            {
                APS item = new APS
                {
                    工單號碼 = "APS000"+TB_ORDER.Text,
                    料號 = TB_PART.Text,
                    工作站 = node.Name,
                    產出日期 = DP_Start.SelectedDate.Value.ToString("yyyy/MM/dd"),
                    產出量 = TB_COUNT.Text,
                };
                data_desc.Add(item);
            }
            
            string filepath2 = $@"./APS.xlsx";
            XLWorkbook workbook2 = new XLWorkbook();
            var sheet2 = workbook2.Worksheets.Add("Report");
            int rowIdx2 = 2;
            foreach (var item in data_desc)
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

            string filepath3 = $@"./COUNT.xlsx";
            XLWorkbook workbook3 = new XLWorkbook();
            var sheet3 = workbook3.Worksheets.Add("Report");
            int rowIdx3 = 2;
            foreach (var item in data_count)
            {
                int conlumnIndex3 = 1;
                foreach (var jtem in item.GetType().GetProperties())
                {
                    sheet3.Cell(rowIdx3, conlumnIndex3).Value = string.Concat("'", Convert.ToString(jtem.GetValue(item, null)));
                    conlumnIndex3++;
                }
                rowIdx3++;
            }
            workbook3.SaveAs(filepath3);
        }
    }
}
