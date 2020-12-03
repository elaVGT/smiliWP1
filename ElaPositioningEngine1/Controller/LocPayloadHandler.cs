using ElaPositioningEngine.Model.Positioning;
using ElaSmili;
using ElaWirepasLibrary.Model.Wirepas.Payload;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace ElaPositioningEngine.Controller
{
    public class LocPayloadHandler
    {
        private Mutex m_mutexIncoming = new Mutex();
        /** internal collection of locPayload data*/
        private readonly Dictionary<uint, Tuple<byte[], string>> IncomingLocPayloadList = new Dictionary<uint, Tuple<byte[], string>>();
        public byte[] WirepasMessage { get; set; }

        /** function to add incoming payload to IncomingPayloadList*/
        public void AddIncomingLocPayload(uint NodeID, byte[] payload, string type)
        {
            if (this.m_mutexIncoming.WaitOne())
            {
                if (!IncomingLocPayloadList.ContainsKey(NodeID))
                {
                    this.IncomingLocPayloadList.Add(NodeID, new Tuple<byte[], string>(payload, type));
                }
                this.m_mutexIncoming.ReleaseMutex();
            }
        }

        /** Collection of incoming payload data*/
        public Dictionary<uint, Tuple<byte[], string>> GetIncomingLocPayload()
        {
            Dictionary<uint, Tuple<byte[], string>> PayloadList = new Dictionary<uint, Tuple<byte[], string>>();
            if (this.m_mutexIncoming.WaitOne())
            {
                foreach (KeyValuePair<uint, Tuple<byte[], string>> entry in this.IncomingLocPayloadList)
                {
                    PayloadList.Add(entry.Key, entry.Value);
                }
                this.IncomingLocPayloadList.Clear();
                this.m_mutexIncoming.ReleaseMutex();
            }
            return PayloadList;
        }

        public LocPayloadHandler()
        {

        }

        /** process incoming payload and transfer the calculated position*/
        public async void ProcessPositioning()
        {
            try
            {
                foreach (KeyValuePair<uint, Tuple<byte[], string>> entry in GetIncomingLocPayload())
                {
                    object loc_payload = WirepasPayloadFactory.GetInstance().GetLocationObject(entry.Value.Item1);
                    AlgoWCL Position_W = new AlgoWCL(loc_payload);

                    try
                    {
                        iDataHandler.getInstance().update_iPosData(new ElaLocationResult { NodeItem = new ElaSmiliNodeItem { NodeAddress = entry.Key, NodeType = entry.Value.Item2 } , XCoordinate = Math.Round(Position_W.Xw, 2), YCoordinate = Math.Round(Position_W.Yw, 2), AnchorsJson = Position_W.AnchorsDataJson });                   
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
