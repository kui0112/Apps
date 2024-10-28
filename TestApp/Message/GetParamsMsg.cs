using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TestApp.Message
{
    public static class Payloads
    {
        public record ThreeString
        {
            public string A;
            public string B;
            public string C;

            public ThreeString(string a, string b, string c)
            {
                A = a;
                B = b;
                C = c;
            }
        }
    }

    public class GetReadParamsMsg : RequestMessage<Payloads.ThreeString>
    {
        public GetReadParamsMsg() : base() { }
    }

    public class GetWriteParamsMsg : RequestMessage<Payloads.ThreeString>
    {
        public GetWriteParamsMsg() : base() { }
    }
}
