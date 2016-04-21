using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DifficultySettings;

namespace plugin_pacas00_server.GameInteractingClasses.Enums
{
    class WorldSettingsEnums
    {
        //public enum eMobDifficulty
        //{
        //    Easy,
        //    Normal,
        //    Hard
        //}

        //public enum DeathEffect
        //{
        //    Easy,
        //    Clumsy,
        //    IronMan,
        //    SquishCore,
        //    HardCore
        //}

        public enum eResourceOptions
        {
            Plentiful = 0,
            Scarce = 1,
            Greg = 2,
            Casual = 3
        }

        public enum ePowerOptions
        {
            Plentiful = 0,
            Scarce = 1
        }

        public enum eConveyorOptions
        {
            Fast = 0,
            Slow = 1
        }

        public enum eDayNightOptions
        {
            Eternal_Day = 0,
            Normal = 1,
            Eternal_Night = 2
        }

        public static eDayNightOptions Parse(string s)
        {
            return (eDayNightOptions) Enum.Parse(typeof(eDayNightOptions), s.Replace(" ", "_"));
        }
    }
}
