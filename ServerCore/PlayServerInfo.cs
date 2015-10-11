using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuggleServerCore
{
    public class PlayServerInfo
    {
        public string Name;
        public string IPAddress;
        public ushort Port;

        public PlayServerInfo(string name, string address, ushort port)
        {
            Name = name;
            IPAddress = address;
            Port = port;
        }
    }
}
