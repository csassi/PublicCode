///////////////////////////////////////////////////////////
//@file: Customer.cs
//@author: Christian Sassi
//@purpose: Object to hold customer accounts.
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
    public class Customer
    {
        ConcurrentBag<Account> itsAccounts;
        string itsID = "";

        public Customer(string theID)
        {
            itsAccounts = new ConcurrentBag<Account>();
            itsID = theID;
        }

        public string ID
        {
            get
            {
                return itsID;
            }
        }

        public ConcurrentBag<Account> Accounts
        {
            get
            {
                return itsAccounts;
            }
        }

        public double TotalAccountBalance
        {
            get
            {
                double balance = 0.0;
                foreach (Account account in itsAccounts)
                {
                    balance += account.AccountBalance;
                }
                return balance;
            }
        }

        public void addNewAccount(string theAccountName, double theInitalAccount)
        {
            Account newAccount = new Account(theAccountName);
            newAccount.AccountBalance = theInitalAccount;
            itsAccounts.Add(newAccount);
        }

        public Account getAccount(string theAccount)
        {
            foreach (Account account in itsAccounts)
            {
                if (account.AccountName == theAccount)
                {
                    return account;
                }
            }
            return null;
        }
    }
}
