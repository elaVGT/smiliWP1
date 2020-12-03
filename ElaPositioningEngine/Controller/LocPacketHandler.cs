using ElaPositioningEngine.Model.Positioning;
using ElaSmili;
using ElaWirepasLibrary.Model.Wirepas.Payload;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace ElaPositioningEngine.Controller
{
    public class LocPacketHandler
    {
        private static LocPacketHandler instance = null;
        private Mutex mutex = new Mutex();

        public byte[] WirepasMessage { get; set; }

        public static LocPacketHandler GetInstance()
        {
            if (null == instance)
            {
                instance = new LocPacketHandler();
            }
            return instance;
        }

        public LocPacketHandler()
        {

        } 
    }
}
