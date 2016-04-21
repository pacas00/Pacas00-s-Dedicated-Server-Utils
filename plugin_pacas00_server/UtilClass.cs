using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plugin_pacas00_server
{
    class UtilClass
    {
        public const string modName = "Pacas00's Dedicated Server Utils";
        public const string modId = "pacas00.server";

        public static void WriteLine(string s)
        {
            ServerConsole.DoServerString(UtilClass.modName + ": " + s);
        }

        public static void WriteLine(int x)
        {
            ServerConsole.DoServerString(UtilClass.modName + ": " + x);
        }

        public static void WriteLine(float f)
        {
            ServerConsole.DoServerString(UtilClass.modName + ": " + f);
        }




    }
}
