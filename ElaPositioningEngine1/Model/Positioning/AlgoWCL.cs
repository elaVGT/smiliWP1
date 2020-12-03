using ElaPositioningEngine.Model.RawData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using ElaPositioningEngine.Configuration.CalibrationModel;

namespace ElaPositioningEngine.Model.Positioning
{
    public class AlgoWCL
    {
        public double Xw;
        public double Yw;
        public string AnchorsDataJson;

        /* constructor */
        public AlgoWCL()
        {

        }

        /* constructor */
        public AlgoWCL(object m_loc_payload)
        {           
            Xw = 0;
            Yw = 0;

            ertlsRawData eRawData = new ertlsRawData(m_loc_payload);
            List<ertlsRawDataItem> sortedRawDataList = eRawData.getListSortRssi();
            double[] weight = new double[eRawData.RawDataList.Count];

            double g = CalibrationConfigModel.GetInstance().WclConfigsDict[sortedRawDataList[0].technology+ sortedRawDataList[0].environment].Degree; // get degree g depending upon tech and environment

            /** Algorithm: 
             * weight = 1/(d^g) 
             * Xt = sum(x*weight)/sum(weight) 
             * Yt = sum(y*weight)/sum(weight)
             */

            for (int i = 0; i < Math.Min(4, eRawData.RawDataList.Count); i++)     // todo count decides how many anchors take part in positioning
            {
                weight[i] = 1 / (Math.Pow(eRawData.RawDataList[i].eDistance, g));
                Xw += sortedRawDataList[i].x_coordinate * weight[i];
                Yw += sortedRawDataList[i].y_coordinate * weight[i];
            }

            Xw /= weight.Sum();
            Yw /= weight.Sum();
            AnchorsDataJson = JsonSerializer.Serialize(sortedRawDataList);  // json containing info of anchors contributing to localization
        }
    }
}
