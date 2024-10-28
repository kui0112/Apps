using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using TestApp.Message;

namespace TestApp.ViewModel
{
    public partial class WriteRegistersViewModel : ObservableObject
    {
        private readonly IMessenger _messenger;

        [ObservableProperty]
        private string _slaveAddress = "0";

        [ObservableProperty]
        private string _startRegisterAddress = "0";

        [ObservableProperty]
        private string _values = "0";

        public WriteRegistersViewModel() : base()
        {
            _messenger = WeakReferenceMessenger.Default;
            _messenger.Register<GetWriteParamsMsg>(this, GetParams);
        }

        private void GetParams(object sender, GetWriteParamsMsg msg)
        {
            msg.Reply(new Payloads.ThreeString(SlaveAddress, StartRegisterAddress, Values));
        }
    }
}
