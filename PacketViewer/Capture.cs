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
                if (this.device == null)
                    return false;
                return this.device.Started;
            }
        }

        public Capture(MainWindow mainWindow)
        {
            this.MainWindow = mainWindow;
        }

        public IEnumerable<string> GetDevices()
        {
            foreach (ICaptureDevice dev in CaptureDeviceList.Instance)
            {
                yield return dev.Description;
            }
        }

        public void StartCapture(string deviceName, string ip)
        {
            this.device = null;
            if (deviceName == "" || ip == "")
                return;

            try
            {
                this.device = CaptureDeviceList.Instance.Where(dev => dev.Description == deviceName).Select(dev => dev).First();
            }
            catch
            {
                MessageBox.Show("DEVICE FAILED TO INITIALIZE", "WRONG DEVICE!", 0, MessageBoxImage.Error);
                throw;
            }

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

                    while (this.MainWindow.pp.ProcessClientData()) { };
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
                    while (this.MainWindow.pp.ProcessServerData()) { };
                }

            }
        }
    }
}
