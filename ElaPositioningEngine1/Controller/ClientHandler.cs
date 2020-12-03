using System;
using System.Collections.Generic;
using System.Text;
using ElaPositioningEngine.gRPC;
using ElaSmili;

namespace ElaPositioningEngine.Controller
{
    public class ClientHandler
    {
        public static ClientHandler instance = null;
        private RawDataClient rawDataClient = null;
       
        public static ClientHandler GetInstance()
        {
            if(null == instance)
            {
                instance = new ClientHandler();
            }
            return instance;
        }

        public void AddRawDataClient(string ipAddress, int port)
        {
            if(null != rawDataClient)
            {
                if(false == rawDataClient.IpAddress.Equals(ipAddress) && port != rawDataClient.IPort)
                {
                    rawDataClient = new RawDataClient(ipAddress, port);
                }
            }
            else
            {
                rawDataClient = new RawDataClient(ipAddress, port);
            }
        }

        public void ClearRawDataClient()
        {
            rawDataClient = null;
        }

        public RawDataClient GetRawDataClient()
        {
            return this.rawDataClient;
        }
    }
}
