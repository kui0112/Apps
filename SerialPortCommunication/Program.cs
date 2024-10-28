using System.IO.Ports;
using System.Text;


var msg = "0210000400020400010001";
var bytes = Enumerable.Range(0, msg.Length / 2).Select(i => Convert.ToByte(msg.Substring(i * 2, 2), 16));
var lrc = ((bytes.Aggregate((a, b) => (byte)(a + b)) ^ 0xFF) + 1) & 0xFF;

Console.WriteLine(lrc.ToString("X2"));
