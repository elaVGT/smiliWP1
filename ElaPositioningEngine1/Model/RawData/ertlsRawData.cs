using ElaPositioningEngine.Model.Positioning;
using ElaPositioningEngine.Model.RawData;
using eRTLS.Models.Positioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElaPositioningEngine.Configuration.AnchorModel;
using ElaPositioningEngine.Configuration;
using ElaWirepasLibrary.Model.Wirepas.Payload;
using ElaWirepasLibrary.Model.Wirepas.Payload.DataItems;

namespace ElaPositioningEngine.Model.RawData
{
    class ertlsRawData
    {
        //private WirepasLocation payloadObject;

        /** constructor */
        public ertlsRawData()
        {

        }

        #region accessors definition
        public List<ertlsRawDataItem> RawDataList { get; } = new List<ertlsRawDataItem>();
        #endregion

        /** constructor */
        public ertlsRawData(object loc_payload)
        {
            updateRawData(loc_payload);
        }

        /** sort list by rssi*/
        public List<ertlsRawDataItem> getListSortRssi()
        {
            List<ertlsRawDataItem> sortedByRssi = this.RawDataList;
            sortedByRssi.Sort((x, y) => x.rssi.CompareTo(y.rssi));

            // remove anchors installed on other floors
            for (int j = 1; j < sortedByRssi.Count; j++)
            {
                if (!sortedByRssi[0].floor.Equals(sortedByRssi[j].floor))
                {
                    int iTargetIndex = j;
                    sortedByRssi.RemoveAt(iTargetIndex);
                }
            }
            return sortedByRssi;
        }

        public void updateRawData(object loc_payload)
        {
            try
            {
                WirepasLocation location = (loc_payload as WirepasLocation);
                foreach (WirepasLocationDataItem Item in location.LocationResults)
                {
                    foreach (AnchorConfiguration anchor in AnchorModel.GetInstance().Anchors)
                    {
                        if (anchor.Id == Item.node_address && anchor.Type.Substring(0, 7) != "Deleted")  // exclude deleted anchor
                        {
                            ertlsRawDataItem RawData = new ertlsRawDataItem();
                            RawData.node_address = Item.node_address;
                            RawData.rssi = Item.rssi;
                            RawData.x_coordinate = anchor.X;
                            RawData.y_coordinate = anchor.Y;
                            RawData.technology = anchor.Technology;
                            RawData.environment = anchor.Environment;
                            RawData.floor = anchor.Floor;
                            EstimateDistance ed = new EstimateDistance(RawData.technology + RawData.environment, RawData);  // estimate distance based on Tech and environment
                            RawData.eDistance = ed.eDist; // calculate eDist using eStimatedDistance() 
                            this.RawDataList.Add(RawData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "RawData01");
            }
        }
    }
}
