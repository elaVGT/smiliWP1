using System.Threading.Tasks;
using Grpc.Core;
using ElaWirepasDataService.Mqtt;
using ElaWirepasDataService.Controllers;
using System;
using ElaWirepas;
using ElaSmili;
using ElaWirepasLibrary.Model.Wirepas;

/**
 * \namespace ElaWirepasDataService.Grpc.Services
 * \brief implementation on the server side of the microservices functions
 */
namespace ElaWirepasDataService.Grpc.Services
{

    public class SmiliDataFlowService : ElaSmiliRequestService.ElaSmiliRequestServiceBase
    {
        /**
         * \fn GetData  
         * \brief GetData function to start the  dataflow for smili // todo: combine
         * \param [in] ElaSmiliDataPacket : smili datapacket containing Data request
         * 
         * \return IServerStreamWriter<ElaSmiliDataPacket> stream  smili datapacket with Wirepas Data
         */
        public override async Task GetData(ElaSmiliDataPacket request, IServerStreamWriter<ElaSmiliDataPacket> responseStream, ServerCallContext context)
        {
            try
            {
                new ElaMqttSubscriber().Subscribe(new ElaWirepasDataRequest { BrokerAdress = request.SmiliRequest.Mreq.BrokerAdress, BrokerPort = request.SmiliRequest.Mreq.BrokerPort }); // !

                while (!context.CancellationToken.IsCancellationRequested)
                {
                    if (ElaWirepasDatapacketHandler.getInstance().CountAvailableDataPackets() > 0)
                    {
                        var datapackets = ElaWirepasDatapacketHandler.getInstance().GetLast();
                        if (datapackets.Count > 0)
                        {
                            foreach (ElaWirepasDataPacket response in datapackets)
                            {
                                await responseStream.WriteAsync(new ElaSmiliDataPacket { WirepasData = response }); //!
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "003");
            }
        }
    }
    }
