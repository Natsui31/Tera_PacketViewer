using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PacketDotNet.Utils;
using SharpPcap;
using PacketDotNet;

namespace PacketViewer
{
    public class Capture
    {
        
        private ICaptureDevice device;
        private string captureIp;

        protected MainWindow MainWindow;

        public bool IsRunning
        {
            get
            {
                if (device == null)
                    return false;
                return device.Started;
            }
        }

        public Capture(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
        }

        public IEnumerable<string> GetDevices()
        {
            List<string> tmpList = new List<string>();
            CaptureDeviceList deviceList = CaptureDeviceList.Instance;

            foreach (ICaptureDevice device in deviceList)
            {
                yield return device.Description;
            }
        }

        public void StartCapture(string deviceName, string ip)
        {
            if (deviceName == "" || ip == "")
                return;
            if (this.IsRunning)
                MessageBox.Show("You shouldn't start capture another time.");
        
            CaptureDeviceList deviceList = CaptureDeviceList.Instance;
            
            foreach (var dev in deviceList)
            {
                if (dev.Description == deviceName)
                    this.device = dev;
            }

            if (this.device == null)
                MessageBox.Show("Device Fail");
   
            // Register our handler function to the
            // 'packet arrival' event
            this.device.OnPacketArrival += new SharpPcap.PacketArrivalEventHandler(device_OnPacketArrival);
            
            // Open the device for capturing
            int readTimeoutMilliseconds = 1000;
            this.device.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);

            //Filter
            string filter = "host " + ip;
            this.device.Filter = filter;
            this.captureIp = ip;

            // Start the capturing process
            this.device.StartCapture();
        }

        public void StopCapture()
        {
            this.device.StopCapture();
            this.device.Close();
            this.captureIp = "";
        }

        public void device_OnPacketArrival(object sender, CaptureEventArgs eCap)
        {
            ByteArraySegment raw = new ByteArraySegment(eCap.Packet.Data);
            EthernetPacket ethernetPacket = new EthernetPacket(raw);
            IpPacket ipPacket = (IpPacket)ethernetPacket.PayloadPacket;
            TcpPacket tcp = (TcpPacket)ipPacket.PayloadPacket;

            if (ipPacket != null && tcp != null)
            {
                string destIp = ipPacket.DestinationAddress.ToString();
                
                  
                if (destIp == captureIp)
                {
                    //Client -> Server
                    this.MainWindow.pp.AppendClientData(tcp.PayloadData);
                    
                    // ReSharper disable CSharpWarnings::CS0642
                    while (this.MainWindow.pp.ProcessClientData()) ;
                    // ReSharper restore CSharpWarnings::CS0642
                }
                else
                {
                    //Do a check for a new game Connection. Each handshake starts with a dword 1 packet from the server.
                    byte[] test = new byte[4] {0x01, 0x00, 0x00, 0x00};
                    if (StructuralComparisons.StructuralEqualityComparer.Equals(test, tcp.PayloadData))
                    {
                        //New Connection detected. 
                        //We should reset State and Security Info
                        this.MainWindow.pp.Init();
                        this.MainWindow.pp.State = 0;
                        this.MainWindow.ClearPackets();
                    }

                    //Sever -> Client
                    this.MainWindow.pp.AppendServerData(tcp.PayloadData);
                    // ReSharper disable CSharpWarnings::CS0642
                    while (this.MainWindow.pp.ProcessServerData()) ;
                    // ReSharper restore CSharpWarnings::CS0642
                }

            }
        }
    }
}
