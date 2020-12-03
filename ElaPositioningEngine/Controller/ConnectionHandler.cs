using ElaSmili;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElaPositioningEngine.Controller
{
    public class ConnectionHandler
    {
        /** constructor */
        public ConnectionHandler()
        {
            ClientHandler.GetInstance().AddRawDataClient("192.168.1.12", 5001); //! todo
        }

        public async void RequestData(ElaSmiliRequest _SmiliRequest)
        {
            try
            {
                await ClientHandler.GetInstance().GetRawDataClient().GetData(new ElaSmiliDataPacket
                {
                    SmiliRequest = new ElaSmiliRequest
                    {
                        RequestType = "loc", 
                        Mreq = new MqttRequest { BrokerAdress = "192.168.1.17", BrokerPort = 1883 }  
                    }
                }); // ! todo connect
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "00101");
            }
        }

       

    }
}
