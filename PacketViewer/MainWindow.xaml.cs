using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
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
using Network.PacketHandler;
using Utils;

namespace PacketViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Capture Capture;
        private ObservableCollection<Packet> Packets;
        
        public MainWindow()
        {
            InitializeComponent();
            this.Title = String.Format("Tera PacketViewer v{0}.{1}", Version.GetVersion.Major, Version.GetVersion.Minor);
            this.DataContext = this;

            foreach (var device in Capture.GetDevicesName())
                DeviceListBox.Items.Add(device);

            foreach (var elem in Enum.GetNames(typeof(OpCode)).OrderBy(e => e))
                this.OpcodeListBox.Items.Add(elem);

            this.ServerListBox.SelectedIndex = 0;
            this.DeviceListBox.SelectedIndex = 0;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Dispatcher.BeginInvokeShutdown(System.Windows.Threading.DispatcherPriority.Send);
            if (this.Capture != null)
                if (this.Capture.IsRunning)
                    this.Capture.Stop();
            this.Capture = null;
        }

        private void ClearGUI()
        {
            // Clear GUI
                if (this.Capture != null)
                    this.Packets.Clear();
                this.PacketsListBox.Items.Clear();
                this.FindHexBox.Text = "Find Hex";
                this.FindOpcodeBox.Text = "Find Opcode";
                this.HexViewTextBox.Document.Blocks.Clear();
                this.DataViewTextBox.Document.Blocks.Clear();
        }

        private void ExitMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CaptureMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if ((string)CaptureMenuItem.Header == "Start Capture")
            {
                if (this.Capture != null)
                    this.ClearGUI(); // Clear GUI

                CaptureMenuItem.Header = "Stop Capture";
                this.ServerListBox.IsEnabled = false;
                this.DeviceListBox.IsEnabled = false;

                // Create new class with Selected Device
                this.Capture = new Capture((string)DeviceListBox.SelectedItem);
                this.Packets = this.Capture.PacketProcessor.Packets;

                // Define Filters for Capture Process
                string serverBox = this.ServerListBox.SelectionBoxItem.ToString();
                if (serverBox.Contains(':'))
                {
                    string[] splittedString = serverBox.Split(':');
                    IPAddress.Parse(splittedString[0]); // Lazy Check IP
                    this.Capture.SetFilter(splittedString[0], splittedString[1]);
                }
                else
                {
                    if (serverBox.Contains('.'))
                        IPAddress.Parse(serverBox); // Lazy Check IP
                    else
                        try { serverBox = ServerTera.List.Where(elem => elem.Key == serverBox).Select(elem => elem.Value).First(); }
                        catch { }
                    this.Capture.SetFilter(serverBox);
                }

                // Set Event
                this.Packets.CollectionChanged += new NotifyCollectionChangedEventHandler(Packet_Notifier);

                // Start Capture on Device
                this.Capture.Start();

            }
            else if ((string)CaptureMenuItem.Header == "Stop Capture")
            {
                CaptureMenuItem.Header = "Start Capture";
                this.ServerListBox.IsEnabled = true;
                this.DeviceListBox.IsEnabled = true;

                // Stop Capture on Device
                this.Capture.Stop();
                this.Packets.CollectionChanged -= new NotifyCollectionChangedEventHandler(Packet_Notifier);
            }
            else throw new Exception("Dirty Thing happened"); // Avoid dirty thing.
        }

        private void NewServerAddress_OnList(object sender, RoutedEventArgs e)
        {
            string value = "127.0.0.1 or 127.0.0.1:11101";
            if (InputBox.Show("New Server Address", "Enter New Server Address:", ref value) == System.Windows.Forms.DialogResult.OK)
            {
                this.ServerListBox.Items.Add(value);
                this.ServerListBox.SelectedIndex = (this.ServerListBox.Items.Count - 1);
            }
        }

        private void Packet_Notifier(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action.ToString() != "Add")
                return;

            this.Dispatcher.BeginInvoke(new Action(delegate
                {
                    Packet packet = this.Packets[e.NewStartingIndex];
                    bool isClient = (packet.Source == PacketSource.Client);
                    Color color = isClient ? Colors.WhiteSmoke : Colors.LightBlue;
                    ListBoxItem item = new ListBoxItem
                    {
                        Content = (object)String.Format("[{0}] {1} [{2}]",
                                                        isClient ? "C" : "S",
                                                        packet.GetOpcodeName(),
                                                        packet.Data.Length),
                        Background = new SolidColorBrush(color),
                    };
                    this.PacketsListBox.Items.Add(item);
                }), System.Windows.Threading.DispatcherPriority.Normal);
        }

        private void OpcodeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate
                {
                    if (this.PacketsListBox.SelectedIndex == -1)
                        return;
                    if (this.Packets == null)
                        return;

                    // Get Packet on Packets Collection
                    var packet = this.Packets[this.PacketsListBox.SelectedIndex];

                    // Set Opcode on FindOpcodeBox
                    this.FindOpcodeBox.Text = packet.GetOpcode();

                    // Set Data as it on HexViewBox
                    this.HexViewTextBox.Document.Blocks.Clear();
                    this.HexViewTextBox.Document.Blocks.Add(new Paragraph(new Run(packet.Data.ToHex())));

                    // Set Packet on DataViewBox
                    this.DataViewTextBox.Document.Blocks.Clear();
                    this.DataViewTextBox.Document.Blocks.Add(new Paragraph(new Run(packet.ToString())));
                }));
        }

        private void SearchEnumButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Packets == null || this.Packets.Count == 0)
                return;
            string name = this.OpcodeListBox.SelectedItem.ToString();

            for (int i = this.PacketsListBox.SelectedIndex + 1; i < this.PacketsListBox.Items.Count; i++)
            {
                if (this.Packets[i].GetOpcodeName() == name)
                {
                    this.PacketsListBox.Focus();
                    this.PacketsListBox.SelectedIndex = i;
                    this.PacketsListBox.ScrollIntoView(this.PacketsListBox.SelectedItem);
                    return;
                }
            }

            if (MessageBox.Show("Find from start?", "Not found", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            for (int i = 0; i < this.PacketsListBox.SelectedIndex; i++)
            {
                if (this.Packets[i].GetOpcodeName() == name)
                {
                    this.PacketsListBox.Focus();
                    this.PacketsListBox.SelectedIndex = i;
                    this.PacketsListBox.ScrollIntoView(this.PacketsListBox.SelectedItem);
                    return;
                }
            }
        }

        private void SearchHexButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Packets == null || this.Packets.Count == 0)
                return;

            string hex = this.FindHexBox.Text.Replace(" ", "");

            for (int i = this.PacketsListBox.SelectedIndex + 1; i < this.Packets.Count; i++)
            {
                if (this.Packets[i].Data.ToHex().IndexOf(hex, 0, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    this.PacketsListBox.SelectedIndex = i;
                    this.PacketsListBox.ScrollIntoView(this.PacketsListBox.SelectedItem);
                    return;
                }
            }

            if (MessageBox.Show("Find from start?", "Not found", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            for (int i = 0; i < this.PacketsListBox.SelectedIndex; i++)
            {
                if (this.Packets[i].Data.ToHex().IndexOf(hex, 0, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    this.PacketsListBox.SelectedIndex = i;
                    this.PacketsListBox.ScrollIntoView(this.PacketsListBox.SelectedItem);
                    return;
                }
            }
        }

        private void SearchOpcodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Packets == null || this.Packets.Count == 0)
                return;

            string hex = this.FindOpcodeBox.Text.Replace(" ", "");

            for (int i = this.PacketsListBox.SelectedIndex + 1; i < this.Packets.Count; i++)
            {
                if (String.Compare(this.Packets[i].GetOpcode(), hex, true) == 0)
                {
                    this.PacketsListBox.SelectedIndex = i;
                    this.PacketsListBox.ScrollIntoView(this.PacketsListBox.SelectedItem);
                    return;
                }
            }

            if (MessageBox.Show("Find from start?", "Not found", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            for (int i = 0; i < this.PacketsListBox.SelectedIndex; i++)
            {
                var op = this.Packets[i].GetOpcode();
                if (String.Compare(op, hex, true) == 0)
                {
                    this.PacketsListBox.SelectedIndex = i;
                    this.PacketsListBox.ScrollIntoView(this.PacketsListBox.SelectedItem);
                    return;
                }
            }
        }

        private void AboutThis_Click(object sender, RoutedEventArgs e)
        {
            var aboutThisForm = new AboutBox();
            aboutThisForm.ShowDialog();
        }

        private void ClearCapture_Click(object sender, RoutedEventArgs e)
        {
            this.ClearGUI();
        }
    }
}