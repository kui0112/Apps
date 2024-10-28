using System.IO.Ports;
using System.Text;

namespace SerialPortCommunication
{
    class Test
    {
        public static void Sync()
        {
            var port = new SerialPort
            {
                // 设置串口名
                PortName = "COM2",
                // 设置波特率
                BaudRate = 9600,
                // 设置奇偶校验方式
                Parity = Parity.None,
                // 设置停止位
                StopBits = StopBits.One
            };

            port.Open();

            try
            {
                while (true)
                {
                    Console.Write("Your Message: ");
                    // 读取用户输入
                    var msg = Console.ReadLine();
                    if (msg == null || msg.Trim() == "") continue;
                    // 发送消息
                    port.WriteLine(msg);
                    // 接收消息
                    var receivedMsg = port.ReadLine();
                    Console.WriteLine($"Received: {receivedMsg}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            port.Close();
        }

        public static async void Async()
        {
            var port = new SerialPort
            {
                // 设置串口名
                PortName = "COM2",
                // 设置波特率
                BaudRate = 9600,
                // 设置奇偶校验方式
                Parity = Parity.None,
                // 设置停止位
                StopBits = StopBits.One
            };

            port.DataReceived += (s, e) =>
            {
                var receivedMsg = (s as SerialPort)!.ReadExisting();
                Console.WriteLine($"Received: {receivedMsg}");
            };

            port.Open();

            Task<string> ReadLineAsync()
            {

                return Task.Run(() =>
                {
                    Console.Write("Your Message: ");
                    return Console.ReadLine()!;
                });
            }

            async Task SendAsync(SerialPort sp, string msg)
            {
                var bytes = Encoding.UTF8.GetBytes(msg);
                await sp.BaseStream.WriteAsync(bytes, 0, bytes.Length);
            }

            await Task.Run(async () =>
            {
                while (true)
                {
                    // 读取用户输入
                    var msg = await ReadLineAsync()!;
                    if (msg == null || msg.Trim() == "") continue;

                    // 发送消息
                    await SendAsync(port, msg);
                }
            });

            port.Close();
        }
    }
}
