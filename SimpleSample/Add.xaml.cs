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
using System.Windows.Navigation;
using System.Windows.Shapes;
using NetworkUI;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using Utils;
using NetworkModel;
using System.Collections;
using System.ComponentModel;
using System.IO;
using ClosedXML.Excel;
using System.Reflection;
using System.Data;
using System.Collections.ObjectModel;


namespace SampleCode
{
    /// <summary>
    /// This is a Window that uses NetworkView to display a flow-chart.
    /// </summary>
    public partial class Add : Window
    {
        public NodeViewModel node_source;
        public NodeViewModel node_dest;
        public int node_count, node_one,total;
        public string tree_name, tree_image;

        class COUNT
        {
            public string 工單號碼 { get; set; }
            public string 料號 { get; set; }
            public string 產出日期 { get; set; }
            public string 產出量 { get; set; }
        }

        class APS
        {
            public string 工單號碼 { get; set; }
            public string 料號 { get; set; }
            public string 工作站 { get; set; }
            public string 產出日期 { get; set; }
            public string 產出量 { get; set; }
        }

        public class Node
        {
            public string Name { get; set; }
            public string Image { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
        }

        public class Connection
        {
            public string Source { get; set; }
            public string Dest { get; set; }
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            New newWindow = new New();
            newWindow.ShowDialog();
            Select_DB();
        }

        private void AQL_Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AQL_Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        public Add()
        {
            InitializeComponent();
            Select_DB();
        }

        private void Btn_Next_Click(object sender, RoutedEventArgs e)
        {

            ObservableCollection<COUNT> data_count;
            data_count = new ObservableCollection<COUNT>();
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
                    data_count.Add(item);
                }

            }
            foreach (var node in data_count)
            {
                if (node.工單號碼 != TB_WO.Text)
                {
                    TB_WO.Text= node.工單號碼;
                    Select_Count();
                    return;
                }
            }
            
        }

        public void Select_DB()
        {
            //node設定
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
                        Y = Double.Parse(ws.Cell(i, 4).Value.ToString()),
                    };
                    nodes.Add(item);
                }

            }

            node_count = 1;
            this.ViewModel.Network = new NetworkViewModel();

            foreach (var node in nodes)
            {
                this.ViewModel.CreateNode(node.Name, new Point(node.X, node.Y), node.Image);
            }

            //connection設定
            var connections = new List<Connection>();
            using (XLWorkbook wb2 = new XLWorkbook($@"./Connection.xlsx"))
            {
                var ws2 = wb2.Worksheets.Worksheet("Report");
                var range2 = ws2.RangeUsed();
                for (int i = 2; i < range2.RowCount() + 2; i++)
                {
                    Connection item = new Connection
                    {
                        Source = ws2.Cell(i, 1).Value.ToString(),
                        Dest = ws2.Cell(i, 2).Value.ToString()
                    };
                    connections.Add(item);
                }

            }

            foreach (var item in connections)
            {
                var nodesCopy = this.ViewModel.Network.Nodes.ToArray();
                foreach (var node in nodesCopy)
                {
                    if (node.Name == item.Source)
                    {
                        node_source = node;
                    }
                    if (node.Name == item.Dest)
                    {
                        node_dest = node;
                    }
                }
                this.ViewModel.ConnNode(node_source, node_dest);
            }

            ObservableCollection<APS> data_desc;
            data_desc = new ObservableCollection<APS>();
            using (XLWorkbook wb2 = new XLWorkbook($@"./APS.xlsx"))
            {
                var ws2 = wb2.Worksheets.Worksheet("Report");
                var range2 = ws2.RangeUsed();
                for (int i = 2; i < range2.RowCount() + 2; i++)
                {
                    APS item = new APS
                    {
                        工單號碼 = ws2.Cell(i, 1).Value.ToString(),
                        料號 = ws2.Cell(i, 2).Value.ToString(),
                        工作站 = ws2.Cell(i, 3).Value.ToString(),
                        產出日期 = ws2.Cell(i, 4).Value.ToString(),
                        產出量 = ws2.Cell(i, 5).Value.ToString(),
                    };
                    data_desc.Add(item);
                    if (TB_WO.Text =="")
                    {
                        TB_WO.Text = item.工單號碼;
                    }
                    
                }

            }
            Dag_TC_AQL_DESC.ItemsSource = data_desc;
            Select_Count();

        }

        public void Select_Count()
        {
            //合計
            ObservableCollection<COUNT> data_count;
            data_count = new ObservableCollection<COUNT>();
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
                    data_count.Add(item);
                }

            }

            foreach (var node in data_count)
            {
                if (node.工單號碼 == TB_WO.Text)
                {
                    total = int.Parse(node.產出量);
                }
            }

            var nodesCopy2 = this.ViewModel.Network.Nodes.ToArray();
            foreach (var node in nodesCopy2)
            {
                if (node.Name != "*開始" && node.Name != "*結束")
                {
                    node.Source = total;
                }
                if (node.Name == "*開始")
                {
                    node.Source = 0;
                }
                if (node.Name == "*結束")
                {
                    node.Source = 0;
                }
            }
        }
        public MainWindowViewModel ViewModel
        {
            get
            {
                return (MainWindowViewModel)this.DataContext;
            }
        }

    }
}


