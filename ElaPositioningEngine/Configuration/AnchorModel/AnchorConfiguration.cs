using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ElaPositioningEngine.Configuration.AnchorModel
{
    [Serializable]
    public class AnchorConfiguration
    {

        #region accessors
        public string Type { get; set; } = string.Empty;
        public string Technology { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
        public string Floor { get; set; } = string.Empty;
        public uint Id { get; set; } = 0;
        public double X { get; set; } = 0.0;
        public double Y { get; set; } = 0.0;
        #endregion

        /** default cosntructor */
        public AnchorConfiguration()
        {

        }
    }
}
