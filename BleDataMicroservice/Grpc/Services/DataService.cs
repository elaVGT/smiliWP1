using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/**
* \namespace BleDataMicroservice.Grpc.Services
* \brief implementation on the server side of the microservices functions
*/
namespace BleDataMicroservice.Grpc.Services
{
    /**
     * \class DataService
     * \brief implemention of the BLE Data microservice server interface
     */
    public class DataService : ElaWirepasPublicService.ElaWirepasPublicServiceBase
    {
        /**
         * \fn StartWirepasDataFlow
         * \brief StartWirepasDataFlow function to start the Wirepas dataflow 
         * \param [in] ElaWirepasDataRequest : Data request
         * 
         * \return IServerStreamWriter<ElaWirepasDataPacket> stream with Wirepas Data
         */
    }
}
