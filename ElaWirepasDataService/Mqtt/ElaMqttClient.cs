using ElaWirepasDataService.Mqtt.Infos;
using System;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ElaWirepasDataService.Mqtt
{
    public class ElaMqttClient
    {
        private static ElaMqttClient instance = null;

        /** current mqtt client */
        private MqttClient m_MqttClient = null;


        public Dictionary<string, MqttClient> m_mqttClientList = new Dictionary<string, MqttClient>();

        public void AddMqttClient(MqttSubscribeInfo info)
        {
            this.m_MqttClient = new MqttClient(info.Sub_broker_address, info.Sub_broker_port, false, null, null, new MqttSslProtocols());
            if (!m_mqttClientList.ContainsKey(info.Sub_broker_address))
            {
                m_mqttClientList.Add(info.Sub_broker_address, this.m_MqttClient); // todo: user address into the key
            }
        }

        public MqttClient GetMqttClient(MqttSubscribeInfo info)
        {
            this.m_MqttClient = m_mqttClientList[info.Sub_broker_address];

            if (!this.m_MqttClient.IsConnected)
            {
                string clientId = Guid.NewGuid().ToString();
                m_MqttClient.Connect(clientId);
            }
            return m_MqttClient;
        }

        public MqttClient GetMqttClient()
        {
            return m_MqttClient;
        }

        /** constructor */
        private ElaMqttClient()
        {

        }

        /** singleton */
        public static ElaMqttClient getInstance()
        {
            if (null == instance)
            {
                instance = new ElaMqttClient();
            }
            return instance;
        }


        public bool Disconnect()
        {
            if (null != this.m_MqttClient)
            {
                this.m_MqttClient.Disconnect();
            }
            return true;
        }

        public bool Uns()
        {
            if (null != this.m_MqttClient)
            {
                this.m_MqttClient.Disconnect();
            }
            return true;
        }
    }
}
