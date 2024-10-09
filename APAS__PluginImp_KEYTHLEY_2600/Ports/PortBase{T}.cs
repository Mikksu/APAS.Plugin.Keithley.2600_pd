using System;

namespace APAS.Plugin.KEYTHLEY.SMU2600.Ports
{
    public class PortBase<T>(T innerPort) : IPort
    {
        /// <summary>
        /// 返回实际通讯端口实例。
        /// </summary>
        public T InnerPort { get; } = innerPort;

        /// <inheritdoc/>
        public virtual void Open()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual void Close()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual void Write(string message)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual void Write(byte[] data)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual string ReadAscii()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual byte[] ReadBin(int len)
        {
            throw new NotImplementedException();
        }

        public virtual void Dispose()
        {
            // TODO release managed resources here
        }

        public override string ToString()
        {
            return innerPort?.ToString()??"Inner port is null";
        }
    }
}
