using System;
using System.IO.Ports;
using NationalInstruments.NI4882;

namespace APAS.Plugin.KEYTHLEY.SMU2600.Ports
{
    public static class PortFactory
    {
        public static IPort CreateGPIBPort(int boardNum, byte primaryAddress, byte secondaryAddress = 0x0)
        {
            var gpib = new Device(boardNum, new Address(primaryAddress, secondaryAddress));
            return new PortGPIB(gpib);
        }

        public static IPort CreateSerialPort(string portName, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            var serial = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            return new PortSerial(serial);
        }

        /// <summary>
        /// 将文本转换为<see cref="PortTypeEnum"/>类型。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PortTypeEnum? ConvertToPortType(string type)
        {
            if (!Enum.TryParse<PortTypeEnum>(type, true, out var t))
                return null;
            else
                return t;
        }
    }
}
