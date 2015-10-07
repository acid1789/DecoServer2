using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JuggleServerCore;

namespace DecoServer2
{
    class Program
    {
        static ServerBase _server;
        static void Main(string[] args)
        {
            LogThread.AlwaysPrintToConsole = true;
            _server = new ServerBase(1255, "server=127.0.0.1;uid=DecoServer;pwd=dspass;database=deco;");
            _server.TaskProcessor = new TaskProcessor(_server);

            _server.Run();

            LogThread.GetLog().Shutdown();
        }
    }
}
