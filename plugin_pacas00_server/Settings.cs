using plugin_pacas00_server.commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static plugin_pacas00_server.UtilClass;

namespace plugin_pacas00_server
{
    class Settings
    {
        private static Settings instance = null;

        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Settings();
                    instance.Load();
                }

                if (!instance.loaded) instance.Load();


                return instance;
            }
        }

        public static void SaveSettings() { Instance.Save(); }

        public static void ApplyServerSettings()
        {
            NetworkManager.instance.mServerThread.mServerName = instance.settings.ServerName;
            NetworkManager.instance.mServerThread.mnMaxPlayerCount = instance.settings.MaxPlayerCount;
            //NetworkServerThread.SegmentSendDelay = 1f / (float)Settings.Instance.settings.netRate;
        }

        //Static Above

        //Instance Below

        public bool loaded = false;
        public SettingsObject settings = new SettingsObject();
        public static string settingsFileName = "Settings.ini";

        public void Load()
        {
            if (File.Exists(plugin_pacas00_server.workingDir + Path.DirectorySeparatorChar + settingsFileName))
                try
                {
                    using (TextReader reader = File.OpenText(plugin_pacas00_server.workingDir + Path.DirectorySeparatorChar + settingsFileName))
                    {
                        bool hasLine = true;
                        try
                        {
                            while (hasLine)
                            {
                                string line = reader.ReadLine();
                                if (line == null)
                                {
                                    hasLine = false;
                                    break;
                                }
                                parseSettingsLine(line);

                            }
                            loaded = true;
                        }
                        catch (Exception ex)
                        {
                            WriteLine("Settings: " + ex.Message);
                            hasLine = false;
                            reader.Close();
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    WriteLine(String.Format("Load(): {0}", ex.ToString()));
                }
            WriteLine("Settings Loaded!");
        }

        public void parseSettingsLine(string line)
        {
            if (line.Trim().StartsWith("#")) return;
            if (line.Trim().Length < 3) return;
            string[] parts = line.Split('=');

            switch (parts[0])
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

                case ("StatsSavePath"):
                    settings.StatsSavePath = (parts[1]);
                    break;

                case ("StatsSaveFileName"):
                    settings.StatsSaveFileName = (parts[1]);
                    break;

                default: break;
            }
        }

        internal static void Initialise()
        {
            if (instance == null)
            {
                instance = new Settings();
                instance.Load();
            }
        }

        public void Save()
        {
            if (loaded == false)
            {
                instance.settings.ServerName = NetworkManager.instance.mServerThread.mServerName;
                instance.settings.MaxPlayerCount = NetworkManager.instance.mServerThread.mnMaxPlayerCount;

                loaded = true;
            }

            File.Delete(plugin_pacas00_server.workingDir + Path.DirectorySeparatorChar + settingsFileName);

            using (TextWriter writer = File.CreateText(plugin_pacas00_server.workingDir + Path.DirectorySeparatorChar + settingsFileName))
                try
                {
                    {
                        writer.WriteLine("");
                        writer.WriteLine("# -----------------------------");
                        writer.WriteLine("# - Dedicated Server Settings -");
                        writer.WriteLine("# -----------------------------");
                        writer.WriteLine("");
                        writer.WriteLine("#ServerName - Server Name to show in Server Browser");
                        writer.WriteLine("ServerName" + "=" + settings.ServerName);
                        writer.WriteLine("");
                        writer.WriteLine("#MaxPlayerCount - Defaults to 64 ");
                        writer.WriteLine("MaxPlayerCount" + "=" + settings.MaxPlayerCount);
                        writer.WriteLine("");
                        writer.WriteLine("#Stats - 0 is off, 1 is on. WIP ");
                        writer.WriteLine("statsEnabled" + "=" + settings.statsEnabled);
                        writer.WriteLine("");
                        writer.WriteLine("#StatsSavePath - Path to save the stats page in ");
                        writer.WriteLine("StatsSavePath" + "=" + settings.StatsSavePath);
                        writer.WriteLine("");
                        writer.WriteLine("#StatsSaveFileName - file name to save the stats as ");
                        writer.WriteLine("StatsSaveFileName" + "=" + settings.StatsSaveFileName);
                        writer.WriteLine("");                                                
                    }
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    WriteLine(String.Format("Save(): {0}", ex.ToString()));
                    writer.Flush();
                    writer.Close();
                }
            WriteLine("Settings Saved!");
        }
    }

    public class SettingsObject
    {
        public string ServerName { get; set; }
        public int MaxPlayerCount { get; set; }
        public int statsEnabled { get; set; } = 0;

        public string StatsSaveFileName { get; set; } = "stats.htm";
        public string StatsSavePath { get; set; } = "$ModFolder$";
    }


}

