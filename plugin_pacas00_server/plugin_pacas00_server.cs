using plugin_pacas00_server.commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace plugin_pacas00_server
{
	public class plugin_pacas00_server: FortressCraftMod
	{

		//ServerConsole - real console
		//GameManager - Game Stats

		public plugin_pacas00_server()
		{

		}

		public static int LFU_Updates_Per_Second = 5; // assuming that the LFU updates happen 5 times a second.
		public static int triggerCounter = 0;

		public static int triggerCounterMax = LFU_Updates_Per_Second * 15; //debug; fast update every 15 seconds


		bool networkSetupComplete = false;
		bool worldSetupComplete = false;
		public static string workingDir;
		public static HTTPServ HTTPServer = null;

		public override void LowFrequencyUpdate()
		{
			if(worldSetupComplete == false)
				if(WorldScript.instance != null)
				{
					if(WorldScript.instance.mWorldData != null)
					{
						worldSetupComplete = true;
						Settings.SaveSettings();
					}
				}

			if(networkSetupComplete == false)
				if(NetworkManager.instance != null)
				{
					if(NetworkManager.instance.mServerThread != null)
					{
						networkSetupComplete = true;
						//Network Side
						Settings.ApplyServerSettings();

						if(Settings.Instance.settings.HTTPServerEnabled == 1)
						{
							Directory.CreateDirectory(workingDir + Path.DirectorySeparatorChar + "webroot");
							HTTPServer = new HTTPServ(workingDir + Path.DirectorySeparatorChar + "webroot", Settings.Instance.settings.HTTPServerPort);
							HTTPServer.Start();
						}
					}
				}


			if(triggerCounter < triggerCounterMax)
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

					if(Settings.Instance.settings.statsEnabled == 1)
					{
						if(Settings.Instance.settings.statsMode == 0 || Settings.Instance.settings.statsMode == 2)
						{
							//Stats
							StatsHTML.GenerateHTML("Template.html", Settings.Instance.settings.StatsSaveFileName);
						}
						if(Settings.Instance.settings.statsMode == 1 || Settings.Instance.settings.statsMode == 2)
						{
							//Banner
							StatsHTML.GenerateHTML("TemplateBanner.html", Settings.Instance.settings.BannerSaveFileName);
						}
					}
				}
				catch(Exception e)
				{
					ServerConsole.DoServerString(e.Message);
				}
			}
		}

		public void Start()
		{
			UtilClass.WriteLine("Loading...");
			foreach(ModConfiguration current in ModManager.mModConfigurations.Mods)
			{
				if(current.Id.Contains(UtilClass.modId) || current.Name == UtilClass.modName)
				{
					workingDir = current.Path;
					Settings.Initialise();
					break;
				}
			}
			UtilClass.WriteLine("Loaded!");
		}


		public new void StopAllCoroutines()
		{
			if(HTTPServer != null)
			{
				HTTPServer.Stop();
			}

			try
			{
				((MonoBehaviour)this).StopAllCoroutines();
			}
			catch { }
		}

	}
}
