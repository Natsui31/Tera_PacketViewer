using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Network;
using Utils;

namespace PacketViewer
{
    public enum PacketType
    {
        Client = 0,
        Server = 1,
    };

    public class Packet
    {
        public PacketType Type { get; private set; }
        public string Name { get; private set; }
        public byte[] Data { get; private set; }
        public string Hex { get; private set; }
        public string Text { get; private set; }

        public Packet(PacketType type, ushort opCode, byte[] data)
        {
            this.Type = type;
            this.Data = data;

            if (Enum.IsDefined(typeof(OpcodeFlags), (object)opCode))
            {
                this.Name = ((OpcodeFlags)opCode).ToString();
            }
            else
            {
                string opCodeLittleEndianHex = BitConverter.GetBytes(opCode).ToHex();
                Name = string.Format("0x{0}{1}", opCodeLittleEndianHex.Substring(2),
                                     opCodeLittleEndianHex.Substring(0, 2));
            }

            Hex = Data.ToHex().Substring(4);

            Text = "0x" + Hex.Substring(2, 2) + Hex.Substring(0, 2) + "\n\n" + Data.FormatHex();
        }

        /*public static ushort GetPacketOpcode(MainWindow window, string name, OpcodeFlags flags)
        {
            ushort opCode =
                (from val in isServer ? ServerPacketNames : ClientPacketNames
                 where val.Value.Equals(name)
                 select val.Key).FirstOrDefault();

            if (opCode == 0)
            {
                while (true)
                {
                    try
                    {
                        InputValueBox inputBox = new InputValueBox(window, "Need to enter opcode", "Enter " + name + " opcode: ");
                        if (inputBox.Show() == false)
                            return 0;

                        opCode = BitConverter.ToUInt16(inputBox.Result.ToBytes(), 0);
                        break;
                    }
                    catch (Exception e)
                    {
                        System.Windows.MessageBox.Show("WRONG PARAMS!", "Error", 0, System.Windows.MessageBoxImage.Warning);
                    }
                }
            }

            return opCode;
        }*/
    }
}
