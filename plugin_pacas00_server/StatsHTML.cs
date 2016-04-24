using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace plugin_pacas00_server
{
    class StatsHTML
    {
        public const string TemplateURL = "https://cdn.rawgit.com/pacas00/Pacas00-s-Dedicated-Server-Utils/master/plugin_pacas00_server/Stats/Template.html";


        private static string prettyfloat(float count, string toStringParams = "F0")
        {
            string s = "";
            if (count < 1000f)
            {
                s = count.ToString(toStringParams);
                return s;
            }
            else
            {
                count /= 1000f;
                if (count < 1000f)
                {
                    s = count.ToString(toStringParams) + "k";
                    return s;
                }
                else
                {
                    count /= 1000f;
                    s = count.ToString(toStringParams) + "m";
                    return s;
                }
            }
        }

        //        // 
        //$ServerName
        //$WorldName
        //$PlayerCount

        //Server Uptime
        //$Uptime

        //Total World Playtime
        //$PlayTime

        //Power Generated curr/Min
        //$PowerPerMinCurr

        //Power Generated last/Min
        //$PowerPerMinLast

        //Total Pyro Power
        //$TotalPowerPyro

        //Total Solar Power
        //$TotalPowerSolar

        //Total Jet Power
        //$TotalPowerJet

        //Total Power Generated
        //$TotalPower

        //Coal Burned
        //$ResCoalBurned

        //Bars This/Min
        //$ResBarsMinCurr

        //Bars Last/Min
        //$ResBarsMinLast

        //Total Ore
        //$ResTotalOre

        //Total Bars
        //$ResTotalBars

        //$AttackState(Under Attack! or blank)
        //$Threat(%)

        //Total Waves Seen
        //$Waves

        //Total Waves Losses
        //$Losses

        //Total Kills
        //$Kills
        //        //



        public static void GenerateHTML()
        {
            string template = "";

            try
            {
                template = File.ReadAllText(plugin_pacas00_server.workingDir + Path.DirectorySeparatorChar + "Template.html");
            }
            catch (IOException ioex)
            {
                UtilClass.WriteLine("Template not found, downloading...");
                
                using (var client = new WebClient())
                {
                    client.DownloadFile(TemplateURL, plugin_pacas00_server.workingDir + Path.DirectorySeparatorChar + "Template.html");
                }
                template = File.ReadAllText(plugin_pacas00_server.workingDir + Path.DirectorySeparatorChar + "Template.html");
            }
            catch (Exception ex)
            {
                //Log others
                UtilClass.WriteLine(ex);
            }

            string AttackState = "";
            if (MobSpawnManager.mbAttackUnderway)
            {
                AttackState = "Under Attack!";
            }

            int seconds = (int)GameManager.mrTotalServerTime;
            int totalSeconds = (int)WorldScript.instance.mWorldData.mrWorldTimePlayed;

            string serverUptime = string.Format("{0} Days, {1} Hr, {2} Min, {3} Sec", seconds / (3600 * 24), (seconds / 3600) % 24, (seconds / 60) % 60, seconds % 60);
            string worldPlayTime = string.Format("{0} Days, {1} Hr, {2} Min, {3} Sec", totalSeconds / (3600 * 24), (totalSeconds / 3600) % 24, (totalSeconds / 60) % 60, totalSeconds % 60);


            string newPage = template .Replace("$ServerName", NetworkManager.instance.mServerThread.mServerName)

                .Replace("$WorldName", WorldScript.instance.mWorldData.mName)
                .Replace("$PlayerCount", GameManager.mnCurrentTotalPlayers.ToString())

                .Replace("$Uptime", serverUptime)
                .Replace("$PlayTime", worldPlayTime)

                .Replace("$PowerPerMinCurr", prettyfloat(GameManager.mrPowerThisMin, "F2"))
                .Replace("$PowerPerMinLast", prettyfloat(GameManager.mrPowerLastMin, "F2"))
                .Replace("$TotalPowerPyro", prettyfloat(GameManager.mrTotalPyroPower, "F2"))
                .Replace("$TotalPowerSolar", prettyfloat(GameManager.mrTotalSolarPower, "F2"))
                .Replace("$TotalPowerJet", prettyfloat(GameManager.mrTotalJetPower, "F2"))
                .Replace("$TotalPower", prettyfloat(GameManager.mrTotalPowerGenerated, "F2"))


                .Replace("$CoalBurned", prettyfloat(GameManager.mnCoalBurned))
                .Replace("$BarsMinCurr", prettyfloat(GameManager.mnBarsThisMin))
                .Replace("$BarsMinLast", prettyfloat(GameManager.mnBarsLastMin))
                .Replace("$TotalOre", prettyfloat(GameManager.mnTotalOre))
                .Replace("$TotalBars", prettyfloat(GameManager.mnTotalBars))


                .Replace("$AttackState", AttackState)                
                .Replace("$Threat", ((int)(MobSpawnManager.mrSmoothedBaseThreat * 100)).ToString())

                .Replace("$Waves", ((int)MobSpawnManager.TotalWavesSeen).ToString())
                .Replace("$Losses", ((int)MobSpawnManager.TotalWavesLosses).ToString())
                .Replace("$Kills", ((int)MobSpawnManager.TotalKills).ToString())  ;

            

            string path = Settings.Instance.settings.StatsSavePath.Replace("$ModFolder$", plugin_pacas00_server.workingDir + Path.DirectorySeparatorChar);
            string fileName = Settings.Instance.settings.StatsSaveFileName;

            if (path.LastIndexOf("\\") != (path.Length - 1))
            {
                path = path + Path.DirectorySeparatorChar;
            }

            using (TextWriter writer = File.CreateText(path + fileName))
                try
                {
                    {
                        writer.Write(newPage);
                    }
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    UtilClass.WriteLine(ex.Message);
                    writer.Flush();
                    writer.Close();
                }
        }
    }
}
