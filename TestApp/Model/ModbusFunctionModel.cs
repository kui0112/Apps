namespace TestApp.Model
{
    public enum ModbusFunctionEnum
    {
        ReadCoils,
        ReadInputs,
        ReadInputRegisters,
        ReadHoldingRegisters,
        WriteMultipleCoils,
        WriteMultipleRegisters,
    }

    public class ModbusFunctionModel
    {
        public string? Name { get; set; }
        public ModbusFunctionEnum? Value { get; set; }
    }
}
