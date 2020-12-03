using ElaSmili;
using ElaWirepas;
using ElaWirepasDataService.Controllers;
using ElaWirepasDataService.Mqtt.Infos;
using ElaWirepasLibrary.Model.Wirepas;
using ElaWirepasLibrary.Model.Wirepas.Payload;
using Google.Protobuf;
using System;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ElaWirepasDataService.Mqtt
{
    /**
     * * \class ElaMqttSubscriber
     * * \brief Mqtt Subscriber
     */
    public class ElaMqttSubscriber
    {
        public ElaMqttSubscriber()
        {

        }

        private ElaWirepasSensorDataItem sensorDataItem;
        private ElaWirepasLocDataItem locationDataItem;
        double NodeBatteryVoltage;
        private string DataType;
        private uint NodeAddress;

        public bool Subscribe(ElaWirepasDataRequest _request)
        {
            MqttSubscribeInfo mqttSubscribeInfo = new MqttSubscribeInfo { Sub_broker_address = _request.BrokerAdress, Sub_broker_port = _request.BrokerPort, Sub_topics = new string[] { "#" }, Sub_qos = new byte[] { Convert.ToByte(2) } };
            ElaMqttClient.getInstance().AddMqttClient(mqttSubscribeInfo);

            try
            {
                // register to message received 
                ElaMqttClient.getInstance().GetMqttClient(mqttSubscribeInfo).MqttMsgPublishReceived += MqttClient_MqttMsgPublishReceived;

                // subscribe to the topic 
                ushort usret = ElaMqttClient.getInstance().GetMqttClient(mqttSubscribeInfo).Subscribe(mqttSubscribeInfo.Sub_topics, mqttSubscribeInfo.Sub_qos);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "001");
                return false;
            }
        }

        public bool Unsubscribe(MqttSubscribeInfo mqttSubscribeInfo, string[] us_topics)
        {
            if (null != ElaMqttClient.getInstance().GetMqttClient(mqttSubscribeInfo))
            {
                ElaMqttClient.getInstance().GetMqttClient(mqttSubscribeInfo).Unsubscribe(us_topics);
            }
            return true;
        }

        private void MqttClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            try
            {
                GenericMessage gen_message = new GenericMessage();
                
                try
                {
                    gen_message = GenericMessage.Parser.ParseFrom(e.Message);  // convert raw incoming message into Protobuf iMessage 
                }
                catch (Google.Protobuf.InvalidProtocolBufferException ex)
                {
                    Console.WriteLine(ex.Message + "00103");
                }

                // Handle PacketReceivedEvent
                if (null != gen_message.Wirepas && null != gen_message.Wirepas.PacketReceivedEvent)
                {
                    WirepasReceivedMessage wirepasRxMessage = new WirepasReceivedMessage(e.Message);
                    //
                    GenericMessage genericMessage = GenericMessage.Parser.ParseFrom(e.Message);
                    string json = JsonFormatter.Default.Format(genericMessage);

                    //
                    NodeAddress = wirepasRxMessage.WirepasRxDataObject.SourceAddress;

                    object payloadObject = WirepasPayloadFactory.GetInstance().Get(wirepasRxMessage.WirepasRxDataObject);
                    Console.WriteLine(wirepasRxMessage.WirepasRxDataObject.SourceAddress + " Endpoint = " + wirepasRxMessage.WirepasRxDataObject.SourceEndpoint);

                    if (null != payloadObject)
                    {
                        if (payloadObject is WirepasLocation)
                        {
                            //WirepasLocation location = (payloadObject as WirepasLocation); // todo !
                            locationDataItem = new ElaWirepasLocDataItem { WpLocPayload = wirepasRxMessage.WirepasRxDataObject.Payload };

                            DataType = "Location";
                        }

                        else if (payloadObject is WirepasBatteryVoltage)
                        {
                            WirepasBatteryVoltage BatVolt = (payloadObject as WirepasBatteryVoltage);
                            NodeBatteryVoltage = Math.Round(Convert.ToDouble(BatVolt.BatVoltResult.battery_voltage) / 1000, 2);
                            DataType = "Battery";
                        }

                        else if (payloadObject is WirepasSensorData)
                        {
                            WirepasSensorData sensorData = (payloadObject as WirepasSensorData);

                            if (sensorData.SensorResult.sensor_type == 2)
                            {
                                sensorDataItem = new ElaWirepasSensorDataItem { SensorType = 2, T = new ElaWirepasT { Temp = sensorData.SensorResult.temperature } };
                            }
                            else if (sensorData.SensorResult.sensor_type == 3)
                            {
                                sensorDataItem = new ElaWirepasSensorDataItem { SensorType = 3, Rht = new ElaWirepasRHT { Temp = sensorData.SensorResult.temperature, Humi = sensorData.SensorResult.humidity } };
                            }
                            else if (sensorData.SensorResult.sensor_type == 4)
                            {
                                sensorDataItem = new ElaWirepasSensorDataItem { SensorType = 4, Di = new ElaWirepasDI { DinActCount = sensorData.SensorResult.digit_in_act_counter, DinDeactCount = sensorData.SensorResult.digit_in_deact_counter } };
                            }
                            else if (sensorData.SensorResult.sensor_type == 5)
                            {
                                sensorDataItem = new ElaWirepasSensorDataItem { SensorType = 5, Do = new ElaWirepasDO { DoutActCount = sensorData.SensorResult.digit_out_act_counter, DoutDeactCount = sensorData.SensorResult.digit_out_deact_counter } };
                            }
                            else if (sensorData.SensorResult.sensor_type == 6)
                            {
                                sensorDataItem = new ElaWirepasSensorDataItem { SensorType = 6, At = new ElaWirepasAT { RemovCont = sensorData.SensorResult.tag_removed_counter, NonRemovCont = sensorData.SensorResult.tag_not_removed_counter } };
                            }
                            else if (sensorData.SensorResult.sensor_type == 7)
                            {
                                sensorDataItem = new ElaWirepasSensorDataItem { SensorType = 7, Mag = new ElaWirepasMAG { MagDectCont = sensorData.SensorResult.mag_detect_counter, MagNonDectCont = sensorData.SensorResult.mag_non_detect_counter } };
                            }
                            else if (sensorData.SensorResult.sensor_type == 8)
                            {
                                sensorDataItem = new ElaWirepasSensorDataItem { SensorType = 8, Mov = new ElaWirepasMOV { MovDectCont = sensorData.SensorResult.mov_detect_counter, MovNonDectCont = sensorData.SensorResult.mov_non_detect_counter } };
                            }

                            DataType = "Sensor";
                        }

                        else if (payloadObject is WirepasDiagnostics)
                        {
                            WirepasDiagnostics diagnostics = (payloadObject as WirepasDiagnostics);
                            if (0 != diagnostics.NodeDiagResults.Voltage)
                            {
                                NodeBatteryVoltage = Convert.ToDouble(diagnostics.NodeDiagResults.Voltage + 200) / 100;
                            }
                            DataType = "Battery";
                        }
                    }

                    DateTime timestamp = DateTime.Now;
                    ElaWirepasDataPacket m_dataPacket = new ElaWirepasDataPacket { TimeStamp = timestamp.ToString("HH:mm:ss"), WirepasLocData = locationDataItem,  WirepasSensorData = sensorDataItem, DataType = DataType, VBat = NodeBatteryVoltage, WpNodeItem = new ElaWirepasNodeItem { NodeAddress = NodeAddress, } };
                    ElaWirepasDataPacket CompleteDataPacket = ElaWirepasDatapacketHandler.getInstance().GetCompleteDatapacket(m_dataPacket);
                    //add datapacket streaming list
                    ElaWirepasDatapacketHandler.getInstance().Add(CompleteDataPacket); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "002");
            }
        }
    }
}