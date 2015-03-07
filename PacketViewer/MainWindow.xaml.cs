using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using Crypt;
using Data.Structures.Quest;
using Microsoft.Win32;
using PacketDotNet;
using PacketDotNet.Utils;
using PacketViewer;
using Network;
using SharpPcap;
using Utils;

//Base by Cerium Unity. Edit by GoneUp. 21.02.2014

namespace PacketViewer
{
    public partial class MainWindow
    {
        public Capture cap;
        public PacketProcessor pp;

        public MainWindow()
        {
            InitializeComponent();
            this.Title = String.Format("Tera PacketViewer v{0}.{1}.{2}", Version.GetVersion.Major, Version.GetVersion.Minor, Version.GetVersion.Build);
            
            foreach (var opcode in Enum.GetNames(typeof(OpcodeFlags)))
                this.PacketNamesList.Items.Add(opcode);

            /*
            this.PacketNamesList.Items.Add(new Separator
                                          {
                                              HorizontalAlignment = HorizontalAlignment.Stretch,
                                              IsEnabled = false
                                          });
            */

            this.PacketNamesList.SelectedIndex = 0;


            this.pp = new PacketProcessor(this);
            this.cap = new Capture(this);

            foreach (var nic in cap.GetDevices())
            {
                this.BoxNic.Items.Add(nic);
            }
            this.BoxNic.SelectedValue = this.BoxNic.Items[0];

            // ReSharper disable ObjectCreationAsStatement
            //new FindAllNpcs(this);
            //new FindAllTraidlists(this);
            //new FindAllGatherables(this);
            //new FindAllClimbs(this);
            //new FindAllCampfires(this);
            // ReSharper restore ObjectCreationAsStatement
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        public void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog {Filter = "*.hex|*.hex"};

            if (openFileDialog.ShowDialog() == false)
                return;

            this.pp.Init();
            this.pp.OpenFileMode = true;

            this.PacketsList.Items.Clear();

            using (FileStream fileStream = File.OpenRead(openFileDialog.FileName))
            {
                using (TextReader stream = new StreamReader(fileStream))
                {
                    while (true)
                    {
                        string line = stream.ReadLine();
                        if (line == null)
                            break;
                        if (line.Length == 0)
                            continue;
                        if (this.pp.State == -1)
                        {
                            this.pp.State = 0;
                            continue;
                        }

                        PacketType type = (line[0] == ' ') ? PacketType.Server : PacketType.Client;

                        string hex = line.Substring(type == PacketType.Server ? 14 : 10, 49).Replace(" ", "");
                        byte[] data = hex.ToBytes();
                        
                        if (type == PacketType.Server)
                        {
                            this.pp.AppendServerData(data);
                            while (this.pp.ProcessServerData()) { };
                        }
                        else if (type == PacketType.Client)
                        {
                            this.pp.AppendClientData(data);
                            while (this.pp.ProcessClientData()) { };
                        }
                    }
                }
            }

            SetText("Loaded " + this.pp.Packets.Count + " packets...");
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ClearPackets()
        {
            Dispatcher.BeginInvoke(
                new Action(
                    delegate
                    {
                        this.PacketsList.Items.Clear();
                    }));
        }

        public void AppendPacket(Color col, string itemText)
        {
            Dispatcher.BeginInvoke(
                new Action(
                    delegate
                    {
                        ListBoxItem item = new ListBoxItem
                        {
                            Content = itemText,
                            Background = new SolidColorBrush(col)
                         };

                        this.PacketsList.Items.Add(item);

                        this.PacketsList.ScrollIntoView(item);
                    }));
        }

        public void SetHex(string text)
        {
            Dispatcher.BeginInvoke(
                new Action(
                    delegate
                    {
                        this.HexTextBox.Document.Blocks.Clear();
                        this.HexTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
                    }));
        }

        public void AppendHex(string text)
        {
            Dispatcher.BeginInvoke(
                new Action(
                    delegate
                    {
                        this.HexTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
                    }));
        }

        public void SetText(string text)
        {
            Dispatcher.BeginInvoke(
                new Action(
                    delegate
                        {
                            this.TextBox.Document.Blocks.Clear();
                            this.TextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
                        }));
        }

        private void OnPacketSelect(object sender, SelectionChangedEventArgs e)
        {
            if (PacketsList.SelectedIndex == -1)
                return;

            this.SetHex(pp.Packets[PacketsList.SelectedIndex].Hex);
            this.SetText(pp.Packets[PacketsList.SelectedIndex].Text);

            this.OpCodeBox.Text = this.pp.Packets[PacketsList.SelectedIndex].Hex.Substring(0, 4);
        }

        private void FindByName(object sender, RoutedEventArgs e)
        {
            if (this.pp.Packets == null)
                return;

            string name = this.PacketNamesList.SelectedItem.ToString();

            for (int i = this.PacketsList.SelectedIndex + 1; i < this.pp.Packets.Count; i++)
            {
                if (this.pp.Packets[i].Name == name)
                {
                    this.PacketsList.SelectedIndex = i;
                    this.PacketsList.ScrollIntoView(this.PacketsList.SelectedItem);
                    return;
                }
            }

            if (MessageBox.Show("Find from start?", "Not found", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            for (int i = 0; i < this.PacketsList.SelectedIndex; i++)
            {
                if (this.pp.Packets[i].Name == name)
                {
                    this.PacketsList.SelectedIndex = i;
                    this.PacketsList.ScrollIntoView(this.PacketsList.SelectedItem);
                    return;
                }
            }
        }

        private void FindByHex(object sender, RoutedEventArgs e)
        {
            if (this.pp.Packets == null)
                return;

            string hex = this.HexBox.Text.Replace(" ", "");

            for (int i = this.PacketsList.SelectedIndex + 1; i < this.pp.Packets.Count; i++)
            {
                if (this.pp.Packets[i].Hex.IndexOf(hex, 4, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    this.PacketsList.SelectedIndex = i;
                    this.PacketsList.ScrollIntoView(PacketsList.SelectedItem);
                    return;
                }
            }

            if (MessageBox.Show("Find from start?", "Not found", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            for (int i = 0; i < this.PacketsList.SelectedIndex; i++)
            {
                if (this.pp.Packets[i].Hex.IndexOf(hex, 4, StringComparison.OrdinalIgnoreCase) != -1)
                {
                    this.PacketsList.SelectedIndex = i;
                    this.PacketsList.ScrollIntoView(PacketsList.SelectedItem);
                    return;
                }
            }
        }

        private void FindByOpCode(object sender, RoutedEventArgs e)
        {
            if (this.pp.Packets == null)
                return;

            string hex = OpCodeBox.Text.Replace(" ", "");

            for (int i = this.PacketsList.SelectedIndex + 1; i < this.pp.Packets.Count; i++)
            {
                if (this.pp.Packets[i].Hex.IndexOf(hex, 0, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    this.PacketsList.SelectedIndex = i;
                    this.PacketsList.ScrollIntoView(PacketsList.SelectedItem);
                    return;
                }
            }

            if (MessageBox.Show("Find from start?", "Not found", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            for (int i = 0; i < PacketsList.SelectedIndex; i++)
            {
                if (this.pp.Packets[i].Hex.IndexOf(hex, 0, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    this.PacketsList.SelectedIndex = i;
                    this.PacketsList.ScrollIntoView(PacketsList.SelectedItem);
                    return;
                }
            }
        }

        private void CaptureMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if ((string)this.CaptureMenuItem.Header == "Start Capture")
            {
                if (this.BoxNic.SelectedValue == null)
                    return;
                if (this.cap.IsRunning)
                    this.cap.StopCapture();

                this.pp.Init();
                ClearCaptureMenuItem_Click(null, null);             

                string nic_des = (string)this.BoxNic.SelectedValue;
                string senderIp = ((string)this.BoxServers.Text).Split(';')[0];

                this.cap.StartCapture(nic_des, senderIp);
                this.BoxNic.IsEnabled = false;
                this.BoxServers.IsEnabled = false;
                this.CaptureMenuItem.Header = (object)"Stop Capture";
            }
            else
            {
                this.cap.StopCapture();
                this.BoxNic.IsEnabled = true;
                this.BoxServers.IsEnabled = true;
                this.CaptureMenuItem.Header = (object)"Start Capture";
            }
        }

        private void ClearCaptureMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.cap.IsRunning)
                this.pp.Packets.Clear();
            this.PacketsList.Items.Clear();

            this.OpCodeBox.Text = "Find OpCode";
            this.HexBox.Text = "Find Hex";
            this.SetHex("");
            this.SetText("");
        }

        private void AboutThis_Click(object sender, RoutedEventArgs e)
        {
            var aboutThisForm = new AboutBox();
            aboutThisForm.ShowDialog();
        }
    }
}
