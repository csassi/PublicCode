///////////////////////////////////////////////////////////
//@file: Account.cs
//@author: Christian Sassi
//@purpose: Object to hold account information for the customer.
//@Generated: Nov 11, 2013
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace BankingSoftware.Bank
{
    public class Account
    {
        string itsAccountName = "";
        double itsAccountBalance = 0.0;
        ConcurrentBag<Transaction> itsTransactions;

        public Account(string theName)
        {
            itsAccountName = theName;
            itsTransactions = new ConcurrentBag<Transaction>();
        }

        public string AccountName
        {
            set
            {
                itsAccountName = value;
            }
            get
            {
                return itsAccountName;
            }
        }
        public double AccountBalance
        {
            set
            {
                itsAccountBalance = value;
            }
            get
            {
                return itsAccountBalance;
            }
        }

        public void addTransaction(Transaction theTransaction)
        {
            if (theTransaction != null)
            {
                itsTransactions.Add(theTransaction);
            }
            else
            {
                Console.WriteLine("ERROR adding Transaction to Account. Transaction is null");
            }
        }
    }
}
