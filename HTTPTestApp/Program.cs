using plugin_pacas00_server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPTestApp
{
    class Program
    {
        static void Main(string[] args)
        {

            string testDir = @"C:\kexploit\static";
            HTTPServ HTTPServer = new HTTPServ(testDir, 8081);
            HTTPServer.Start();


        }
    }
}
