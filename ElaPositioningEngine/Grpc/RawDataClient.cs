using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using ElaSmili;
using Grpc.Core;
using ElaPositioningEngine.Controller;

namespace ElaPositioningEngine.Grpc
{
    public class RawDataClient
    {
        /* constructor */
        public RawDataClient()
        {

        }

        private ElaSmiliRequestService.ElaSmiliRequestServiceClient client = null;
        public string IpAddress { get; } = string.Empty;
        public int IPort { get; } = 0;

        /* constructor */
        public RawDataClient(string _host, int _port)
        {
            client = new ElaSmiliRequestService.ElaSmiliRequestServiceClient(new Channel(_host, _port, ChannelCredentials.Insecure));
        }

        public async Task GetData(ElaSmiliDataPacket elaSmiliDataPacket)
        {      
            try
            {
                using (var serverStream = client.GetData(elaSmiliDataPacket))  
                {
                    while (await serverStream.ResponseStream.MoveNext(System.Threading.CancellationToken.None))
                    {
                        ElaSmiliDataPacket dataPacket = serverStream.ResponseStream.Current;
                        if (dataPacket.WirepasData.DataType == "Location")
                        {
                            EpeResultsHandler.GetInstance().AddIncomingLocPackets(dataPacket);
                        }
                        else if (dataPacket.WirepasData.DataType == "Sensor")
                        {
                            EpeResultsHandler.GetInstance().AddSensorResults(dataPacket);
                        }
                        else if (dataPacket.WirepasData.DataType == "Battery")
                        {
                            EpeResultsHandler.GetInstance().AddBatteryResults(dataPacket);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "00102");
            }
        }
    }
}
