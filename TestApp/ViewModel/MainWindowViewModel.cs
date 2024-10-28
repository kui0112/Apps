using System.Net.Sockets;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using NModbus;
using TestApp.Message;
using TestApp.Model;


namespace TestApp.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IMessenger _messenger;

        [ObservableProperty]
        private string _netAddress = "127.0.0.1";
        [ObservableProperty]
        private string _netPort = "502";

        [ObservableProperty]
        private IEnumerable<ModbusFunctionModel> _modbusFunctions = [
                new ModbusFunctionModel { Name = "ReadCoils", Value = ModbusFunctionEnum.ReadCoils },
                new ModbusFunctionModel { Name = "ReadInputs", Value = ModbusFunctionEnum.ReadInputs },
                new ModbusFunctionModel { Name = "ReadInputRegisters", Value = ModbusFunctionEnum.ReadInputRegisters },
                new ModbusFunctionModel { Name = "ReadHoldingRegisters", Value = ModbusFunctionEnum.ReadHoldingRegisters },
                new ModbusFunctionModel { Name = "WriteMultipleCoils", Value = ModbusFunctionEnum.WriteMultipleCoils },
                new ModbusFunctionModel { Name = "WriteMultipleRegisters", Value = ModbusFunctionEnum.WriteMultipleRegisters },
            ];

        [ObservableProperty]
        private ModbusFunctionEnum _selectedFunctionValue = ModbusFunctionEnum.ReadCoils;

        [ObservableProperty]
        private int _tabSelectedIndex;

        [ObservableProperty]
        private string _logStream = "";

        // network
        private TcpClient? _client;
        private readonly ModbusFactory _factory = new();
        private IModbusMaster? _modbus;

        public MainWindowViewModel()
        {
            _messenger = WeakReferenceMessenger.Default;
            Console.WriteLine("MainWindowViewModel created.");
        }

        [RelayCommand]
        public Task Connect()
        {
            if (_client != null)
            {
                try
                {
                    _client.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            NetPort = NetPort.Trim();
            NetAddress = NetAddress.Trim();

            Console.WriteLine($"connecting to {NetAddress}:{NetPort}.");

            return Task.Run(() =>
            {
                try
                {
                    _client = new TcpClient(NetAddress, int.Parse(NetPort));
                    _modbus = _factory.CreateMaster(_client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBoxResult result = MessageBox.Show("Connect Failed.", "Info", MessageBoxButton.YesNo);
                }
            });
        }

        [RelayCommand]
        public Task Send()
        {
            return Task.Run(() =>
            {
                try
                {

                    if (_client == null || _modbus == null)
                    {
                        MessageBox.Show("Not connected to any Modbus network.", "Info", MessageBoxButton.YesNo);
                        return;
                    };

                    var selectedFunction = Enum.GetName(typeof(ModbusFunctionEnum), SelectedFunctionValue);
                    if (selectedFunction == null) return;
                    if (selectedFunction.StartsWith("Read"))
                    {
                        var response = _messenger.Send(new GetReadParamsMsg());
                        var data = response.Response;

                        var slaveAddress = Convert.ToByte(data.A, 16);
                        var startAddress = Convert.ToUInt16(data.B, 16);
                        var num = Convert.ToUInt16(data.C, 10);

                        Console.WriteLine($"slaveAddress={slaveAddress}, startAddress={startAddress}, num={num}.");

                        switch (SelectedFunctionValue)
                        {
                            case ModbusFunctionEnum.ReadCoils:
                                {
                                    var res = _modbus.ReadCoils(slaveAddress, startAddress, num);
                                    Console.WriteLine($"ReadCoils: {string.Join(", ", res)}");
                                    Log(string.Join(", ", res));
                                    break;
                                }
                            case ModbusFunctionEnum.ReadInputs:
                                {
                                    var res = _modbus.ReadInputs(slaveAddress, startAddress, num);
                                    Console.WriteLine($"ReadInputs: {string.Join(", ", res)}");
                                    Log(string.Join(", ", res));
                                    break;
                                }
                            case ModbusFunctionEnum.ReadInputRegisters:
                                {
                                    var res = _modbus.ReadInputRegisters(slaveAddress, startAddress, num);
                                    Console.WriteLine($"ReadInputRegisters: {string.Join(", ", res)}");
                                    Log(string.Join(", ", res));
                                    break;
                                }
                            case ModbusFunctionEnum.ReadHoldingRegisters:
                                {
                                    var res = _modbus.ReadHoldingRegisters(slaveAddress, startAddress, num);
                                    Console.WriteLine($"ReadHoldingRegisters: {string.Join(", ", res)}");
                                    Log(string.Join(", ", res));
                                    break;
                                }
                            default: break;
                        }
                    }
                    else
                    {
                        var response = _messenger.Send(new GetWriteParamsMsg());
                        var data = response.Response;

                        var slaveAddress = Convert.ToByte(data.A, 16);
                        var startAddress = Convert.ToUInt16(data.B, 16);
                        var valuesString = data.C;

                        Console.WriteLine($"slaveAddress={slaveAddress}, startAddress={startAddress}, valuesString={valuesString}.");

                        switch (SelectedFunctionValue)
                        {
                            case ModbusFunctionEnum.WriteMultipleCoils:
                                {
                                    var values = valuesString.Split().Select(s => s == "1").ToArray();
                                    Console.WriteLine($"WriteMultipleCoils, values={string.Join(",", values)}.");

                                    _modbus.WriteMultipleCoils(slaveAddress, startAddress, values);
                                    Console.WriteLine("WriteMultipleCoils success.");
                                    Log("success");
                                    break;
                                }
                            case ModbusFunctionEnum.WriteMultipleRegisters:
                                {
                                    var values = valuesString.Split().Select(s => Convert.ToUInt16(s, 16)).ToArray();
                                    Console.WriteLine($"WriteMultipleRegisters, values={string.Join(",", values)}.");

                                    _modbus.WriteMultipleRegisters(slaveAddress, startAddress, values);
                                    Console.WriteLine("WriteMultipleRegisters success.");
                                    Log("success");
                                    break;
                                }
                            default: break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxResult result = MessageBox.Show(ex.Message, "Info", MessageBoxButton.YesNo);
                }
            });
        }

        [RelayCommand]
        public void SelectionChange()
        {
            Task.Run(() =>
            {
                var selectedFunction = Enum.GetName(typeof(ModbusFunctionEnum), SelectedFunctionValue);
                if (selectedFunction == null) return;

                TabSelectedIndex = selectedFunction.StartsWith("Read") ? 0 : 1;

                Console.WriteLine($"Current Function: {selectedFunction}");
            });
        }

        public void Log(string str)
        {
            LogStream += $">> {str}\r\n";
        }
    }
}
