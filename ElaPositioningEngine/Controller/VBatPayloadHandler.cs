using ElaPositioningEngine.Model.Positioning;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using ElaWirepasLibrary.Model.Wirepas.Payload;
using ElaWirepasLibrary.Model.Wirepas.Payload.DataItems;

namespace ElaPositioningEngine.Controller
{
    public class VBatPayloadHandler
    {
        private Mutex m_mutexIncoming = new Mutex();
        /** internal collection of batVoltage data*/
        private readonly Dictionary<uint, Tuple<string, string>> IncomingVBatPayloadList = new Dictionary<uint, Tuple<string, string>>();
        public byte[] WirepasMessage { get; set; }

        /** function to add incoming payload to IncomingPayloadList*/
        public void AddIncomingVBatPayload(uint NodeID, string payload, string type)
        {
            if (this.m_mutexIncoming.WaitOne())
            {
                if (!IncomingVBatPayloadList.ContainsKey(NodeID))
                {
                    this.IncomingVBatPayloadList.Add(NodeID, new Tuple<string, string>(payload, type));
                }
                this.m_mutexIncoming.ReleaseMutex();
            }
        }

        public VBatPayloadHandler()
        {

        }

        /** List of incoming payload data*/
        public Dictionary<uint, Tuple<string, string>> GetIncomingVbatPayload()
        {
            Dictionary<uint, Tuple<string, string>> PayloadList = new Dictionary<uint, Tuple<string, string>>();
            if (this.m_mutexIncoming.WaitOne())
            {
                foreach (KeyValuePair<uint, Tuple<string, string>> entry in this.IncomingVBatPayloadList)
                {
                    PayloadList.Add(entry.Key, entry.Value);
                }
                this.IncomingVBatPayloadList.Clear();
                this.m_mutexIncoming.ReleaseMutex();
            }
            return PayloadList;
        }


        ///** process incoming payload and transfer the Vbat*/
        //public async void ProcessVdecoding()
        //{
        //    try
        //    {
        //        foreach (KeyValuePair<uint, Tuple<string, string>> entry in GetIncomingVbatPayload())
        //        {
        //            object vbat_payload = WirepasPayloadFactory.GetInstance().GetBatVoltObject(entry.Value.Item1);
        //            WirepasBatteryVoltage vbat_data = (vbat_payload as WirepasBatteryVoltage);
                    
        //            try
        //            {
        //                iDataHandler.getInstance().update_iVBatData(new ElaGrpcLibrary.BatVoltageData { NodeId = entry.Key, VBat = Math.Round(Convert.ToDouble(vbat_data.BatVoltResult.battery_voltage) / 1000, 2), Type = entry.Value.Item2 });

        //                if (null != ClientHandler.GetInstance().GetErtlsDataClient())
        //                {
        //                    await ClientHandler.GetInstance().GetErtlsDataClient().GetVBat(new ElaGrpcLibrary.BatVoltageData { NodeId = entry.Key, VBat = Math.Round(Convert.ToDouble(vbat_data.BatVoltResult.battery_voltage) / 1000,2), Type = entry.Value.Item2 });
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message + "VbatHand02");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message + "VbatHand01");   
        //    }
        //}
    }
}
