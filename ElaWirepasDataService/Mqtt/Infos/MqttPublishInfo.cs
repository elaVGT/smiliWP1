using System;


namespace ElaWirepasDataService.Mqtt.Infos
{
    /**
     * \class MqttPublishInfo
     * \brief Mqtt Publish Information Model
     */
    public class MqttPublishInfo
    {
        /** constructor */
        public MqttPublishInfo()
        {

        }

        /** constructor */
        public MqttPublishInfo(string topic, byte[] message, byte qos, bool retain)
        {
            this.Pub_topic = topic;
            this.Pub_message = message;
            this.Pub_qos = qos;
            this.Pub_retain = retain;
        }

        #region accessors
        public string Pub_topic { get; set; } = string.Empty;
        public byte[] Pub_message { get; set; } = new byte[] { };
        public byte Pub_qos { get; set; } = Convert.ToByte(1);
        public bool Pub_retain { get; set; } = false;
        #endregion
    }
}
