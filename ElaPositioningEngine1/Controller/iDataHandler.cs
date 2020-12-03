using ElaSmili;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ElaPositioningEngine.Controller
{
    public class iDataHandler
    {
        private Mutex m_mutexIncoming = new Mutex();
        /** internal collection of position data*/
        private readonly Dictionary<uint, ElaLocationResult> iPosDataList = new Dictionary<uint, ElaLocationResult>();
        /** internal collection of Vbat data*/
        private readonly Dictionary<uint, ElaLocationResult> iVBatDataList = new Dictionary<uint, ElaLocationResult>();

        /* singleton */
        public static iDataHandler instance = null;

        public static iDataHandler getInstance()
        {
            if (null == instance)
            {
                instance = new iDataHandler();
            }
            return instance;
        }

        /** function to update internal collection of position data*/
        public void update_iPosData(ElaLocationResult positionData)
        {
            if (this.m_mutexIncoming.WaitOne())
            {
                if (iPosDataList.ContainsKey(positionData.NodeItem.NodeAddress))
                {
                    iPosDataList[positionData.NodeItem.NodeAddress] = positionData;
                }
                else
                {
                    iPosDataList.Add(positionData.NodeItem.NodeAddress, positionData);
                }
                this.m_mutexIncoming.ReleaseMutex();
            }
        }

        ///** function to update internal collection of Vbat data*/
        //public void update_iVBatData(ElaGrpcLibrary.BatVoltageData batVoltageData)
        //{
        //    if (this.m_mutexIncoming.WaitOne())
        //    {
        //        if (iVBatDataList.ContainsKey(batVoltageData.NodeId))
        //        {
        //            iVBatDataList[batVoltageData.NodeId] = batVoltageData;
        //        }
        //        else
        //        {
        //            iVBatDataList.Add(batVoltageData.NodeId, batVoltageData);
        //        }
        //        this.m_mutexIncoming.ReleaseMutex();
        //    }
        //}

        private iDataHandler()
        {

        }

        ///** transfer internal collection of pos data*/
        //public async void iPosDataTransport()
        //{
        //    await Task.Delay(500);
        //    try
        //    {
        //        Dictionary<uint, ElaLocationResult> m_iPosDataList = this.iPosDataList;
        //        foreach (KeyValuePair<uint, ElaLocationResult> pos_item  in m_iPosDataList)
        //        {
        //            try
        //            {
        //                if (null != ClientHandler.GetInstance().GetRawDataClient())
        //                {
        //                    await ClientHandler.GetInstance().GetRawDataClient.GetPosition(new ElaLocationResult
        //                    { NodeItem = new ElaSmiliNodeItem { NodeAddress = pos_item.Key, NodeType = pos_item.Value.NodeItem.NodeType }, XCoordinate = pos_item.Value.XCoordinate, YCoordinate = pos_item.Value.YCoordinate, AnchorsJson = pos_item.Value.AnchorsJson });
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message + "idata4");
        //            }

        //            await Task.Delay(50);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message + "idata3");   
        //    }
        //}

        ///** transfer internal collection of Vbat datan*/
        //public async void iVbatDataTransport()
        //{
        //    await Task.Delay(1000);
        //    try
        //    {
        //        Dictionary<uint, ElaGrpcLibrary.BatVoltageData> m_iVBatDataList = this.iVBatDataList; 
        //        foreach (KeyValuePair<uint, ElaGrpcLibrary.BatVoltageData> vbat_item in m_iVBatDataList)
        //        {
        //            try
        //            {
        //                if (null != ClientHandler.GetInstance().GetErtlsDataClient())
        //                {
        //                    await ClientHandler.GetInstance().GetErtlsDataClient().GetVBat(new ElaGrpcLibrary.BatVoltageData
        //                    { NodeId = vbat_item.Key, VBat = vbat_item.Value.VBat, Type = vbat_item.Value.Type });
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message + "iData1");
        //            }

        //            await Task.Delay(50);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message +"iData2");
        //    }
        //}
    }
}
