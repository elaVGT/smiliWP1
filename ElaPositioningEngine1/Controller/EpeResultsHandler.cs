using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ElaPositioningEngine.gRPC;
using ElaPositioningEngine.Model.Positioning;
using ElaSmili;

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
        /** internal complete dataPackets list */
        private Dictionary<uint, ElaSmiliDataPacket> m_completeDataPackets = new Dictionary<uint, ElaSmiliDataPacket>();
        /** internal list of incoming data packets*/
        private List<ElaSmiliDataPacket> m_iDataPackets = new List<ElaSmiliDataPacket>();

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

        /** add dataPacket to the list */
        public void Add(ElaSmiliDataPacket dataPacket)
        {
            if (this.mutex.WaitOne())
            {
                this.m_iDataPackets.Add(dataPacket);
                this.mutex.ReleaseMutex();
            }
        }

        ///** get the last rows of Anchors available */
        public List<ElaSmiliDataPacket> GetLast()
        {
            List<ElaSmiliDataPacket> ElaSmiliDataPackets = new List<ElaSmiliDataPacket>();
            if (this.mutex.WaitOne())
            {
                foreach (ElaSmiliDataPacket dataPacket in this.m_iDataPackets)
                {
                    ElaSmiliDataPackets.Add(dataPacket);
                }
                this.m_iDataPackets.Clear();
                this.mutex.ReleaseMutex();
            }
            return ElaSmiliDataPackets;
        }

        /** process incoming payload and transfer the calculated position*/
        public async void ProcessPositioning()
        {
            try
            {
                //foreach (KeyValuePair<uint, Tuple<string, string>> entry in GetIncomingLocPayload())
                //{
                //    object loc_payload = WirepasPayloadFactory.GetInstance().GetLocationObject(entry.Value.Item1);
                //    AlgoWCL Position_W = new AlgoWCL(loc_payload);

                //    try
                //    {
                //        iDataHandler.getInstance().update_iPosData(new ElaGrpcLibrary.PositionData { NodeId = entry.Key, PositionX = Math.Round(Position_W.Xw, 2), PositionY = Math.Round(Position_W.Yw, 2), Type = entry.Value.Item2, AnchorsJson = Position_W.AnchorsDataJson });

                //        if (null != ClientHandler.GetInstance().GetErtlsDataClient())
                //        {
                //            await ClientHandler.GetInstance().GetErtlsDataClient().GetPosition(new ElaGrpcLibrary.PositionData { NodeId = entry.Key, PositionX = Math.Round(Position_W.Xw, 2), PositionY = Math.Round(Position_W.Yw, 2), Type = entry.Value.Item2, AnchorsJson = Position_W.AnchorsDataJson });
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message + "LocHand02");
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "LocHand01");
            }
        }
    }
}
