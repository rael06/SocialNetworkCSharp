﻿using Newtonsoft.Json;
using Server.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    { 
        static void Main(string[] args)
        {
            new SocketManager();
        }
    }
}

