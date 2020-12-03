using ElaPositioningEngine.Model.Positioning;
using ElaWirepasLibrary.Model.Wirepas.Payload;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace ElaPositioningEngine.Controller
{
    public class DiagPayloadHandler
    {
        private Mutex m_mutexIncoming = new Mutex();
        /** internal collection of digPayload data*/
        private readonly Dictionary<uint, Tuple<string, string>> IncomingDiagPayloadList = new Dictionary<uint, Tuple<string, string>>();
        public byte[] WirepasMessage { get; set; }

        /** function to add incoming payload to IncomingPayloadList*/
        public void AddIncomingDiagPayload(uint NodeID, string payload, string type)
        {
            if (this.m_mutexIncoming.WaitOne())
            {
                if (!IncomingDiagPayloadList.ContainsKey(NodeID))
                {
                    this.IncomingDiagPayloadList.Add(NodeID, new Tuple<string, string>(payload, type));
                }
                this.m_mutexIncoming.ReleaseMutex();
            }
        }

        /** List of incoming payload data*/
        public Dictionary<uint, Tuple<string, string>> GetIncomingDiagPayload()
        {
            Dictionary<uint, Tuple<string, string>> PayloadList = new Dictionary<uint, Tuple<string, string>>();
            if (this.m_mutexIncoming.WaitOne())
            {
                foreach (KeyValuePair<uint, Tuple<string, string>> entry in this.IncomingDiagPayloadList)
                {
                    PayloadList.Add(entry.Key, entry.Value);
                }
                this.IncomingDiagPayloadList.Clear();
                this.m_mutexIncoming.ReleaseMutex();
            }
            return PayloadList;
        }

        public DiagPayloadHandler()
        {

        }

        /** process incoming payload and transfer dignostics info*/
        //public async void ProcessVdecoding()
        //{
        //    try
        //    {
        //        foreach (KeyValuePair<uint, Tuple<string, string>> entry in GetIncomingDiagPayload())
        //        {
        //            object diag_payload = WirepasPayloadFactory.GetInstance().GetDiagnosticsObject(entry.Value.Item1, WirepasLibraryConstant.SOURCE_ENDPOINT_NODE_DIAGNOSTIC);
        //            WirepasDiagnostics diag_data = (diag_payload as WirepasDiagnostics);

        //            try
        //            {
        //                iDataHandler.getInstance().update_iVBatData(new ElaGrpcLibrary.BatVoltageData { NodeId = entry.Key, VBat = Math.Round(Convert.ToDouble(diag_data.NodeDiagResults.Voltage) / 100, 2), Type = entry.Value.Item2 });

        //                if (null != ClientHandler.GetInstance().GetErtlsDataClient())
        //                {
        //                    await ClientHandler.GetInstance().GetErtlsDataClient().GetVBat(new ElaGrpcLibrary.BatVoltageData

        //                    { NodeId = entry.Key, VBat = Math.Round(2.00 + Convert.ToDouble(diag_data.NodeDiagResults.Voltage) / 100, 2), Type = entry.Value.Item2 });
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message + "DiagP01");
        //            }
        //        }
        //    }
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message + "DiagP01");   
            //}
        //}
    }
}
