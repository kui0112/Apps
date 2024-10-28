using System;
using System.Net.Sockets;
using NModbus;


var address = "127.0.0.1";
var port = 530;

var client = new TcpClient(address, port);
var factory = new ModbusFactory();
var master = factory.CreateMaster(client);

// 数组输出为字符串
string ArrayToString<T>(T[] values, string sep = " ")
{
    return string.Join(sep, values.Select(r => Convert.ToString(r)).ToArray());
}

// 随机数对象
var random = new Random();

// 随机Boolean
bool RandomBoolean()
{
    return random.NextDouble() > 0.5;
}

// 随机ushort
ushort RandomUnsignedShort()
{
    return (ushort)random.Next(ushort.MinValue, ushort.MaxValue);
}

try
{
    // 写线圈
    var coilsToWrite = Enumerable.Range(0, 4).Select(i => RandomBoolean()).ToArray();
    Console.WriteLine($"写线圈：{ArrayToString(coilsToWrite)}");
    master.WriteMultipleCoils(1, 0, coilsToWrite);

    // 读线圈
    var coils = master.ReadCoils(1, 0, 4);
    Console.WriteLine($"读线圈：{ArrayToString(coils)}");

    // 写单个线圈 / 写之前
    Console.WriteLine($"读线圈：{ArrayToString(master.ReadCoils(1, 0, 4))}");

    // 写单个线圈
    var coilValue = RandomBoolean();
    master.WriteSingleCoil(1, 1, coilValue);
    Console.WriteLine($"写单个线圈（地址=1）：{coilValue}");

    // 写单个线圈
    coilValue = RandomBoolean();
    master.WriteSingleCoil(1, 2, coilValue);
    Console.WriteLine($"写单个线圈（地址=2）：{coilValue}");

    // 写单个线圈 / 写之后
    Console.WriteLine($"读线圈：{ArrayToString(master.ReadCoils(1, 0, 4))}");

    // 读输入状态
    var inputs = master.ReadInputs(2, 0, 4);
    Console.WriteLine($"读输入状态：{ArrayToString(inputs)}");

    // 写多个保持寄存器
    var registerValues = Enumerable.Range(0, 4).Select(i => RandomUnsignedShort()).ToArray();
    Console.WriteLine($"写保持寄存器：{ArrayToString(registerValues)}");
    master.WriteMultipleRegisters(3, 0, registerValues);

    // 写单个寄存器 / 写之前
    registerValues = master.ReadHoldingRegisters(3, 0, 4);
    Console.WriteLine($"读保持寄存器：{ArrayToString(registerValues)}");

    // 写单个寄存器
    var value = RandomUnsignedShort();
    Console.WriteLine($"写单个寄存器（地址=1）：{value}");
    master.WriteSingleRegister(3, 1, value);

    // 写单个寄存器
    value = RandomUnsignedShort();
    Console.WriteLine($"写单个寄存器（地址=2）：{value}");
    master.WriteSingleRegister(3, 2, value);

    // 写单个寄存器 / 写之后
    registerValues = master.ReadHoldingRegisters(3, 0, 4);
    Console.WriteLine($"读保持寄存器：{ArrayToString(registerValues)}");

    // 读多个输入寄存器
    var inputRegisters = master.ReadInputRegisters(4, 0, 4);
    Console.WriteLine($"读输入寄存器：{ArrayToString(inputRegisters)}");
}
catch (SlaveException ex)
{
    Console.WriteLine(ex.Message);
}

// 关闭client
client.Close();
