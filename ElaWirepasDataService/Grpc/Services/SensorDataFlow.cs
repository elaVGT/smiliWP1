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

    public class SensorDataFlowService : ElaWirepasPublicService.ElaWirepasPublicServiceBase
    {
        /**
                 * \fn StartWirepasDataFlow
                 * \brief StartWirepasDataFlow function to start the Wirepas dataflow 
                 * \param [in] ElaWirepasDataRequest : Data request
                 * 
                 * \return IServerStreamWriter<ElaWirepasDataPacket> stream with Wirepas Data
                 */
        public override async Task StartWirepasDataFlow(ElaWirepasDataRequest request, IServerStreamWriter<ElaWirepasDataPacket> responseStream, ServerCallContext context)
        {
            try
            {
                new ElaMqttSubscriber().Subscribe(request);

                while (!context.CancellationToken.IsCancellationRequested)
                {
                    var datapackets = ElaWirepasDatapacketHandler.getInstance().GetLast();
                    foreach (ElaWirepasDataPacket response in datapackets)
                    {
                        await responseStream.WriteAsync(response);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /**
         * \fn SendElaWirepasCommand
         * \brief SendElaWirepasCommand function to send Commands to Wirepas Ndes
         * \param [in] SendPacketReq : Send Data request
         * 
         * \return SendPacketResp : send data response
         */
        public override Task<SendPacketResp> SendElaWirepasCommand(SendPacketReq sendPacketReq, ServerCallContext context)
        {
            // protobuf formatting 
            WirepasMessage wirepasMessage = new WirepasMessage { SendPacketReq = sendPacketReq };

            try
            {
                // todo get all getways, get all sinks
                bool published = new ElaMqttPublisher().PublishMessage(ElaWirepasLibraryConstant.SEND_PACKET_TOPIC + "wm-evk/sink0", wirepasMessage);

                //
                // todo: link response from  
                return Task.FromResult(new SendPacketResp { Header = new ResponseHeader { ReqId = sendPacketReq.Header.ReqId } });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Task.FromResult(new SendPacketResp { Header = new ResponseHeader { ReqId = sendPacketReq.Header.ReqId, Res = ErrorCode.InternalError } });
            }
        }
    }
    }
