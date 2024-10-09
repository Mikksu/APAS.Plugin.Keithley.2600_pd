using System;
using System.IO.Ports;

namespace APAS.Plugin.KEYTHLEY.SMU2600.Ports
{
    public class PortSerial(SerialPort serialPort) : PortBase<SerialPort>(serialPort)
    {
        public override void Open()
        {
            if (InnerPort == null)
                throw new NullReferenceException($"inner port is null.");

            InnerPort.Open();
        }

        public override void Close()
        {
            InnerPort.Close();
        }

        public override void Write(string message)
        {
            InnerPort.WriteLine(message);
        }

        public override void Write(byte[] data)
        {
            InnerPort.Write(data, 0, data.Length);
        }

        public override string ReadAscii()
        {
            return InnerPort.ReadLine();
        }

        public override byte[] ReadBin(int len)
        {
            var data = new byte[len];
            InnerPort.Read(data, 0, len);
            return data;
        }
    }
}
