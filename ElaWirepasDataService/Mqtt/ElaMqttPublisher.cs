using ElaWirepas;
using Google.Protobuf;
using System;
using System.Diagnostics;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ElaWirepasDataService.Mqtt
{
    /**
     * * \class ElaMqttPublisher
     * * \brief Mqtt publisher
     */
    public class ElaMqttPublisher
    {
        public ElaMqttPublisher()
        {

        }

        /** function to
         * publish wirepas message
         * 
         */
        public bool PublishMessage(string topic, WirepasMessage wirepasMessage)
        {
            // protobuf formatting
            GenericMessage genericMessage = new GenericMessage { Wirepas = wirepasMessage };
            byte[] internalMessage = genericMessage.ToByteArray();

            try
            {
                ElaMqttClient.getInstance().GetMqttClient().Publish(topic, internalMessage, 2, false);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "EMP001");
                return false;
            }
        }
    }
}
