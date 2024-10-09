using NationalInstruments.NI4882;
using System;

namespace APAS.Plugin.KEYTHLEY.SMU2600.Ports
{
    public class PortGPIB(Device gpib) : PortBase<Device>(gpib)
    {
        public override void Open()
        {
            if (InnerPort == null)
                throw new NullReferenceException($"inner port is null.");
        }

        public override void Close()
        {
            
        }

        public override void Write(string message)
        {
            InnerPort.Write(message);
        }

        public override void Write(byte[] data)
        {
            InnerPort.Write(data);
        }

        public override string ReadAscii()
        {
            return InnerPort.ReadString();
        }

        public override byte[] ReadBin(int len)
        {
            return InnerPort.ReadByteArray(len);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
