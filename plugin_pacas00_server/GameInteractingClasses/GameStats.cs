using PeterCashelNet.Owin.WebpageGeneration;
using PeterCashelNet.Owin.WebpageGeneration.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plugin_pacas00_server.GameInteractingClasses
{
    class GameStats
    {
        //Imports GameManager for static properties. Checks instance to make sure the its ready.
        //Imports MobSpawnManager for threat information


        public static bool setupComplete = false;

        public static void Setup()
        {
            if (GameManager.instance != null)
            {
                setupComplete = true;
                ServerConsole.DoServerString("GameManager is ready - Pacas00.Server");
            }
            else
            {
                ServerConsole.DoServerString("Null GameManager - Pacas00.Server");
            }
        }


        private static string prettyfloat(float count)
        {
            string s = "";
            if (count < 1000f)
            {
                s = count.ToString("F2");
                return s;
            }
            else
            {
                count /= 1000f;
                if (count < 1000f)
                {
                    s = count.ToString("F2") + "k";
                    return s;
                }
                else
                {
                    count /= 1000f;
                    s = count.ToString("F2") + "m";
                    return s;
                }
            }
        }



        public static void GenerateHTML()
        {
            PageHeader h = new PageHeader()
                .title("Server Stats")
                .meta(new PeterCashelNet.Owin.WebpageGeneration.Options.meta_Options().http_equiv__refresh("120"))
                .link("stylesheet", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css", CrossoriginEnum.anonymous, "sha384-1q8mTJOASx8j1Au+a5WDVnPi2lkFfwwEAa8hDDdjZlpLegxhjVME1fgjWPGmkzs7")
                .link("stylesheet", "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap-theme.min.css", CrossoriginEnum.anonymous, "sha384-fLW2N01lMqjakBkx3l/M9EahuwpSfeNvV63J5ezn3uZzapT0u7EYsXMjQV+0En5r")
                .link("stylesheet", "https://cdn.rawgit.com/pacas00/Pacas00-s-Dedicated-Server-Utils/master/plugin_pacas00_server/Stats/css/style.css")
                .script("https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js", CrossoriginEnum.anonymous, "sha384-0mSbJDEHialfmuBBQP6A4Qrprq5OVfW37PRR3j5ELqxss1yVqOtnepnHVP9aJ7xS")
                .script("https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js")
                ;




            List<PagePart.tablerow> table_Power = new List<PagePart.tablerow>();
            table_Power.Add(new PagePart.tablerow() // NOT CORRECT!
                .Add("Power Generated curr/Min")
                .Add(prettyfloat(GameManager.mrPowerThisMin).ToString()));
            table_Power.Add(new PagePart.tablerow()
                .Add("Power Generated last/Min")
                .Add(prettyfloat(GameManager.mrPowerLastMin).ToString()));
            table_Power.Add(new PagePart.tablerow()
                .Add("")
                .Add(""));

            table_Power.Add(new PagePart.tablerow()
                .Add("Total Pyro Power")
                .Add(prettyfloat(GameManager.mrTotalPyroPower).ToString()));
            table_Power.Add(new PagePart.tablerow()
                .Add("Total Solar Power")
                .Add(prettyfloat(GameManager.mrTotalSolarPower).ToString()));
            table_Power.Add(new PagePart.tablerow()
                 .Add("Total Jet Power")
                 .Add(prettyfloat(GameManager.mrTotalJetPower).ToString()));
            table_Power.Add(new PagePart.tablerow()
                 .Add("Total Power Generated")
                 .Add(prettyfloat(GameManager.mrTotalPowerGenerated).ToString()));

            PagePart div_Power = new PagePart()
                .h("Power", 3)
                .table(table_Power)
                .br();





            List<PagePart.tablerow> table_Resources = new List<PagePart.tablerow>();
            table_Resources.Add(new PagePart.tablerow()
                .Add("Coal Burned")
                .Add(prettyfloat(GameManager.mnCoalBurned).ToString()));
            table_Resources.Add(new PagePart.tablerow()
                .Add("Bars This/Min")
                .Add(prettyfloat(GameManager.mnBarsThisMin).ToString()));
            table_Resources.Add(new PagePart.tablerow()
                .Add("Bars Last/Min")
                .Add(prettyfloat(GameManager.mnBarsLastMin).ToString()));
            table_Resources.Add(new PagePart.tablerow()
                .Add("Total Ore")
                .Add(prettyfloat(GameManager.mnTotalOre)));
            table_Resources.Add(new PagePart.tablerow()
                .Add("Total Bars")
                .Add(prettyfloat(GameManager.mnTotalBars)));




            PagePart div_Resources = new PagePart()
                .h("Resources ", 3)
                .table(table_Resources)
                .br();





            List<PagePart.tablerow> table_Threat = new List<PagePart.tablerow>();
            table_Threat.Add(new PagePart.tablerow()
                .Add("Total Waves Seen")
                .Add(((int)MobSpawnManager.TotalWavesSeen).ToString()));
            table_Threat.Add(new PagePart.tablerow()
                .Add("Total Waves Losses")
                .Add(((int)MobSpawnManager.TotalWavesLosses).ToString()));
            table_Threat.Add(new PagePart.tablerow()
                .Add("Total Kills")
                .Add(((int)MobSpawnManager.TotalKills).ToString()));
                        
            PagePart div_Threat = new PagePart()
                .h("Threat", 3);

            int threat = (int) (MobSpawnManager.mrSmoothedBaseThreat * 100);

            if (MobSpawnManager.mbAttackUnderway)
            {
                div_Threat = div_Threat
                .p("UNDER ATTACK!!!")
                .p("Threat " + threat + "%")
                .br();
            } else
            {
                div_Threat = div_Threat
                .p("Threat " + threat + "%")
                .br();
            }
                


                div_Threat = div_Threat.table(table_Threat)
                .br();



            int seconds = (int) GameManager.mrTotalServerTime;
            int totalSeconds = (int)WorldScript.instance.mWorldData.mrWorldTimePlayed;

            PagePart div_summary = new PagePart()
                .h(NetworkManager.instance.mServerThread.mServerName, 2)
                .h(WorldScript.instance.mWorldData.mName, 4)
                .p(GameManager.mnCurrentTotalPlayers.ToString() + " Players Online")
                .p("Server Uptime")
                .p(string.Format("{0} Days, {1} Hr, {2} Min, {3} Sec", seconds / (3600 * 24), seconds / 3600, (seconds / 60) % 60, seconds % 60))
                .p("Total World Playtime")
                .p(string.Format("{0} Days, {1} Hr, {2} Min, {3} Sec", totalSeconds / (3600 * 24), totalSeconds / 3600, (totalSeconds / 60) % 60, totalSeconds % 60))
                .br();


            PagePart bodydiv = new PagePart()
                .img("https://cdn.rawgit.com/pacas00/Pacas00-s-Dedicated-Server-Utils/master/plugin_pacas00_server/Stats/images/logo.png", "",150,380)
                .div(div_summary, "divSummary")
                .div(div_Power, "divPower")
                .div(div_Resources, "divResources");

            if (true)
            {
                bodydiv = bodydiv.div(div_Threat, "divThreat");
            }

            PagePart body = new PagePart().div(bodydiv, "well-sm");

            WebPage page = new WebPage(h, body);

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
                        writer.Write(page.ToString());
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
