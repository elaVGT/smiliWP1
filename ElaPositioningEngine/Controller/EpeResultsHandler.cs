using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ElaPositioningEngine.Grpc;
using ElaPositioningEngine.Model.Positioning;
using ElaSmili;
using ElaWirepasLibrary.Model.Wirepas.Payload;

/**
 * \namespace ElaPositioningEngine.Controllers
 * \brief namespace associated to EPE microservice
 */
namespace ElaPositioningEngine.Controller
{
    /**
     * \class EpeResultsHandler
     * \brief hanling datapacket containing to location results
     */
    public class EpeResultsHandler
    {
        private static EpeResultsHandler instance = null;
        /** internal mutex */
        private Mutex mutex = new Mutex();
        /** internal collection of locPayload data*/
        private readonly Dictionary<uint, ElaSmiliDataPacket> IncomingLocPackets = new Dictionary<uint, ElaSmiliDataPacket>();
        /** internal complete dataPackets list */
        private Dictionary<uint, ElaSmiliDataPacket> m_completeDataPackets = new Dictionary<uint, ElaSmiliDataPacket>();

        public static EpeResultsHandler GetInstance()
        {
            if(null == instance)
            {
                instance = new EpeResultsHandler();
            }
            return instance;
        }

        /** constructor */
        private EpeResultsHandler()
        {

        }


        /** function to add incoming payload to IncomingPayloadList*/
        public void AddIncomingLocPackets(ElaSmiliDataPacket in_dataPacket)
        {
            if (this.mutex.WaitOne())
            {
                if (!IncomingLocPackets.ContainsKey(in_dataPacket.WirepasData.WpNodeItem.NodeAddress))
                {
                    this.IncomingLocPackets.Add(in_dataPacket.WirepasData.WpNodeItem.NodeAddress, in_dataPacket);
                }
                else
                {
                    this.IncomingLocPackets[in_dataPacket.WirepasData.WpNodeItem.NodeAddress] = in_dataPacket;
                }
                this.mutex.ReleaseMutex();
            }

            this.ProcessPositioning();
        }

        /** Collection of incoming payload data*/
        public Dictionary<uint, ElaSmiliDataPacket> GetIncomingLocPacket()
        {
            Dictionary<uint, ElaSmiliDataPacket> PacketList = new Dictionary<uint, ElaSmiliDataPacket>();
            if (this.mutex.WaitOne())
            {
                foreach (KeyValuePair<uint, ElaSmiliDataPacket> entry in this.IncomingLocPackets)
                {
                    PacketList.Add(entry.Key, entry.Value);
                }
                this.IncomingLocPackets.Clear();
                this.mutex.ReleaseMutex();
            }
            return PacketList;
        }

        /** add sensor dataPacket to the list */
        public void AddSensorResults(ElaSmiliDataPacket Sensor_result_dataPacket)
        {
            if (this.mutex.WaitOne())
            {
                if (!m_completeDataPackets.ContainsKey(Sensor_result_dataPacket.WirepasData.WpNodeItem.NodeAddress))
                {
                    this.m_completeDataPackets.Add(Sensor_result_dataPacket.WirepasData.WpNodeItem.NodeAddress, Sensor_result_dataPacket);
                    iDataHandler.getInstance().update_iPosData(Sensor_result_dataPacket.WirepasData.WpNodeItem.NodeAddress, Sensor_result_dataPacket);
                }
                else
                {
                    double vbat = this.m_completeDataPackets[Sensor_result_dataPacket.WirepasData.WpNodeItem.NodeAddress].WirepasData.VBat;
                    this.m_completeDataPackets[Sensor_result_dataPacket.WirepasData.WpNodeItem.NodeAddress] = Sensor_result_dataPacket;
                    if(vbat != 0)
                    {
                        this.m_completeDataPackets[Sensor_result_dataPacket.WirepasData.WpNodeItem.NodeAddress].WirepasData.VBat = vbat; //! check 
                    }
                }
                this.mutex.ReleaseMutex();
            }
        }

        /** add bat info to dataPacket in the list */
        public void AddBatteryResults(ElaSmiliDataPacket Battery_result_dataPacket)
        {
            if (this.mutex.WaitOne())
            {
                if (!m_completeDataPackets.ContainsKey(Battery_result_dataPacket.WirepasData.WpNodeItem.NodeAddress))
                {
                    this.m_completeDataPackets.Add(Battery_result_dataPacket.WirepasData.WpNodeItem.NodeAddress, Battery_result_dataPacket);
                    iDataHandler.getInstance().update_iPosData(Battery_result_dataPacket.WirepasData.WpNodeItem.NodeAddress, Battery_result_dataPacket);
                }
                else
                {
                    this.m_completeDataPackets[Battery_result_dataPacket.WirepasData.WpNodeItem.NodeAddress].WirepasData.VBat = Battery_result_dataPacket.WirepasData.VBat; // update only vbat data
                }
                this.mutex.ReleaseMutex();
            }
        }

        /* add datapacket containing location results to the list*/
        public void  AddResult(ElaSmiliDataPacket result_dataPacket)
        {
            if (this.mutex.WaitOne())
            {
                if (!m_completeDataPackets.ContainsKey(result_dataPacket.WirepasData.WpNodeItem.NodeAddress))
                {
                    this.m_completeDataPackets.Add(result_dataPacket.WirepasData.WpNodeItem.NodeAddress, result_dataPacket);         
                }
                else
                {
                    double vbat = this.m_completeDataPackets[result_dataPacket.WirepasData.WpNodeItem.NodeAddress].WirepasData.VBat;                   
                    this.m_completeDataPackets[result_dataPacket.WirepasData.WpNodeItem.NodeAddress] = result_dataPacket;
                    if (vbat != 0)
                    {
                        this.m_completeDataPackets[result_dataPacket.WirepasData.WpNodeItem.NodeAddress].WirepasData.VBat = vbat; //! check 
                    }
                }
                iDataHandler.getInstance().update_iPosData(result_dataPacket.WirepasData.WpNodeItem.NodeAddress, result_dataPacket);
                this.mutex.ReleaseMutex();
            }
        }
        /** getter on the counter of available dataPackets */
        public int CountAvailableDataPackets()
        {
            int rValues = 0;
            if (this.mutex.WaitOne())
            {
                rValues = this.m_completeDataPackets.Count;
                this.mutex.ReleaseMutex();
            }
            return rValues;
        }

        ///** get the last rows of Anchors available */
        public List<ElaSmiliDataPacket> GetLast()
        {
            List<ElaSmiliDataPacket> ElaSmiliDataPackets = new List<ElaSmiliDataPacket>();
            if (this.mutex.WaitOne())
            {
                foreach (ElaSmiliDataPacket dataPacket in this.m_completeDataPackets.Values)
                {
                    ElaSmiliDataPackets.Add(dataPacket);
                }
                this.m_completeDataPackets.Clear();
                this.mutex.ReleaseMutex();
            }
            return ElaSmiliDataPackets;
        }

        /** process incoming payload and transfer the calculated position*/
        public void ProcessPositioning()
        {
            try
            {
                foreach (KeyValuePair<uint, ElaSmiliDataPacket> entry in GetIncomingLocPacket())
                {
                    object loc_payload = WirepasPayloadFactory.GetInstance().GetLocationObject(entry.Value.WirepasData.WirepasLocData.WpLocPayload.ToByteArray());
                    AlgoWCL Position_W = new AlgoWCL(loc_payload);

                    try
                    {
                        // add to results 
                        this.AddResult(new ElaSmiliDataPacket { WirepasData = entry.Value.WirepasData, BleData = entry.Value.BleData, ElaTech = entry.Value.ElaTech, LocResult = new ElaLocationResult { AnchorsJson = Position_W.AnchorsDataJson, XCoordinate = Math.Round(Position_W.Xw, 2), YCoordinate = Math.Round(Position_W.Yw, 2) } });
                        // add to iDataHandler
                        iDataHandler.getInstance().update_iPosData(entry.Key, new ElaSmiliDataPacket { WirepasData = entry.Value.WirepasData, BleData = entry.Value.BleData, ElaTech = entry.Value.ElaTech, LocResult = new ElaLocationResult { AnchorsJson = Position_W.AnchorsDataJson, XCoordinate = Math.Round(Position_W.Xw, 2), YCoordinate = Math.Round(Position_W.Yw, 2) } });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + "LocHand02");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "LocHand01");
            }
        }
    }
}
