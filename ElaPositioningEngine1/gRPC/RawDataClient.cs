using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using ElaSmili;
using Grpc.Core;
using ElaPositioningEngine.Controller;

namespace ElaPositioningEngine.gRPC
{
    public class RawDataClient
    {
        private ElaSmiliRequestService.ElaSmiliRequestServiceClient client = null;
        public string IpAddress { get; } = string.Empty;
        public int IPort { get; } = 0;

        /* constructor */
        public RawDataClient()
        {

        }

        /* constructor */
        public RawDataClient(string _host, int _port)
        {
            client = new ElaSmiliRequestService.ElaSmiliRequestServiceClient(new Channel(_host, _port, ChannelCredentials.Insecure));
        }

        LocPayloadHandler LocPayload_handler = new LocPayloadHandler();
        public async Task GetData(ElaSmiliDataPacket elaSmiliDataPacket)
        {      
            try
            {
                using (var serverStream = client.GetData(elaSmiliDataPacket))  
                {
                    while (await serverStream.ResponseStream.MoveNext(System.Threading.CancellationToken.None))
                    {
                        ElaSmiliDataPacket dataPacket = serverStream.ResponseStream.Current;
                        if (dataPacket.ElaTech == "Wirepas")
                        {
                            LocPayload_handler.AddIncomingLocPayload(dataPacket.WirepasData.WpNodeItem.NodeAddress, dataPacket.WirepasData.WirepasLocData.WpLocPayload.ToByteArray(), dataPacket.WirepasData.WpNodeItem.NodeType);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        internal void AddDataClient(ElaSmiliDataPacket request)
        {
            client = new ElaSmiliRequestService.ElaSmiliRequestServiceClient(new Channel("192.168.0.66", 1883, ChannelCredentials.Insecure));
        }
    }
}
