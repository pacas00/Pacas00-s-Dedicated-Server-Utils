using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static plugin_pacas00_server.UtilClass;

namespace plugin_pacas00_server.commands
{
    class CustomConsoleCommands : FortressCraftMod
    {

        public void Start()
        {
            this.CreateCommand("resourceLevel", "Set World Resource Level", CmdParameterType.String, "ResourceDifficulty");
            this.CreateCommand("powerLevel", "Set World Power Level", CmdParameterType.String, "PowerDifficulty");
            this.CreateCommand("dayNightCycle", "Set Day/Night Cycle", CmdParameterType.String, "DayNightCycle");
            this.CreateCommand("convSpeed", "Set Conveyor Speed", CmdParameterType.String, "ConvSpeed");
            this.CreateCommand("mobDifficulty", "Set Mob Difficulty", CmdParameterType.String, "MobDifficulty");
            this.CreateCommand("deathMode", "Set the Death Mode", CmdParameterType.String, "DeathEffect");


            this.CreateCommand("ServerName", "Set what shows as the name in the server browser", CmdParameterType.String, "ServerName");
            this.CreateCommand("MaxPlayers", "Set Server's Max Players", CmdParameterType.String, "MaxPlayers");

            this.CreateCommand("addRP", "Add Research Points", CmdParameterType.String, "AddResearchPoints");
        }

        public static void SaveWorldSettings()
        {
            DifficultySettings.SetSettingsFromWorldData(WorldScript.instance.mWorldData);
            WorldScript.instance.SaveWorldSettings();
        }

        private void ServerName(string parameters)
        {
            if (!(WorldScript.mbIsServer == true))
            {

                //We only run commands on the server.
                global::Console.LogTargetFunction("Only the Dedicated Server can run this command.", ConsoleMessageType.Trace);
                return;
            }


            Settings.Instance.settings.ServerName = parameters;

            Settings.SaveSettings();
            Settings.ApplyServerSettings();

            global::Console.LogTargetFunction("Set Server Name to [" + parameters + "].", ConsoleMessageType.Trace);

        }

        private void MaxPlayers(string parameters)
        {
            if (!(WorldScript.mbIsServer == true))
            {

                //We only run commands on the server.
                global::Console.LogTargetFunction("Only the Dedicated Server can run this command.", ConsoleMessageType.Trace);
                return;
            }

            int num = -1;
            if (int.TryParse(parameters, out num))
            {
                Settings.Instance.settings.MaxPlayerCount = (num);

                Settings.SaveSettings();
                Settings.ApplyServerSettings();

                global::Console.LogTargetFunction("Set MaxPlayers to [" + parameters + "].", ConsoleMessageType.Trace);
            }
            else
            {
                global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only numbers are supported", ConsoleMessageType.Trace);
            }
        }

        private void PowerDifficulty(string parameters)
        {
            if (!(WorldScript.mbIsServer == true))
            {

                //We only run commands on the server.
                global::Console.LogTargetFunction("Only the Dedicated Server can run this command.", ConsoleMessageType.Trace);
                return;
            }

            int num = -1;
            if (int.TryParse(parameters, out num))
            {
                if (num == 0 || num == 1)
                {
                    WorldScript.instance.mWorldData.mnPowerLevel = num;
                    SaveWorldSettings();
                    global::Console.LogTargetFunction("Power Difficulty set to [" + parameters + "], Restart Server to take effect.", ConsoleMessageType.Trace);
                }
                else
                {
                    global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Plentiful) and 1 (Scarce) are supported", ConsoleMessageType.Trace);
                }
            }
            else
            {
                global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Plentiful) and 1 (Scarce) are supported", ConsoleMessageType.Trace);
            }
        }

        private void ResourceDifficulty(string parameters)
        {
            if (!(WorldScript.mbIsServer == true))
            {

                //We only run commands on the server.
                global::Console.LogTargetFunction("Only the Dedicated Server can run this command.", ConsoleMessageType.Trace);
                return;
            }

            int num = -1;
            if (int.TryParse(parameters, out num))
            {
                if (num == 0 || num == 1 || num == 2 || num == 3)
                {
                    WorldScript.instance.mWorldData.mnResourceLevel = num;
                    SaveWorldSettings();
                    global::Console.LogTargetFunction("Resource Difficulty set to [" + parameters + "], Restart Server to take effect.", ConsoleMessageType.Trace);
                }
                else
                {
                    global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Plentiful), 1 (Scarce), 2 (Greg), and 3 (Casual) are supported", ConsoleMessageType.Trace);
                }
            }
            else
            {
                global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Plentiful), 1 (Scarce), 2 (Greg), and 3 (Casual) are supported", ConsoleMessageType.Trace);
            }
        }

        private void DayNightCycle(string parameters)
        {
            if (!(WorldScript.mbIsServer == true))
            {

                //We only run commands on the server.
                global::Console.LogTargetFunction("Only the Dedicated Server can run this command.", ConsoleMessageType.Trace);
                return;
            }

            int num = -1;
            if (int.TryParse(parameters, out num))
            {
                if (num == 0 || num == 1 || num == 2)
                {
                    WorldScript.instance.mWorldData.mnDayLevel = num;
                    SaveWorldSettings();
                    global::Console.LogTargetFunction("Day Night Cycle set to [" + parameters + "], Restart Server to take effect.", ConsoleMessageType.Trace);
                }
                else
                {
                    global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Eternal Day), 1 (Normal) and 2 (Eternal Night) are supported", ConsoleMessageType.Trace);
                }
            }
            else
            {
                global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Eternal Day), 1 (Normal) and 2 (Eternal Night) are supported", ConsoleMessageType.Trace);
            }
        }

        private void DeathEffect(string parameters)
        {
            if (!(WorldScript.mbIsServer == true))
            {

                //We only run commands on the server.
                global::Console.LogTargetFunction("Only the Dedicated Server can run this command.", ConsoleMessageType.Trace);
                return;
            }

            int num = -1;
            if (int.TryParse(parameters, out num))
            {
                if (num == 0 || num == 1 || num == 2 || num == 3 || num == 4)
                {
                    WorldScript.instance.mWorldData.meDeathEffect = (DifficultySettings.DeathEffect)num;
                    SaveWorldSettings();
                    global::Console.LogTargetFunction("Death Effect set to [" + parameters + "], Restart Server to take effect.", ConsoleMessageType.Trace);
                }
                else
                {
                    global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Easy), 1 (Clumsy), 2 (IronMan), 3 (SquishCore) and 4 (HardCore) are supported", ConsoleMessageType.Trace);
                }
            }
            else
            {
                global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Easy), 1 (Clumsy), 2 (IronMan), 3 (SquishCore) and 4 (HardCore) are supported", ConsoleMessageType.Trace);
            }
        }

        private void MobDifficulty(string parameters)
        {
            if (!(WorldScript.mbIsServer == true))
            {

                //We only run commands on the server.
                global::Console.LogTargetFunction("Only the Dedicated Server can run this command.", ConsoleMessageType.Trace);
                return;
            }

            int num = -1;
            if (int.TryParse(parameters, out num))
            {
                if (num == 0 || num == 1 || num == 2)
                {
                    WorldScript.instance.mWorldData.mnMobLevel = num;
                    SaveWorldSettings();
                    global::Console.LogTargetFunction("Mob Difficulty set to [" + parameters + "], Restart Server to take effect.", ConsoleMessageType.Trace);
                }
                else
                {
                    global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Trivial), 1 (Normal) and 2 (Hard) are supported", ConsoleMessageType.Trace);
                }
            }
            else
            {
                global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Trivial), 1 (Normal) and 2 (Hard) are supported", ConsoleMessageType.Trace);
            }
        }

        private void ConvSpeed(string parameters)
        {
            if (!(WorldScript.mbIsServer == true))
            {

                //We only run commands on the server.
                global::Console.LogTargetFunction("Only the Dedicated Server can run this command.", ConsoleMessageType.Trace);
                return;
            }

            int num = -1;
            if (int.TryParse(parameters, out num))
            {
                if (num == 0 || num == 1)
                {
                    WorldScript.instance.mWorldData.mnConveyorLevel = num;
                    SaveWorldSettings();
                    global::Console.LogTargetFunction("Conveyor Speed set to [" + parameters + "], Restart Server to take effect.", ConsoleMessageType.Trace);
                }
                else
                {
                    global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Fast) and 1 (Slow) are supported", ConsoleMessageType.Trace);
                }
            }
            else
            {
                global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Fast) and 1 (Slow) are supported", ConsoleMessageType.Trace);
            }
        }

        private void AddResearchPoints(string parameters)
        {
            if (!(WorldScript.mbIsServer == true))
            {

                //We only run commands on the server.
                global::Console.LogTargetFunction("Only the Dedicated Server can run this command.", ConsoleMessageType.Trace);
                return;
            }

            int num = -1;
            if (int.TryParse(parameters, out num))
            {
                PlayerResearch.mSharedResearch.GiveResearchPoints(num);
                global::Console.LogTargetFunction("Added [" + parameters + "] Research Points.", ConsoleMessageType.Trace);
            }
            else
            {
                global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only numbers are supported", ConsoleMessageType.Trace);
            }
        }



        private void CreateCommand(string command, string description, CmdParameterType type, string functionName)
        {
            global::Console.AddCommand(new ConsoleCommand(command, description, type, base.gameObject, functionName));
        }
    }
}
