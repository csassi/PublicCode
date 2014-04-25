///////////////////////////////////////////////////////////
//@file: TCPClientWorker.cs
//@author: Christian Sassi
//@purpose: Object to be spawned when a client connects to TCP server.
//@Generated: Nov 11, 2013
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using BankingSoftware.Bank;

namespace BankingSoftware.Utils
{
    class TCPClientWorker
    {
        Socket itsClientSocket = null;
        Thread itsThread = null;
        Stream theStream = null;
        StreamReader theStreamReader = null;
        StreamWriter theStreamWriter = null;
        Object itsLock = null;
        Bank.Bank itsBank = null;
    
        bool itsClientRunning = false;
        string itsClientID = "";

        string RegExp = "";
        string clientIDCommand = "";
        string whichAccount = "";
        string toAccount = "";
        string withdrawCommand = "";
        string depositCommand = "";
        string transferCommand = "";
        string requestAccountBalCommand = "";
        string requestTransactionCommand = "";


        public TCPClientWorker(Socket theSocket, BankingSoftware.Bank.Bank theBank)
        {
            itsLock = new Object();

            itsClientRunning = true;

            itsClientSocket = theSocket;
            itsBank = theBank;

            theStream = new NetworkStream(theSocket);
            theStreamReader = new StreamReader(theStream);
            theStreamWriter = new StreamWriter(theStream);
            theStreamWriter.AutoFlush = true;
            itsThread = new Thread(Run);
            itsThread.Start();

            //Regular expressions to get the data from the client.
            RegExp = "[<]([A-Za-z0-9]+)[>]";
            clientIDCommand = "ClientID " + RegExp;
            whichAccount = "FromAccount " + RegExp;
            toAccount = "ToAccount " + RegExp;
            withdrawCommand = "Withdraw " + RegExp + " " + clientIDCommand + " " + whichAccount;
            depositCommand = "Deposit " + RegExp + " " + clientIDCommand + " " + whichAccount;
            transferCommand = "Transfer " + RegExp + " " + clientIDCommand + " " + whichAccount + " " + toAccount;
            requestAccountBalCommand = "BalanceRequest " + clientIDCommand;
            requestTransactionCommand = "TransactionRequest " + clientIDCommand + " " + whichAccount;
        }

        void Run()
        {
            if (itsClientRunning)
            {
                Write("BankName " + "<" + itsBank.BankName + ">");
            }
            while (itsClientRunning)
            {
                try
                {
                    string theCommand = theStreamReader.ReadLine();
                    string theAccount = "";
                    string theAmount = "";
                    string toAccount = "";
                    string theClientID = "";

                    Console.WriteLine("--" + theCommand + "--");

                    //Only add a new customer if this regexp passes.
                    if (checkInitialClientID(theCommand))
                    {
                        Console.WriteLine("Client Accepted...");
                    }
                    else if (checkDeposit(theCommand, ref theAmount, ref theAccount, ref theClientID))
                    {
                        lock (itsLock)
                        {
                            itsBank.doDeposit(theAmount, theAccount, theClientID);
                        }
                    }
                    else if (checkWithdrawl(theCommand, ref theAmount, ref theAccount, ref theClientID))
                    {
                        lock (itsLock)
                        {
                            itsBank.doWithdawl(theAmount, theAccount, theClientID);
                        }
                    }
                    else if (checkTransfer(theCommand, ref theAmount, ref theAccount, ref toAccount, ref theClientID))
                    {
                        lock (itsLock)
                        {
                            itsBank.doTransfer(theAmount, theAccount, toAccount, theClientID);
                        }
                    }
                    else if (checkBalanceRequest(theCommand, ref theClientID))
                    {
                        lock (itsLock)
                        {
                            foreach (Account account in itsBank.getCustomer(theClientID).Accounts)
                            {
                                Write("BalanceRequest {Account: " + account.AccountName + " Balance: " + account.AccountBalance + "}");
                            }

                            Write("BalanceRequest {Finished}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Command NOT recognized! [" + itsClientSocket.RemoteEndPoint + "]" + theCommand);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Socket Error: " + ex.Message);
                    Disconnect();
                    itsClientRunning = false;
                }
            }
           
        }
        public void Write(string toWrite)
        {
            theStreamWriter.WriteLine(toWrite);
        }
        public void Disconnect()
        {
            theStream.Close();
            itsClientSocket.Close();
            Console.WriteLine("Client Disconnect!!!");
        }

        private bool checkInitialClientID(string theCommand)
        {
            Regex clientIDInitalize = new Regex("^" + clientIDCommand + "$", RegexOptions.IgnoreCase);
            Match match = clientIDInitalize.Match(theCommand);

            if (match.Success)
            {
                itsClientID = match.Groups[1].Value;
                lock (itsLock)
                {
                    itsBank.doAddCustomer(itsClientID);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkDeposit(string theCommand, ref string theAmount, ref string theAccount, ref string theClientID)
        {
            Regex clientDeposit = new Regex("^" + depositCommand + "$", RegexOptions.IgnoreCase);
            Match match = clientDeposit.Match(theCommand);

            if (match.Success)
            {
                theAmount = match.Groups[1].Value;
                theClientID = match.Groups[2].Value;
                theAccount = match.Groups[3].Value;

                if (theClientID != itsClientID)
                {
                    Console.WriteLine("ERROR: Client IDs DO NOT MATCH!!!!! BADNESS!!!");
                    return false;
                }

                Console.WriteLine("Client: " + theClientID + " is Depositing " + theAmount + " into account " + theAccount);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkWithdrawl(string theCommand, ref string theAmount, ref string theAccount, ref string theClientID)
        {
            Regex clientDeposit = new Regex("^" + withdrawCommand + "$", RegexOptions.IgnoreCase);
            Match match = clientDeposit.Match(theCommand);

            if (match.Success)
            {
                theAmount = match.Groups[1].Value;
                theClientID = match.Groups[2].Value;
                theAccount = match.Groups[3].Value;

                if (theClientID != itsClientID)
                {
                    Console.WriteLine("ERROR: Client IDs DO NOT MATCH!!!!! BADNESS!!!");
                    return false;
                }

                Console.WriteLine("Client: " + theClientID + " is Withdrawing " + theAmount + " from account " + theAccount);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkTransfer(string theCommand, ref string theAmount, ref string theAccount, ref string toAccount, ref string theClientID)
        {
            Regex clientDeposit = new Regex("^" + transferCommand + "$", RegexOptions.IgnoreCase);
            Match match = clientDeposit.Match(theCommand);

            if (match.Success)
            {
                theAmount = match.Groups[1].Value;
                theClientID = match.Groups[2].Value;
                theAccount = match.Groups[3].Value;
                toAccount = match.Groups[4].Value;

                if (theClientID != itsClientID)
                {
                    Console.WriteLine("ERROR: Client IDs DO NOT MATCH!!!!! BADNESS!!!");
                    return false;
                }

                Console.WriteLine("Client: " + theClientID + " is Transferring " + theAmount +
                    " from account " + theAccount + " to account " + toAccount);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkBalanceRequest(string theCommand, ref string theClientID)
        {
            Regex clientDeposit = new Regex("^" + requestAccountBalCommand + "$", RegexOptions.IgnoreCase);
            Match match = clientDeposit.Match(theCommand);

            if (match.Success)
            {
                theClientID = match.Groups[1].Value;

                if (theClientID != itsClientID)
                {
                    Console.WriteLine("ERROR: Client IDs DO NOT MATCH!!!!! BADNESS!!!");
                    return false;
                }

                Console.WriteLine("Client: " + theClientID + " is requesting account balances.");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
