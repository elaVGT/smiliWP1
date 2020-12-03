using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElaPositioningEngine.Configuration.CalibrationModel
{

    [Serializable]
    public class WclConfiguration
    {

        #region accessors
        public string Technology { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
        public double Degree { get; set; } = 0.00;
        #endregion

        /** default cosntructor */
        public WclConfiguration()
        {

        }
    }
}
