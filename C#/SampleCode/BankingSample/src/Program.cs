using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BankingSoftware.Utils;
using System.Threading;

namespace BankingSoftware 
{
    class Program
    {
        static void Main(string[] args)
        
        {
            TCPServer theServer = new TCPServer(5455, "MIAMI BANK");
            theServer.Start();
            theServer.RunServerLoop();
        }
    }
}
