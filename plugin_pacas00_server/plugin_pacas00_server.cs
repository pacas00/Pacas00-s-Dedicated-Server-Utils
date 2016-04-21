using plugin_pacas00_server.GameInteractingClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static plugin_pacas00_server.UtilClass;

namespace plugin_pacas00_server
{
    public class plugin_pacas00_server : FortressCraftMod
    {

        //ServerConsole - real console
        //GameManager - Game Stats

        public plugin_pacas00_server()
        {
            
        }

        public static int LFU_Updates_Per_Second = 5; // assuming that the LFU updates happen 5 times a second.

        public static int triggerCounter = 0;

#if DEBUG
        public static int triggerCounterMax = LFU_Updates_Per_Second * 15; //debug fast update every 15 seconds
#else
        public static int triggerCounterMax = LFU_Updates_Per_Second * 60; //release update every 1 mins
#endif

        bool networkSetupComplete = false;
        bool worldSetupComplete = false;
        public static string workingDir;
        
        public override void LowFrequencyUpdate()
        {
            if (worldSetupComplete == false)
                if (WorldScript.instance != null)
                {
                    if (WorldScript.instance.mWorldData != null)
                    {
                        worldSetupComplete = true;
                        WorldSettings.Setup();
                        Settings.SaveSettings();
                    }
                }

            if (networkSetupComplete == false)
                if (NetworkManager.instance != null)
                {
                    if (NetworkManager.instance.mServerThread != null)
                    {
                        networkSetupComplete = true;
                        //Network Side
                        Settings.ApplyServerSettings();
                    }
                }

            if (triggerCounter < triggerCounterMax)
            {
                triggerCounter++;
            }
            else
            {
                try
                {
                    triggerCounter = 0;
                    //List<NetworkServerConnection> playerConns = NetworkManager.instance.mServerThread.GetConnections();

                    //if (playerConns.Count > 0)
                    //{
                    //    Players.clearPlayers();
                    //    foreach (NetworkServerConnection conn in playerConns)
                    //    {

                    //        ServerConsole.DoServerString(conn.mPlayer.mUserName);
                    //        if (conn.mPlayer.mbHasGameObject)
                    //        {
                    //            //Access to Player :D
                    //            Players.addPlayer(conn.mPlayer);
                    //        }
                    //    }

                    //}

                    if (Settings.Instance.settings.statsEnabled == 1)
                    {
                        WriteLine("Updating Stats");
                        GameStats.GenerateHTML();
                    }
                }
                catch (Exception e)
                {
                    ServerConsole.DoServerString(e.Message);
                }
            }
        }

        public void Start()
        {
            WriteLine("Loading...");
            foreach (ModConfiguration current in ModManager.mModConfigurations.Mods)
            {
                if (current.Id.Contains(UtilClass.modId) || current.Name == UtilClass.modName) 
                {
                    workingDir = current.Path;
                    Settings.Initialise();
                    break;
                }
            }
            WriteLine("Loaded!");
        }
    }
}
