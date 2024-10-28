using System.Net.Sockets;
using NModbus;

namespace App1
{
    internal class Communication
    {
        private string address;
        private int port;
        private ModbusFactory factory;

        private TcpClient? client;
        private IModbusMaster? modbus;

        public Communication(string address, int port)
        {
            this.address = address;
            this.port = port;
            this.factory = new ModbusFactory();
        }

        public void Connect()
        {
            client = new TcpClient(address, port);
            modbus = factory.CreateMaster(client);
        }

        public IModbusMaster? Modbus()
        {
            return modbus;
        }

        public void Disconnect()
        {
            modbus?.Dispose();
            client?.Close();
        }
    }
}
