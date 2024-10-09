using System;

namespace APAS.Plugin.KEYTHLEY.SMU2600.Ports
{
    public interface IPort: IDisposable
    {
        /// <summary>
        /// 打开端口。
        /// </summary>
        void Open();

        /// <summary>
        /// 关闭端口。
        /// </summary>
        void Close();

        /// <summary>
        /// 写ASCII数据。
        /// </summary>
        void Write(string message);

        /// <summary>
        /// 写BIN数据。
        /// </summary>
        /// <param name="data"></param>
        void Write(byte[] data);

        /// <summary>
        /// 读ASCII数据。
        /// </summary>
        /// <returns></returns>
        string ReadAscii();

        /// <summary>
        /// 读二进制数据。
        /// </summary>
        /// <param name="len">读取的长度。</param>
        /// <returns></returns>
        byte[] ReadBin(int len);
    }
}
