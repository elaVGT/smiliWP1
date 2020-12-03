

namespace ElaWirepasDataService.Mqtt.Infos
{
    /**
     * \class MqttPublishInfo
     * \brief Mqtt Subscribe Information Model
     */
    public class MqttSubscribeInfo
    {

        /** constructor */
        public MqttSubscribeInfo()
        {

        }

        /** constructor */
        public MqttSubscribeInfo(string brokerAdress, int port, string[] topics, byte[] qos)
        {
            this.Sub_broker_address = brokerAdress;
            this.Sub_broker_port = port;
            this.Sub_topics = topics;
            this.Sub_qos = qos;
        }

        #region accessors
        public string Sub_broker_address { get; set; } = string.Empty;
        public int Sub_broker_port { get; set; } = 0;
        public string[] Sub_topics { get; set; } = new string[] { };
        public byte[] Sub_qos { get; set; } = new byte[] { };
        #endregion
    }
}