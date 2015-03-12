using System;
using System.ComponentModel;
using System.Globalization;
using Utils;

namespace Network.PacketHandler
{
    /// <summary>
    /// Packet Type options list. 
    /// </summary>
    /// <remarks>We use this to distinguish packets send by Client or Server.</remarks>
    public enum PacketSource
    {
        Client = 0,
        Server = 1,
    };

    public struct Packet
    {

        /// <summary>
        /// The <see cref="PacketSource"/> of the packet.
        /// </summary>
        public readonly PacketSource Source;

        /// <summary>
        /// The data in the packet.
        /// </summary>
        public readonly byte[] Data;

        /// <summary>
        /// The opcode of the packet.
        /// </summary>
        private readonly ushort Opcode;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="data"></param>
        /// <param name="opcode"></param>
        public Packet(PacketSource source, byte[] data)
        {
            this.Source = source;
            this.Data = data;

            var hex = data.ToHex().Substring(4, 4);
            this.Opcode = ushort.Parse(hex.Substring(2, 2) + hex.Substring(0, 2), NumberStyles.HexNumber);
           
        }

        public new string ToString()
        {
            string text = "";
            text += String.Format("Opcode : {0} \n", GetOpcodeName());
            text += String.Format("Type : {0}  \n ", this.Source);
            text += "\n";
            text += Data.FormatHex();
            return text;
        }

        /// <summary>
        /// Get Opcode as 0x0000 format.
        /// </summary>
        public string GetOpcode()
        {
            return String.Format("0x{0}", this.Opcode.ToString("X4"));
        }

        /// <summary>
        /// Get OpcodeName from OpCode enum.
        /// </summary>
        public string GetOpcodeName()
        {
            if (Enum.IsDefined(typeof(OpCode), this.Opcode))
                return Enum.GetName(typeof(OpCode), this.Opcode);
            else
                return String.Format("0x{0}", this.Opcode.ToString("X4"));
        }
    }
}
