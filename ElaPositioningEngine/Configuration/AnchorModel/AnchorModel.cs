using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ElaPositioningEngine.Configuration.AnchorModel
{
    [Serializable]
    public class AnchorModel
    {
        #region accessors
        public List<AnchorConfiguration> Anchors { get; set; } = new List<AnchorConfiguration>();
        #endregion

        /** singleton implementation */
        private static AnchorModel instance = null;

        // constructor
        private AnchorModel()
        {

        }

        /** singleton   */
        public static AnchorModel GetInstance()
        {
            if (null == instance)
            {
                instance = new AnchorModel();
            }
            return instance;
        }

        public void add(AnchorModel model)
        {
            foreach(AnchorConfiguration anchor in model.Anchors)
            {
                this.Anchors.Add(anchor);
            }
        }

        public bool LoadAnchorsConfig(string AnchorsConfig_Json)
        {
            try
            {
                AnchorModel Anchorconfig = (AnchorModel)JsonSerializer.Deserialize(AnchorsConfig_Json, typeof(AnchorModel));
                this.add(Anchorconfig);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "AnchMod01");
                return false;
            }
        }
    }
}
