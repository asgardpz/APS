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
using LiveCharts;
using LiveCharts.Wpf;
using ClosedXML.Excel;
using System.Windows.Threading;

namespace SampleCode
{
    /// <summary>
    /// Charts.xaml 的互動邏輯
    /// </summary>
    public partial class Charts : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        private double[] temp = { 22, 25, 30, 18, 40, 5, 2, 1, 1, 1, 11, 12 };
        private double[] temp_Line = { 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80, 80 };
        class COUNT
        {
            public string 工單號碼 { get; set; }
            public string 料號 { get; set; }
            public string 產出日期 { get; set; }
            public string 產出量 { get; set; }
        }
        DispatcherTimer timer1 = new DispatcherTimer();

        public Charts()
        {
            InitializeComponent();
            Set_Chart();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = TimeSpan.FromSeconds(10);
            timer1.Start();

        }

        public void Set_Chart()
        {
            LB_Time.Content = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            Im_M.Visibility = Visibility.Visible;
            LB_M.Visibility = Visibility.Visible;
            Im_N.Visibility = Visibility.Hidden;
            LB_N.Visibility = Visibility.Hidden;
            string NowH;
            NowH = DateTime.Now.ToString("HH");
            if (int.Parse(NowH) > 18)
            {
                Im_M.Visibility = Visibility.Hidden;
                LB_M.Visibility = Visibility.Hidden;
                Im_N.Visibility = Visibility.Visible;
                LB_N.Visibility = Visibility.Visible;
            }
            //直條圖
            ColumnSeries mylineseries = new ColumnSeries();
            mylineseries.Title = "產量";
            mylineseries.PointGeometry = null;
            Labels = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            mylineseries.Values = new ChartValues<double>(temp);
            SeriesCollection = new SeriesCollection { };
            SeriesCollection.Add(mylineseries);

            //折线图
            LineSeries target = new LineSeries();
            target.Title = "目標量";
            target.LineSmoothness = 0;
            target.Values = new ChartValues<double>(temp_Line);
            SeriesCollection.Add(target);
            DataContext = this;
            TB_TARGET.Text = "80";
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
                    if (TB_WO.Text == "")
                    {
                        TB_WO.Text = item.工單號碼;
                        TB_PART_NO.Text = item.料號;
                        TB_COUNT.Text = item.產出量;
                    }
                    else
                    {
                        TB_WO_NEXT.Text = item.工單號碼;
                    }

                }

            }
        }

        //計時器
        private void timer1_Tick(object sender, EventArgs e)
        {
            Set_Chart();
        }

    }
}
