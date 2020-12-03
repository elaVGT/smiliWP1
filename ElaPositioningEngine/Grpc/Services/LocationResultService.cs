using System.Threading.Tasks;
using Grpc.Core;
using System;
using ElaSmili;
using ElaPositioningEngine.Controller;
using ElaPositioningEngine.Configuration.AnchorModel;
using ElaPositioningEngine.Configuration.CalibrationModel;

/**
 * \namespace ElaPositioningEngine.gRPC.Services
 * \brief implementation on the server side of the microservices functions
 */
namespace ElaPositioningEngine.Grpc.Services
{
    /**
     * \class LocationResultService
     * \brief implemention of the EPE microservice results server interface
     */
    public class LocationResultService : ElaSmiliRequestService.ElaSmiliRequestServiceBase
    {
        /**
         * \fn GetLocation
         * \brief GetLocation function to start the Location results dataflow 
         * \param [in] ElaSmiliDataPacket : smili datapacket containing results request
         * 
         * \return IServerStreamWriter<ElaSmiliDataPacket> stream with smili datapacket containing results Data
         */
        public override async Task GetLocation(ElaSmiliDataPacket request, IServerStreamWriter<ElaSmiliDataPacket> responseStream, ServerCallContext context) 
        {
            try
            {
                new ConnectionHandler().RequestData(request.SmiliRequest);
                AnchorModel.GetInstance().LoadAnchorsConfig(request.SmiliRequest.SmiliConfigRequest.AnchorsConfigJson);
                CalibrationConfigModel.GetInstance().LoadCaliConfig(request.SmiliRequest.SmiliConfigRequest.CaliConfigJson);
               // if (request.SmiliRequest.SmiliConfigRequest.Status == "StartUp") // if fresh instance of eRTLS 
                //{
                    // send online positioning and vbat data which is contineously updating at EPE to eRTLS GUI 
                  //  iDataHandler.getInstance().iPosDataTransport();
                  //  iDataHandler.getInstance().iVbatDataTransport();
               // }

                while (!context.CancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        if (EpeResultsHandler.GetInstance().CountAvailableDataPackets() > 0)
                        {
                            var datapackets = EpeResultsHandler.GetInstance().GetLast();
                            if (datapackets.Count > 0)
                            {

                                foreach (ElaSmiliDataPacket response in datapackets)
                                {
                                    await responseStream.WriteAsync(response); //!
                                    Console.WriteLine(response.WirepasData.WpNodeItem.NodeAddress + " " + response.WirepasData.DataType + " streamed " + response.WirepasData.TimeStamp);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + "offofo");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "offofox");
            }
        }
    }
}
