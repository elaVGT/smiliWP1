using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElaPositioningEngine.Model.RawData
{
    public class ertlsRawDataItem
    {
        public double node_address { get; set; }
        public double rssi { get; set; }
        public double x_coordinate { get; set; }
        public double y_coordinate { get; set; }
        public double eDistance { get; set; }
        public string floor { get; set; }
        public string technology { get; set; }
        public string environment { get; set; }
    }
}
