using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using TestApp.Message;

namespace TestApp.ViewModel
{
    public partial class ReadRegistersViewModel : ObservableObject
    {
        private readonly IMessenger _messenger;

        [ObservableProperty]
        private string _slaveAddress = "0";

        [ObservableProperty]
        private string _startRegisterAddress = "0";

        [ObservableProperty]
        private string _registerCount = "1";

        public ReadRegistersViewModel() : base()
        {
            _messenger = WeakReferenceMessenger.Default;
            _messenger.Register<GetReadParamsMsg>(this, GetParams);
        }

        private void GetParams(object sender, GetReadParamsMsg msg)
        {
            msg.Reply(new Payloads.ThreeString(SlaveAddress, StartRegisterAddress, RegisterCount));
        }
    }
}
