using System;
using System.Collections;
using System.Collections.ObjectModel;
using PacketDotNet;
using PacketDotNet.Utils;
using Network.Crypto;
using SharpPcap;

namespace Network.PacketHandler
{
    public class PacketProcessor
    {
        public ObservableCollection<Packet> Packets { get; set; }

        private Capture Capture;

        private Session SecSession;
        private int State;

        private byte[] ServerBuffer;
        private byte[] ClientBuffer;


        public PacketProcessor(Capture capture)
        {
            this.Capture = capture;
            this.Packets = new ObservableCollection<Packet>();
            this.Capture.Device.OnPacketArrival += new PacketArrivalEventHandler(this.OnPacketArrival);

            this.SecSession = new Session();
            this.State = -1;

            this.ServerBuffer = new byte[0];
            this.ClientBuffer = new byte[0];
        }

        private void OnPacketArrival(object sender, CaptureEventArgs eCap)
        {
            ByteArraySegment raw = new ByteArraySegment(eCap.Packet.Data);
            EthernetPacket ethernetPacket = new EthernetPacket(raw);
            if (ethernetPacket.Type == EthernetPacketType.IpV4)
            {
                IpPacket ipPacket = (IpPacket)ethernetPacket.PayloadPacket;
                if (ipPacket.Protocol.ToString() == "TCP")
                {
                    TcpPacket tcp = (TcpPacket)ipPacket.PayloadPacket;
                    if (ipPacket != null && tcp != null)
                    {
                        if (this.Capture.CaptureIp == ipPacket.DestinationAddress.ToString())
                        {
                            // Client -> Server*
                            this.AppendClientData(tcp.PayloadData);
                            while (this.ProcessClientData()) { };

                        }
                        else if (this.Capture.CaptureIp == ipPacket.SourceAddress.ToString())
                        {
                            // Do a check for a new game Connection. Each handshake starts with a dword 1 packet from the server.
                            byte[] test = new byte[4] { 0x01, 0x00, 0x00, 0x00 };
                            if (StructuralComparisons.StructuralEqualityComparer.Equals(test, tcp.PayloadData))
                            {
                                // New Connection detected. 
                                // We should reset crypto.
                                this.SecSession = new Session();
                                this.State = 0;

                                this.ServerBuffer = new byte[0];
                                this.ClientBuffer = new byte[0];
                            }

                            // Server -> Client
                            this.AppendServerData(tcp.PayloadData);
                            while (this.ProcessServerData()) { };
                        }
                        else return;
                    }
                }
            }

        }

        private void AppendServerData(byte[] data)
        {
            if (State == 2)
                SecSession.Encrypt(ref data);

            Array.Resize(ref ServerBuffer, ServerBuffer.Length + data.Length);
            Array.Copy(data, 0, ServerBuffer, ServerBuffer.Length - data.Length, data.Length);
        }

        private byte[] GetServerData(int length)
        {
            byte[] result = new byte[length];
            Array.Copy(ServerBuffer, result, length);

            byte[] reserve = (byte[])ServerBuffer.Clone();
            ServerBuffer = new byte[ServerBuffer.Length - length];
            Array.Copy(reserve, length, ServerBuffer, 0, ServerBuffer.Length);

            return result;
        }

        private void AppendClientData(byte[] data)
        {
            if (State == 2)
                SecSession.Decrypt(ref data);

            Array.Resize(ref ClientBuffer, ClientBuffer.Length + data.Length);
            Array.Copy(data, 0, ClientBuffer, ClientBuffer.Length - data.Length, data.Length);
        }

        private byte[] GetClientData(int length)
        {
            byte[] result = new byte[length];
            Array.Copy(ClientBuffer, result, length);

            byte[] reserve = (byte[])ClientBuffer.Clone();
            ClientBuffer = new byte[ClientBuffer.Length - length];
            Array.Copy(reserve, length, ClientBuffer, 0, ClientBuffer.Length);

            return result;
        }

        private bool ProcessServerData()
        {
            switch (State)
            {
                case -1:
                    //Garbage. Drop it.
                    GetServerData(ServerBuffer.Length);
                    return false;
                case 0:
                    if (ServerBuffer.Length < 128)
                    {
                        //First Dword 1 Options Packet. Ignore it.
                        GetServerData(ServerBuffer.Length);
                        return false;
                    }

                    SecSession.ServerKey1 = GetServerData(128);
                    State++;
                    return true;
                case 1:
                    if (ServerBuffer.Length < 128)
                        return false;
                    SecSession.ServerKey2 = GetServerData(128);
                    SecSession.Init();
                    State++;
                    return true;
            }

            if (ServerBuffer.Length < 4)
                return false;

            int length = BitConverter.ToUInt16(ServerBuffer, 0);

            if (ServerBuffer.Length < length)
                return false;

            ushort opCode = BitConverter.ToUInt16(ServerBuffer, 2);
            byte[] data = GetServerData(length);

            //if ((opCode < (ushort)0xFFF) && (data.LongLength < 4)) // Dirty fix for avoid null/corrupted packet
                //return false;

            Packets.Add(new Packet(PacketSource.Server, data));

            return false;
        }

        private bool ProcessClientData()
        {
            switch (State)
            {
                case -1:
                    //Garbage. Drop it.
                    GetClientData(ClientBuffer.Length);
                    return false;

                case 0:
                    if (ClientBuffer.Length < 128)
                    {
                        // garbage from a running game session. we ignore it.
                        //If we open a hex File we handle it diffrent ;)
                        GetClientData(ClientBuffer.Length);
                        return false;
                    }

                    SecSession.ClientKey1 = GetClientData(128);
                    return true;
                case 1:
                    if (ClientBuffer.Length < 128)
                        return false;

                    SecSession.ClientKey2 = GetClientData(128);
                    return true;
            }

            if (ClientBuffer.Length < 4)
                return false;

            int length = BitConverter.ToUInt16(ClientBuffer, 0);

            if (ClientBuffer.Length < length)
                return false;

            ushort opCode = BitConverter.ToUInt16(ClientBuffer, 2);
            byte[] data = GetClientData(length);

            //if ((opCode < (ushort)0xFFF) && (data.LongLength < 4)) // Dirty fix for avoid null/corrupted packet
                //return false;

            this.Packets.Add(new Packet(PacketSource.Client, data));

            return false;
        }
    }
}
