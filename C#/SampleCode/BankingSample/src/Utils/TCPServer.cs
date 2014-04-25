///////////////////////////////////////////////////////////
//@file: TCPServer.cs
//@author: Christian Sassi
//@purpose: Object to be the main loop, it holds the bank object.
//@Generated: Nov 11, 2013
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using System.Text.RegularExpressions;

namespace BankingSoftware.Utils
{
    public class TCPServer
    {
        private volatile bool itsIsServerRunning = false;
        private TcpListener itsSocket;
        private int itsPort;
        Thread itsThread = null;
        ConcurrentBag<TCPClientWorker> itsClients;
        BankingSoftware.Bank.Bank itsBank;

        public TCPServer(int port, string theBankName)
        {
            itsPort = port;
            itsClients = new ConcurrentBag<TCPClientWorker>();
            itsBank = new BankingSoftware.Bank.Bank(theBankName);
        }

        public BankingSoftware.Bank.Bank TheBank
        {
            get 
            {
                return itsBank;
            }
        }

        public void Start()
        {
            Console.WriteLine("Initializing Services...");
            try
            {
                itsIsServerRunning = true;
                itsSocket = new TcpListener(IPAddress.Parse(LocalIPAddress()), itsPort);
                itsSocket.Start();      
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Server Start Failure: " + ex.Message);
                itsIsServerRunning = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Server Start Failure: " + ex.Message);
                itsIsServerRunning = true;
            }

            itsThread = new Thread(Run);
            itsThread.Start();
            Console.WriteLine("Starting Client Services for: " + TheBank.BankName);
        }
        public void RunServerLoop()
        {
            WriteServerCommands();
            string theInput = "";
            while (itsIsServerRunning)
            {
                string theLine = Console.ReadLine();
                Regex theInputRegExp = new Regex("^([1|2|3|4|5|h|q]) ?((.*) (.*) ([0-9][0-9]/[0-9][0-9]/[0-9][0-9]))?$", RegexOptions.IgnoreCase);
                Match match = theInputRegExp.Match(theLine);

                if (match.Success)
                {
                    theInput = match.Groups[1].Value;
                    switch (theInput)
                    {
                        case "1":
                            TheBank.PrintAllCustomers();
                            break;
                         case "2":
                            TheBank.PrintAllCustomersAccounts();
                            break;
                         case "3":
                            TheBank.PrintTop5CustomersAccounts();
                            break;
                         case "4":
                            TheBank.PrintBottom5CustomersAccounts();
                            break;
                         case "5":
                             Console.WriteLine("AccountID: " + match.Groups[3].Value);
                             Console.WriteLine("Account: " + match.Groups[4].Value);
                             Console.WriteLine("Date: " + match.Groups[5].Value);

                            Console.WriteLine("Unimplemented - 5");
                            break;
                         case "h":
                            WriteServerCommands();
                            break;
                         case "H":
                            WriteServerCommands();
                            break;
                         case "q":
                            itsIsServerRunning = false;
                            break;
                    }
                }
                else
                {
                    Console.Clear();


                    WriteServerCommands();

                    Console.WriteLine("\nUnrecognized Input!!!");
                }
            }
        }
        public void Run()
        {
            try
            {
                while (itsIsServerRunning)
                {
                    if (!itsSocket.Pending())
                    {
                        Thread.Sleep(1);
                        continue; 
                    }
                    Socket clientSocket = itsSocket.AcceptSocket();
                    Console.WriteLine("Client Connected : " + clientSocket.RemoteEndPoint);
                    TCPClientWorker newClient = new TCPClientWorker(clientSocket, itsBank);
                    itsClients.Add(newClient);
                    WriteServerCommands();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Socket Error: "  + ex.Message);
            }

            foreach (TCPClientWorker client in itsClients)
            {
                client.Disconnect();
            }
            
            itsSocket.Stop();
        }
        private void WriteServerCommands()
        {
            Console.WriteLine("Server Commands: Enter");
            Console.WriteLine("\t h - Print this message again.");
            Console.WriteLine("\t q - Quits application.");
            Console.WriteLine("\t 1 - Print all customers IDs.");
            Console.WriteLine("\t 2 - Print all customers IDs and accounts with balances.");
            Console.WriteLine("\t 3 - Print top 5 customers with highest combined account balance.");
            Console.WriteLine("\t 4 - Print bottom 5 customers with lowest combined account balance.");
            Console.WriteLine("\t 5 CUSTOMER_ID(.*) CUSTOMER_ACCOUNT(.*) DATE[0-9][0-9]/[0-9][0-9]/[0-9][0-9] - \n\t\tPrint customers account balance on a date.");
                    
        }
        static public string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
    }
}
