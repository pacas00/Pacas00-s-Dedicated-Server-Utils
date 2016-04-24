using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace plugin_pacas00_server
{
    class StatsHTML
    {
        public const string TemplateURL = "https://cdn.rawgit.com/pacas00/Pacas00-s-Dedicated-Server-Utils/master/plugin_pacas00_server/Stats/Template.html";

        static float mrPeakOreMin = 0f;
        static float mrPeakBarsMin = 0f;


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

        //Power /Sec
        //$PowerPerSec

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
            //int totalSeconds = (int)WorldScript.instance.mWorldData.mrWorldTimePlayed;
            int totalSeconds = (int) GameManager.mrTotalPowerGenerated;




            string serverUptime = string.Format("{0} Days, {1} Hr, {2} Min, {3} Sec", seconds / (3600 * 24), (seconds / 3600) % 24, (seconds / 60) % 60, seconds % 60);
            string worldPlayTime = string.Format("{0} Days, {1} Hr, {2} Min, {3} Sec", totalSeconds / (3600 * 24), (totalSeconds / 3600) % 24, (totalSeconds / 60) % 60, totalSeconds % 60);

			//The holobase has different values, in since the holobase isnt initalised on the dedicated server, i cannot use refection to read the labels,
            //so, the following is a modified copy paste to get the correct values.
            //These names refer to the label names and vars in Holobase.
            string Ores_Count_Label = "";
            string Ores_Overlay_Label = "";

            string Bars_Count_Label = "";
            string Bars_Overlay_Label = "";

            float mrPeakPowerMin = 1f;

            string Power_Sec_Label = "";
            string Total_Power_Label = "";


            float num = (float)GameManager.mnOresLastMin;
            Ores_Count_Label = GameManager.mnTotalOre.ToString();
            Ores_Overlay_Label = num.ToString("F0") + " ore/min"; //Ores Min

            float num2 = (float)GameManager.mnBarsLastMin;
            Bars_Count_Label = GameManager.mnTotalBars.ToString();
            Bars_Overlay_Label = num2.ToString("F0") + " bars/min"; //bars min

            if (GameManager.mrTotalTimeSimulated > 0f)
            {
                float num3 = GameManager.mrTotalPowerGenerated / GameManager.mrTotalTimeSimulated;
                if (num3 > mrPeakPowerMin)
                {
                    mrPeakPowerMin = num3;
                }
                Power_Sec_Label = num3.ToString("F2");
                Total_Power_Label = prettyfloat(GameManager.mrTotalPowerGenerated);
            }
            string Total_Time = GameManager.mrTotalTimeSimulated.ToString("F2");
			//
			

            string newPage = template .Replace("$ServerName", NetworkManager.instance.mServerThread.mServerName)

                .Replace("$WorldName", WorldScript.instance.mWorldData.mName)
                .Replace("$PlayerCount", GameManager.mnCurrentTotalPlayers.ToString())

                .Replace("$Uptime", serverUptime)
                .Replace("$PlayTime", worldPlayTime)

                .Replace("$PowerPerSec", Power_Sec_Label)
                .Replace("$TotalPowerPyro", prettyfloat(GameManager.mrTotalPyroPower, "F2"))
                .Replace("$TotalPowerSolar", prettyfloat(GameManager.mrTotalSolarPower, "F2"))
                .Replace("$TotalPowerJet", prettyfloat(GameManager.mrTotalJetPower, "F2"))
                .Replace("$TotalPower", Total_Power_Label)


                .Replace("$CoalBurned", prettyfloat(GameManager.mnCoalBurned))
                .Replace("$OresMin", Ores_Overlay_Label)
                .Replace("$BarsMin", Bars_Overlay_Label)
                .Replace("$TotalOre", Ores_Count_Label)
                .Replace("$TotalBars", Bars_Count_Label)


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
