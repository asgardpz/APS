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
    public partial class MainWindow : Window
    {
        public NodeViewModel node_source;
        public NodeViewModel node_dest;
        public int node_count,node_one;
        public string tree_name, tree_image;

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
            public string One { get; set; }
        }

        public class ORDER
        {
            public string Name { get; set; }
            public int Order { get; set; }
        }
        public List<Person> persons = new List<Person>();

        public MainWindow()
        {
            InitializeComponent();
            node_count = 1;
            this.MI_Conn.IsEnabled = false;
            this.MI_Delete.IsEnabled = false;

            Person person1 = new Person() { Name = "作業站", Image = "Resources/pack.png" };
            Person person2 = new Person() { Name = "投入站", Image = "Resources/process.png" };
            Person person3 = new Person() { Name = "測試站", Image = "Resources/test.png" };
            Person child1 = new Person() { Name = "組裝", Image = "Resources/pack.png" };
            Person child2 = new Person() { Name = "包裝", Image = "Resources/pack.png" };
            Person child3 = new Person() { Name = "SMT投入", Image = "Resources/process.png" };
            Person child4 = new Person() { Name = "熱管投入", Image = "Resources/process.png" };
            Person child5 = new Person() { Name = "FQC測試", Image = "Resources/test.png" };
            Person child6 = new Person() { Name = "OQC測試", Image = "Resources/test.png" };
            Person child7 = new Person() { Name = "維修", Image = "Resources/test.png" };
            Person child8 = new Person() { Name = "收料", Image = "Resources/process.png" };
            Person child9 = new Person() { Name = "DIP", Image = "Resources/process.png" };
            Person child10 = new Person() { Name = "燒錄", Image = "Resources/process.png" };
            Person child11 = new Person() { Name = "成品", Image = "Resources/pack.png" };
            Person child12 = new Person() { Name = "半成品", Image = "Resources/pack.png" };
            Person child13 = new Person() { Name = "可靠性測試", Image = "Resources/test.png" };

            //pack
            person1.Children.Add(child1);
            person1.Children.Add(child2);
            person1.Children.Add(child11);
            person1.Children.Add(child12);
            persons.Add(person1);

            //process
            person2.Children.Add(child3);
            person2.Children.Add(child4);
            person2.Children.Add(child8);
            person2.Children.Add(child9);
            person2.Children.Add(child10);
            persons.Add(person2);

            //test
            person3.Children.Add(child5);
            person3.Children.Add(child6);
            person3.Children.Add(child7);
            person3.Children.Add(child13);
            persons.Add(person3);
            trvPersons.ItemsSource = persons;
            
        }

        public MainWindowViewModel ViewModel
        {
            get
            {
                return (MainWindowViewModel)this.DataContext;
            }
        }

        //新增開始和結束的Node
        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            node_count = 1;
            node_one = 1;
            this.ViewModel.Network = new NetworkViewModel();
            this.ViewModel.CreateNode("*開始", new Point(10, 10), "Resources/switch-on.png");
            this.ViewModel.CreateNode("*結束", new Point(100, 10), "Resources/switch-off.png");
            this.MI_Conn.IsEnabled = true;
            this.MI_Delete.IsEnabled = true;
            var nodesCopy = this.ViewModel.Network.Nodes.ToArray();
            foreach (var node in nodesCopy)
            {
                if (node.Name == "*開始")
                {
                    node_source = node;
                }
            }
            node_dest = new NodeViewModel();
            LB_IN.Content = "";
            LB_Out.Content = "";
        }

        //新增Node 
        private void CreateNode_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.ViewModel.Network != null)
            {
                this.ViewModel.CreateNode("流程" + node_count, new Point(100 * node_count, 10),"");
                node_count += 1;
                var nodesCopy = this.ViewModel.Network.Nodes.ToArray();
                foreach (var node in nodesCopy)
                {
                    if (node.Name == "*結束")
                    {
                        node.X = (100 * node_count);
                    }
                }
            }
        }

        //連結
        private void ConnNode_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.ViewModel.Network != null)
            {
                LB_Nodes.Items.Clear();
                var nodesCopy = this.ViewModel.Network.Nodes.ToArray();
                foreach (var node in nodesCopy)
                {
                    if (node.IsSelected)
                    {
                        node_source = node;
                    }
                }
                if (node_source == null)
                {
                    return;
                }
                foreach (var node in nodesCopy)
                {
                    if (node.Name != node_source.Name && node.Source==0)
                    {
                        LB_Nodes.Items.Add(node.Name);
                    }
                }
            }
        }

        //點選List後連結
        private void LB_Nodes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var nodesCopy = this.ViewModel.Network.Nodes.ToArray();

            if (LB_Nodes.Items.Count > 0)
            {
                foreach (var node in nodesCopy)
                {
                    if (node.Name == LB_Nodes.SelectedItem.ToString())
                    {
                        node_dest = node;
                    }
                }
                this.ViewModel.ConnNode(node_source, node_dest);
                if (node_dest.Name == "*結束" && node_one ==1)
                {
                    LB_Out.Content = node_source.Name.ToString();
                    node_one = 2;
                }
                LB_Nodes.Items.Clear();
            }

        }

        //連結
        private void ConnNode2_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.ViewModel.Network != null)
            {
                LB_Nodes.Items.Clear();
                var nodesCopy = this.ViewModel.Network.Nodes.ToArray();
                foreach (var node in nodesCopy)
                {
                    if (node.IsSelected)
                    {
                        node_source = node;
                    }
                }
                if (node_source == null)
                {
                    return;
                }
                foreach (var node in nodesCopy)
                {
                    if (node.Name != node_source.Name)
                    {
                        LB_Nodes.Items.Add(node.Name);
                    }
                }
            }
        }

        //刪除選擇的Node 
        private void DeleteSelectedNodes_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.ViewModel.Network != null)
            {
                this.ViewModel.DeleteSelectedNodes();
            }

        }

        //儲存設定
        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {

            if (this.ViewModel.Network != null)
            {
                //Node設定
                var nodes = new List<Node>();
                var nodesCopy = this.ViewModel.Network.Nodes.ToArray();
                foreach (var node in nodesCopy)
                {
                    Node item = new Node
                    {
                        Name = node.Name,
                        X = node.X,
                        Y = node.Y,
                        Image = node.Image
                    };
                    nodes.Add(item);
                }

                var connections = new List<Connection>();
                var ConnectionsCopy = this.ViewModel.Network.Connections.ToArray();
                node_one = 1;
                foreach (var connection in ConnectionsCopy)
                {
                    Connection item = new Connection
                    {
                        Source = connection.SourceConnector.ParentNode.Name,
                        Dest = connection.DestConnector.ParentNode.Name
                    };
                    item.One = node_one.ToString();
                    if (item.Dest == "*結束" && node_one == 1)
                    {
                        node_one = 2;
                    }
                    connections.Add(item);
                }

                string filepath = $@"./Node.xlsx";

                XLWorkbook workbook = new XLWorkbook();
                var sheet = workbook.Worksheets.Add("Report");
                int rowIdx = 2;
                foreach (var item in nodes)
                {
                    int conlumnIndex = 1;
                    foreach (var jtem in item.GetType().GetProperties())
                    {
                        sheet.Cell(rowIdx, conlumnIndex).Value = string.Concat("'", Convert.ToString(jtem.GetValue(item, null)));
                        conlumnIndex++;
                    }
                    rowIdx++;
                }
                workbook.SaveAs(filepath);

                //Connection設定
                string filepath2 = $@"./Connection.xlsx";
                XLWorkbook workbook2 = new XLWorkbook();
                var sheet2 = workbook2.Worksheets.Add("Report");
                int rowIdx2 = 2;
                foreach (var item in connections)
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
            }

        }

        //讀取設定
        private void Btn_Load_Click(object sender, RoutedEventArgs e)
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
                        Y = Double.Parse(ws.Cell(i, 4).Value.ToString())
                    };
                    nodes.Add(item);
                }

            }

            node_count = 1;
            this.ViewModel.Network = new NetworkViewModel();
            this.MI_Conn.IsEnabled = true;
            this.MI_Delete.IsEnabled = true;
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
            node_one = 1;
            LB_IN.Content = "";
            LB_Out.Content = "";
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
                if (node_source.Name == "*開始" && node_one == 1)
                {
                    LB_IN.Content = node_dest.Name.ToString();
                }
                if (node_dest.Name == "*結束" && node_one == 1)
                {
                    LB_Out.Content = node_source.Name.ToString();
                    node_one = 2;
                }
            }
            MessageBox.Show("已讀取", "訊息", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //排序
        private void Btn_Order_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.Network != null)
            {
                //Node設定
                var nodes = new List<Node>();
                var nodesCopy = this.ViewModel.Network.Nodes.ToArray();
                foreach (var node in nodesCopy)
                {
                    Node item = new Node
                    {
                        Name = node.Name,
                        X = node.X,
                        Y = node.Y,
                        Image = node.Image
                    };
                    nodes.Add(item);
                }

                var connections = new List<Connection>();
                var ConnectionsCopy = this.ViewModel.Network.Connections.ToArray();
                node_one = 1;
                foreach (var connection in ConnectionsCopy)
                {
                    Connection item = new Connection
                    {
                        Source = connection.SourceConnector.ParentNode.Name,
                        Dest = connection.DestConnector.ParentNode.Name
                    };
                    item.One = node_one.ToString();
                    if (item.Dest == "*結束" && node_one == 1)
                    {
                        node_one = 2;
                    }
                    connections.Add(item);
                }

                var orders = new List<ORDER>();
                int id;
                id = 1;
                foreach (var conn in connections)
                {
                    if(conn.One.ToString() == "1")
                    {
                        ORDER item = new ORDER
                        {
                            Name = conn.Source,
                            Order = id,
                        };
                        orders.Add(item);
                        id += 1;
                    }
                    if(conn.Dest =="*結束" && conn.One.ToString() == "1")
                    {
                        ORDER item = new ORDER
                        {
                            Name = conn.Dest,
                            Order = id,
                        };
                        orders.Add(item);
                        id += 1;
                    }
                }

                foreach (var node in nodes)
                {
                    foreach (var order in orders)
                    {
                        if(node.Name == order.Name)
                        {
                            switch (order.Order)
                            {
                                case 1:
                                    node.X = 10 ;
                                    break;
                                case int n when (n > 1 && n < 10):
                                    node.X = (n * 100)-100;
                                    break;
                                case 10:
                                    node.X = 10;
                                    node.Y = 100;
                                    break;
                                case int n when (n > 10 && n < 20):
                                    node.X = ((n-9) * 100) - 100;
                                    node.Y = 100;
                                    break;
                                case 20:
                                    node.X = 10;
                                    node.Y = 200;
                                    break;
                                case int n when (n > 20 && n < 30):
                                    node.X = ((n - 19) * 100) - 100;
                                    node.Y = 200;
                                    break;
                                case 30:
                                    node.X = 10;
                                    node.Y = 300;
                                    break;
                                case int n when (n > 30 && n < 40):
                                    node.X = ((n - 29) * 100) - 100;
                                    node.Y = 300;
                                    break;
                            }
                        }
                    }
                }
                node_count = 1;
                this.ViewModel.Network = new NetworkViewModel();

                this.MI_Conn.IsEnabled = true;
                this.MI_Delete.IsEnabled = true;

                foreach (var node in nodes)
                {
                    this.ViewModel.CreateNode(node.Name, new Point(node.X, node.Y),node.Image);
                }

                foreach (var item in connections)
                {
                    var nodes_order = this.ViewModel.Network.Nodes.ToArray();
                    foreach (var node in nodes_order)
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
                MessageBox.Show("排列完成", "訊息", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //存成圖片
        private void Btn_Pic_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.Network != null)
            {
                ImageBrush Mybrush = new ImageBrush();
                Mybrush.ImageSource = new BitmapImage(new Uri(@"from.png", UriKind.Relative));
                Canvas_pic.Background = Mybrush;
                Size tSize = new Size(Canvas_pic.Width, Canvas_pic.Height);
                Canvas_pic.Measure(tSize);
                DrawingVisual tDrawingVisual = new DrawingVisual();
                using (DrawingContext context = tDrawingVisual.RenderOpen())
                {

                    VisualBrush tVisualBrush = new VisualBrush(Canvas_pic);
                    tVisualBrush.Stretch = Stretch.Fill;
                    context.DrawRectangle(tVisualBrush, null, new Rect(0, 0, 1024, 768));
                    context.Close();
                }

                RenderTargetBitmap tRenderTargetBitmap = new RenderTargetBitmap((int)Canvas_pic.RenderSize.Width, (int)Canvas_pic.RenderSize.Height, 96d, 96d, PixelFormats.Default);
                tRenderTargetBitmap.Render(tDrawingVisual);

                using (FileStream tFileStream = new FileStream(@"to.png", FileMode.Create, FileAccess.Write))
                {
                    PngBitmapEncoder tPngBitmapEncoder = new PngBitmapEncoder();
                    tPngBitmapEncoder.Interlace = PngInterlaceOption.On;
                    tPngBitmapEncoder.Frames.Add(BitmapFrame.Create(tRenderTargetBitmap));
                    tPngBitmapEncoder.Save(tFileStream);
                    tFileStream.Close();
                }
                MessageBox.Show("已存成圖片", "訊息", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //樹狀清單
        public class Person : TreeViewItemBase
        {
            public Person()
            {
                this.Children = new ObservableCollection<Person>();
            }

            public string Name { get; set; }
            public string Image { get; set; }
            public ObservableCollection<Person> Children { get; set; }
        }

        //點樹狀產生新Node
        private void trvPersons_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            foreach (var person in persons)
            {
                foreach (var children in person.Children)
                {
                    if (children.IsSelected == true)
                    {
                        if (this.ViewModel.Network != null)
                        {
                            int check_node;
                            check_node = 1;
                            var nodesList = this.ViewModel.Network.Nodes.ToArray();
                            foreach (var node in nodesList)
                            {
                                if (node.Name == children.Name)
                                {
                                    check_node =2;
                                }
                            }
                            if (check_node==1)
                            {
                                this.ViewModel.CreateNode(children.Name, new Point(100 * node_count, 10), children.Image);
                                node_count += 1;
                                var nodesCopy = this.ViewModel.Network.Nodes.ToArray();
                                foreach (var node in nodesCopy)
                                {
                                    if (node.Name == "*結束")
                                    {
                                        node.X = (100 * node_count);
                                    }
                                }
                                //第一條流程未結束前自動連結
                                if (node_dest == null)
                                {
                                    foreach (var node in nodesCopy)
                                    {
                                        if (node.Name == node_source.Name)
                                        {
                                            node_source = node;
                                        }
                                        if (node.Name == children.Name)
                                        {
                                            node_dest = node;
                                        }
                                    }
                                    this.ViewModel.ConnNode(node_source, node_dest);
                                    if (node_source.Name == "*開始")
                                    {
                                        LB_IN.Content = node_dest.Name.ToString();
                                    }
                                    node_source = node_dest;
                                }
                                else
                                {
                                    if (node_dest.Name != "*結束")
                                    {
                                        foreach (var node in nodesCopy)
                                        {
                                            if (node.Name == node_source.Name)
                                            {
                                                node_source = node;
                                            }
                                            if (node.Name == children.Name)
                                            {
                                                node_dest = node;
                                            }
                                        }
                                        this.ViewModel.ConnNode(node_source, node_dest);
                                        if (node_source.Name == "*開始")
                                        {
                                            LB_IN.Content = node_dest.Name.ToString();
                                        }
                                        node_source = node_dest;
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        //樹狀清單選擇
        public class TreeViewItemBase : INotifyPropertyChanged
        {
            private bool isSelected;
            public bool IsSelected
            {
                get { return this.isSelected; }
                set
                {
                    if (value != this.isSelected)
                    {
                        this.isSelected = value;
                        NotifyPropertyChanged("IsSelected");
                    }
                }
            }

            private bool isExpanded;
            public bool IsExpanded
            {
                get { return this.isExpanded; }
                set
                {
                    if (value != this.isExpanded)
                    {
                        this.isExpanded = value;
                        NotifyPropertyChanged("IsExpanded");
                    }
                }
            }


            public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged(string propName)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }

    }
}


