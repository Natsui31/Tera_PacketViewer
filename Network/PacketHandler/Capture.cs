using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net;

using SharpPcap;
using Utils;


namespace Network.PacketHandler
{
    /// <summary>
    /// Class for capture device.
    /// </summary>
    public class Capture
    {
        /// <summary>
        /// 
        /// </summary>
        internal string CaptureIp;

        /// <summary>
        /// PacketProcessor properties
        /// </summary>
        public PacketProcessor PacketProcessor { get; private set; }

        /// <summary>
        /// Interface for capture device.
        /// </summary>
        public ICaptureDevice Device { get; private set; }

        /// <summary>
        /// Kernel level filtering expression associated with this device.
        /// For more info on filter expression syntax, see:
        /// https://www.winpcap.org/docs/docs_40_2/html/group__language.html
        /// </summary>
        public string Filter
        {
            get
            {
                return this.Device.Filter;
            }
            set
            {
                this.Device.Filter = value;
            }
        }

        /// <summary>
        /// Return a value indicating if the capturing process of this device is started.
        /// </summary>
        public bool IsRunning { get { return this.Device.Started; } }

        public Capture(string deviceName, int read_timeout = 1000)
        {
            this.Device = CaptureDeviceList.Instance.Where(device => device.Description == deviceName).Select(device => device).First();
            
            this.Device.Open(DeviceMode.Promiscuous, read_timeout);
            this.PacketProcessor = new PacketProcessor(this);
        }

        public void Start()
        {
            this.Device.StartCapture();
        }

        public void Stop()
        {
            this.Device.StopCapture();
        }

        public void SetFilter(string ipDest)
        {
            this.Filter = "";
            IPAddress.Parse(ipDest); // Lazy Check IP
            this.CaptureIp = ipDest.ToString();
            this.Filter = String.Format("host {0}", ipDest);
        }

        public void SetFilter(string ipDest, string portDest)
        {
            this.Filter = "";
            IPAddress.Parse(ipDest); // Lazy Check IP
            this.CaptureIp = ipDest;
            this.Filter = String.Format("host {0} and port {1}", ipDest, portDest);
        }

        public static IEnumerable<string> GetDevicesName()
        {
            foreach (var device in CaptureDeviceList.Instance)
                yield return device.Description;
        }
    }
}
