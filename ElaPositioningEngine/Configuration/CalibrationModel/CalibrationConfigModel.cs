using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ElaPositioningEngine.Configuration.CalibrationModel
{
    [Serializable]
    public class CalibrationConfigModel
    {
        #region accessors
        public List<CaliConfiguration> Calibrations { get; set; } = new List<CaliConfiguration>();
        public List<WclConfiguration> WclConfigs { get; set; } = new List<WclConfiguration>();
        // dictionary to distinguish calibrtions for different combinations of technologies and environments
        public Dictionary<string, CaliConfiguration> CalibrationsDict { get; set; } = new Dictionary<string, CaliConfiguration>();
        // dictionary to distinguish WclConfigurations for different combinations of technologies and environments
        public Dictionary<string, WclConfiguration> WclConfigsDict { get; set; } = new Dictionary<string, WclConfiguration>();
        #endregion

        /** singleton implementation */
        private static CalibrationConfigModel instance = null;

        // constructor              
        private CalibrationConfigModel()
        {

        }

        /** singleton   */
        public static CalibrationConfigModel GetInstance()
        {
            if (null == instance)
            {
                instance = new CalibrationConfigModel();
            }
            return instance;
        }

        public bool LoadCaliConfig(string CaliConfig_Json)
        {
            try
            {
                CalibrationConfigModel Cali_config = (CalibrationConfigModel)JsonSerializer.Deserialize(CaliConfig_Json, typeof(CalibrationConfigModel));
                this.Set(Cali_config);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "CaliConfig01");
                return false;
            }
        }

        // constructor
        public void Set(CalibrationConfigModel model)
        {
            foreach (CaliConfiguration cali in model.Calibrations)
            {
                if (!CalibrationsDict.ContainsKey(cali.Technology + cali.Environment))
                {
                    this.CalibrationsDict.Add(cali.Technology + cali.Environment, cali);
                }
            }

            foreach (WclConfiguration wclConfig in model.WclConfigs)
            {
                if (!WclConfigsDict.ContainsKey(wclConfig.Technology + wclConfig.Environment))
                {
                    this.WclConfigsDict.Add(wclConfig.Technology + wclConfig.Environment, wclConfig);
                }
            }
        }
    }
}
