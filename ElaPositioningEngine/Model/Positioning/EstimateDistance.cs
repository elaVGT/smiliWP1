using ElaPositioningEngine.Configuration.CalibrationModel;
using ElaPositioningEngine.Model.RawData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRTLS.Models.Positioning
{
    class EstimateDistance
    {
        public double eDist = 0;

        /* constructor */
        public EstimateDistance()
        {

        }

        /* constructor */ // estimate distance based on Tech and environment
        public EstimateDistance(string TechEnvi, ertlsRawDataItem m_rawData)
        {
            eDist = Math.Abs(
                      CalibrationConfigModel.GetInstance().CalibrationsDict[TechEnvi].C0 * Math.Pow(m_rawData.rssi, 4)
                    + CalibrationConfigModel.GetInstance().CalibrationsDict[TechEnvi].C1 * Math.Pow(m_rawData.rssi, 3)
                    + CalibrationConfigModel.GetInstance().CalibrationsDict[TechEnvi].C2 * Math.Pow(m_rawData.rssi, 2)
                    + CalibrationConfigModel.GetInstance().CalibrationsDict[TechEnvi].C3 * m_rawData.rssi
                    + CalibrationConfigModel.GetInstance().CalibrationsDict[TechEnvi].C4
                    );
        }
    }
}
