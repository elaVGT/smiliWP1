using elaMicroservicesGrpc;
using ElaWirepas;
using System;
using System.Collections.Generic;
using System.Threading;

/**
 * \namespace ElaWirepasDataService.Controllers
 * \brief namespace associated to Wirepas data flow microservice
 */
namespace ElaWirepasDataService.Controllers
{
    /**
     * \class ElaWirepasDatapacketHandler
     * \brief hanling custom datapacket related to wirepas data messages 
     */
    public class ElaWirepasDatapacketHandler
    {
        private static ElaWirepasDatapacketHandler instance = null;

        /** internal mutex */
        private Mutex mutex = new Mutex(); 

        /** internal complete dataPackets list */
        private Dictionary<uint, ElaWirepasDataPacket> m_completeDataPackets = new Dictionary<uint, ElaWirepasDataPacket>();

        /** internal list of incoming data packets*/
        private List<ElaWirepasDataPacket> m_iDataPackets = new List<ElaWirepasDataPacket>();

        /** constructor */
        private ElaWirepasDatapacketHandler()
        {

        }

        /** singleton */
        public static ElaWirepasDatapacketHandler getInstance()
        {
            if (null == instance)
            {
                instance = new ElaWirepasDatapacketHandler();
            }
            return instance;
        }

        /** add dataPacket to the list */
        public void Add(ElaWirepasDataPacket dataPacket)
        {
            if (this.mutex.WaitOne())
            {
                this.m_iDataPackets.Add(dataPacket);
                this.mutex.ReleaseMutex();
            }
        }

        /** clear rows */
        public void clear()
        {
            if (this.mutex.WaitOne())
            {
                this.m_iDataPackets.Clear();
                this.mutex.ReleaseMutex();
            }
        }

        /** getter on the counter of available dataPackets */
        public int CountAvailableDataPackets()
        {
            int rValues = 0;
            if (this.mutex.WaitOne())
            {
                rValues = this.m_iDataPackets.Count;
                this.mutex.ReleaseMutex();
            }
            return rValues;
        }

        ///** get the last rows of Anchors available */
        public List<ElaWirepasDataPacket> GetLast()
        {
            List<ElaWirepasDataPacket> ElaWirepasDataPackets = new List<ElaWirepasDataPacket>();
            if (this.mutex.WaitOne())
            {
                foreach (ElaWirepasDataPacket dataPacket in this.m_iDataPackets)
                {
                    ElaWirepasDataPackets.Add(dataPacket);
                }
                this.m_iDataPackets.Clear();
                this.mutex.ReleaseMutex();
            }
            return ElaWirepasDataPackets;
        }

        ///** get the last rows of Anchors available */
        public ElaWirepasDataPacket GetCompleteDatapacket(ElaWirepasDataPacket dataPacket)
        {
            ElaWirepasDataPacket m_dataPacket = new ElaWirepasDataPacket();
            if (m_completeDataPackets.ContainsKey(dataPacket.WpNodeItem.NodeAddress))
            {
                m_dataPacket = m_completeDataPackets[dataPacket.WpNodeItem.NodeAddress];
                if (dataPacket.DataType == "Dia" && dataPacket.VBat != 0) // todo: constants
                {
                    m_dataPacket.VBat = dataPacket.VBat;
                }
                else if(dataPacket.DataType == "Sensor" && dataPacket.VBat != 0)
                {
                    m_dataPacket.VBat = dataPacket.VBat;
                }

                m_dataPacket.TimeStamp = dataPacket.TimeStamp;
                m_dataPacket.WirepasSensorData = dataPacket.WirepasSensorData;
                m_dataPacket.WirepasLocData = dataPacket.WirepasLocData;
                m_dataPacket.DataType = dataPacket.DataType;
                m_dataPacket.WpNodeItem.LocalName = GetNodeConfig(m_dataPacket.WpNodeItem.NodeAddress).LocalName;
                m_dataPacket.WpNodeItem.NodeRole = GetNodeConfig(m_dataPacket.WpNodeItem.NodeAddress).NodeRole;
                m_dataPacket.WpNodeItem.NodeType = GetNodeConfig(m_dataPacket.WpNodeItem.NodeAddress).NodeType;

                m_completeDataPackets[dataPacket.WpNodeItem.NodeAddress] = m_dataPacket;
            }
            else
            {
                m_dataPacket = dataPacket;
                m_dataPacket.WpNodeItem.LocalName = GetNodeConfig(m_dataPacket.WpNodeItem.NodeAddress).LocalName;
                m_dataPacket.WpNodeItem.NodeRole = GetNodeConfig(m_dataPacket.WpNodeItem.NodeAddress).NodeRole;
                m_dataPacket.WpNodeItem.NodeType = GetNodeConfig(m_dataPacket.WpNodeItem.NodeAddress).NodeType;

                m_completeDataPackets.Add(m_dataPacket.WpNodeItem.NodeAddress,m_dataPacket);
            }
            return m_dataPacket;
        }
        private ElaWirepasNodeItem GetNodeConfig(uint nodeAddress)
        {
            ElaWirepasNodeItem m_elaWirepasNodeItem = new ElaWirepasNodeItem();
            //todo
            if(nodeAddress != 1)
            {
                m_elaWirepasNodeItem.NodeRole = "Todo";
                m_elaWirepasNodeItem.NodeType = "Node";
                m_elaWirepasNodeItem.LocalName = "Tag_" + nodeAddress;
            }
            else
            {
                m_elaWirepasNodeItem.NodeRole = "Todo";
                m_elaWirepasNodeItem.NodeType = "Sink";
                m_elaWirepasNodeItem.LocalName = "Sink_01";
            }

            return m_elaWirepasNodeItem;
        }
    }
}
