using System.Threading.Tasks;
using Grpc.Core;
using System;
using ElaSmili;
using ElaPositioningEngine.Controller;

/**
 * \namespace ElaPositioningEngine.gRPC.Services
 * \brief implementation on the server side of the microservices functions
 */
namespace ElaPositioningEngine.gRPC.Services
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
                new RawDataClient().AddDataClient(request);

                while (!context.CancellationToken.IsCancellationRequested)
                {
                    var datapackets = EpeResultsHandler.GetInstance().GetLast();
                    foreach (ElaSmiliDataPacket response in datapackets)
                    {
                        await responseStream.WriteAsync(response); //!
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
