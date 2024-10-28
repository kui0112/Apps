using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusTest
{
    /// <summary>
    /// 读取模式
    /// </summary>
    public enum ReadType
    {
        //功能码01,读线圈
        Read01 = 0x01,
        //功能码02,读离散输入
        Read02 = 0x02,
        //功能码03,读多个寄存器
        Read03 = 0x03,
        //功能码04,读输入寄存器
        Read04 = 0x04
    }

    /// <summary>
    /// 写入模式
    /// </summary>
    public enum WriteType
    {
        //功能码05,写单个线圈
        Write01 = 0x05,
        //功能码06,写单个寄存器
        Write03 = 0x06,
        //功能码0F,写多个线圈
        Write01s = 0x0F,
        //功能码10,写多个寄存器
        Write03s = 0x10
    }

    internal static class Utils
    {
        public static Random random = new Random();

        public static byte[] CRC16(byte[] data)
        {
            int len = data.Length;
            if (len > 0)
            {
                ushort crc = 0xFFFF;
                for (int i = 0; i < len; i++)
                {
                    crc = (ushort)(crc ^ (data[i]));
                    for (int j = 0; j < 8; j++)
                    {
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                    }
                }
                byte hi = (byte)((crc & 0xFF00) >> 8); //高位置
                byte lo = (byte)(crc & 0x00FF); //低位置

                return BitConverter.IsLittleEndian ? new byte[] { lo, hi } : new byte[] { hi, lo };
            }
            return new byte[] { 0, 0 };
        }

        //public static string ArrayToString<T>(T[] values, string sep = " ")
        //{
        //    return string.Join(sep, values.Select(r => Convert.ToString(r)).ToArray());
        //}

        //public static bool RandomBoolean()
        //{
        //    return random.NextDouble() > 0.5;
        //}

        //public static ushort RandomUnsignedShort()
        //{
        //    return (ushort)random.Next(ushort.MinValue, ushort.MaxValue);
        //}
    }
}
