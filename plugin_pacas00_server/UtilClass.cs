﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace plugin_pacas00_server
{
    public class UtilClass
    {
        public const string modName = "Pacas00's Server Utils";
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

        public static void WriteLine(bool b)
        {
            ServerConsole.DoServerString(UtilClass.modName + ": " + b);
        }

        internal static void WriteLine(Exception ex)
        {
            ServerConsole.DoServerString(UtilClass.modName + ": " + ex.Message);
            ServerConsole.DoServerString(ex.StackTrace);
        }

    }
}
