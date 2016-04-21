using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static plugin_pacas00_server.UtilClass;

namespace plugin_pacas00_server.GameInteractingClasses
{
    class WorldSettings
    {
        public static bool setupComplete = false;

        public static void Setup()
        {
            if (WorldScript.instance != null)
            {
                setupComplete = true;
            } else
            {
            }
            
        }


        public static string mName { get { return WorldScript.instance.mWorldData.mName; } set { WorldScript.instance.mWorldData.mName = value; } }

        public static float mrGravity { get { return WorldScript.instance.mWorldData.mrGravity; } set { WorldScript.instance.mWorldData.mrGravity = value; } }

        public static float mrMaxFallingSpeed { get { return WorldScript.instance.mWorldData.mrMaxFallingSpeed; } set { WorldScript.instance.mWorldData.mrMaxFallingSpeed = value; } }

        internal static WorldData getWorldData()
        {
            return WorldScript.instance.mWorldData;
        }

        public static float mrMovementSpeed { get { return WorldScript.instance.mWorldData.mrMovementSpeed; } set { WorldScript.instance.mWorldData.mrMovementSpeed = value; } }

        public static float mrJumpSpeed { get { return WorldScript.instance.mWorldData.mrJumpSpeed; } set { WorldScript.instance.mWorldData.mrJumpSpeed = value; } }

        public static eGameMode meGameMode { get { return WorldScript.instance.mWorldData.meGameMode; } }

        public static int mnResourceLevel { get { return WorldScript.instance.mWorldData.mnResourceLevel; } set { WorldScript.instance.mWorldData.mnResourceLevel = value; }  }

        public static int mnPowerLevel { get { return WorldScript.instance.mWorldData.mnPowerLevel; } set { WorldScript.instance.mWorldData.mnPowerLevel = value; } }

        public static int mnConveyorLevel { get { return WorldScript.instance.mWorldData.mnConveyorLevel; } set { WorldScript.instance.mWorldData.mnConveyorLevel = value; } }

        public static int mnDayLevel { get { return WorldScript.instance.mWorldData.mnDayLevel; } set { WorldScript.instance.mWorldData.mnDayLevel = value; }  }

        public static int mnMobLevel { get { return WorldScript.instance.mWorldData.mnMobLevel; } set { WorldScript.instance.mWorldData.mnMobLevel = value; }  }

        public static DifficultySettings.DeathEffect meDeathEffect { get { return WorldScript.instance.mWorldData.meDeathEffect; } set { WorldScript.instance.mWorldData.meDeathEffect = value; } }


    }








    }
