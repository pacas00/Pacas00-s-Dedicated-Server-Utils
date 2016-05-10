using plugin_pacas00_server.commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace plugin_pacas00_server
{
	class Settings
	{
		private static Settings instance = null;

		public static Settings Instance
		{
			get
			{
				if(instance == null)
				{
					instance = new Settings();
					instance.Load();
				}

				if(!instance.loaded) instance.Load();


				return instance;
			}
		}

		public static void SaveSettings() { Instance.Save(); }

		public static void ApplyServerSettings()
		{
			if(NetworkManager.instance == null || NetworkManager.instance.mServerThread == null) return;
			NetworkManager.instance.mServerThread.mServerName = instance.settings.ServerName;
			NetworkManager.instance.mServerThread.mnMaxPlayerCount = instance.settings.MaxPlayerCount;
		}

		//Static Above

		//Instance Below

		public bool loaded = false;
		public SettingsObject settings = new SettingsObject();
		public static string settingsFileName = "Settings.ini";

		public void Load()
		{
			if(File.Exists(plugin_pacas00_server.workingDir + Path.DirectorySeparatorChar + settingsFileName))
				try
				{
					using(TextReader reader = File.OpenText(plugin_pacas00_server.workingDir + Path.DirectorySeparatorChar + settingsFileName))
					{
						bool hasLine = true;
						try
						{
							while(hasLine)
							{
								string line = reader.ReadLine();
								if(line == null)
								{
									hasLine = false;
									break;
								}
								parseSettingsLine(line);

							}
							loaded = true;
						}
						catch(Exception ex)
						{
							UtilClass.WriteLine("Settings: " + ex.Message);
							hasLine = false;
							reader.Close();
						}
						reader.Close();
					}
				}
				catch(Exception ex)
				{
					UtilClass.WriteLine(String.Format("Load(): {0}", ex.ToString()));
				}
			UtilClass.WriteLine("Settings Loaded!");
		}

		public void parseSettingsLine(string line)
		{
			if(line.Trim().StartsWith("#")) return;
			if(line.Trim().Length < 3) return;
			string[] parts = line.Split('=');

			switch(parts[0])
			{
				case ("MaxPlayerCount"):
					settings.MaxPlayerCount = Convert.ToInt32(parts[1]);
					break;

				case ("ServerName"):
					settings.ServerName = (parts[1]);
					break;

				case ("statsEnabled"):
					settings.statsEnabled = Convert.ToInt32(parts[1]);
					break;

				case ("statsMode"):
					settings.statsMode = Convert.ToInt32(parts[1]);
					break;

				case ("StatsSavePath"):
					settings.StatsSavePath = (parts[1]);
					break;

				case ("StatsSaveFileName"):
					settings.StatsSaveFileName = (parts[1]);
					break;

				case ("HTTPServerEnabled"):
					settings.HTTPServerEnabled = Convert.ToInt32(parts[1]);
					break;

				case ("HTTPServerPort"):
					settings.HTTPServerPort = Convert.ToInt32(parts[1]);
					break;

				default: break;
			}
		}

		internal static void Initialise()
		{
			if(instance == null)
			{
				instance = new Settings();
				instance.Load();
			}
		}

		public void Save()
		{
			if(loaded == false)
			{
				instance.settings.ServerName = NetworkManager.instance.mServerThread.mServerName;
				instance.settings.MaxPlayerCount = NetworkManager.instance.mServerThread.mnMaxPlayerCount;

				loaded = true;
			}

			File.Delete(plugin_pacas00_server.workingDir + Path.DirectorySeparatorChar + settingsFileName);

			using(TextWriter writer = File.CreateText(plugin_pacas00_server.workingDir + Path.DirectorySeparatorChar + settingsFileName))
				try
				{
					{
						writer.WriteLine("");
						writer.WriteLine("# -------------------");
						writer.WriteLine("# - Server Settings -");
						writer.WriteLine("# -------------------");
						writer.WriteLine("");
						writer.WriteLine("#ServerName - Server Name to show in Server Browser");
						writer.WriteLine("ServerName" + "=" + settings.ServerName);
						writer.WriteLine("");
						writer.WriteLine("#MaxPlayerCount - Defaults to 64 ");
						writer.WriteLine("MaxPlayerCount" + "=" + settings.MaxPlayerCount);
						writer.WriteLine("");
						writer.WriteLine("#Stats - 0 is off, 1 is on. Generate a stats html page. ");
						writer.WriteLine("# For use with an external webserver or the Built-in HTTP miniserver.");
						writer.WriteLine("statsEnabled" + "=" + settings.statsEnabled);
						writer.WriteLine("");
						writer.WriteLine("#statsMode  0 stats, 1 banner, 2 both");
						writer.WriteLine("# Generate Stats page, Banner Page or both");
						writer.WriteLine("statsMode" + "=" + settings.statsMode);
						writer.WriteLine("");
						writer.WriteLine("#StatsSavePath - Path to save the stats page in. Also used by the Built-in HTTP miniserver.");
						writer.WriteLine("StatsSavePath" + "=" + settings.StatsSavePath);
						writer.WriteLine("");
						writer.WriteLine("#StatsSaveFileName - file name to save the stats as ");
						writer.WriteLine("StatsSaveFileName" + "=" + settings.StatsSaveFileName);
						writer.WriteLine("");
						writer.WriteLine("#BannerSaveFileName - file name to save the banner as. Uses StatSavePath. ");
						writer.WriteLine("BannerSaveFileName" + "=" + settings.BannerSaveFileName);
						writer.WriteLine("");
						writer.WriteLine("");
						writer.WriteLine("");
						writer.WriteLine("#HTTPServerEnabled - 0 is off, 1 is on.");
						writer.WriteLine("# Enable or Disable the Built-in HTTP miniserver to host the stats page.");
						writer.WriteLine("# Hosts files from the StatsSavePath.");
						writer.WriteLine("HTTPServerEnabled" + "=" + settings.HTTPServerEnabled);
						writer.WriteLine("");
						writer.WriteLine("#HTTPServerPort - Port to host miniserver on. Defaults to 8081 ");
						writer.WriteLine("HTTPServerPort" + "=" + settings.HTTPServerPort);
						writer.WriteLine("");
					}
					writer.Flush();
					writer.Close();
				}
				catch(Exception ex)
				{
					UtilClass.WriteLine(String.Format("Save(): {0}", ex.ToString()));
					writer.Flush();
					writer.Close();
				}
			UtilClass.WriteLine("Settings Saved!");
		}
	}

	public class SettingsObject
	{
		public string ServerName { get; set; }
		public int MaxPlayerCount { get; set; }
		public int statsEnabled = 0;
		public int statsMode = 0; // 0 stats, 1 banner, 2 both

		public string StatsSaveFileName = "index.html";
		public string BannerSaveFileName = "banner.html";
		public string StatsSavePath = "$ModFolder$" + Path.DirectorySeparatorChar + "webroot";
		public int HTTPServerEnabled = 0;
		public int HTTPServerPort = 8081;
	}


}

