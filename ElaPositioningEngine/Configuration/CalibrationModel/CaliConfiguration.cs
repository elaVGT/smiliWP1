using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElaPositioningEngine.Configuration.CalibrationModel
{

    [Serializable]
    public class CaliConfiguration
    {

        #region accessors
        public string Technology { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
        public double C0 { get; set; } = 0.0;
        public double C1 { get; set; } = 0.0;
        public double C2 { get; set; } = 0.0;
        public double C3 { get; set; } = 0.0;
        public double C4 { get; set; } = 0.0;

        #endregion

        /** default cosntructor */
        public CaliConfiguration()
        {

        }
    }
}
