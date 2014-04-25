///////////////////////////////////////////////////////////
//@file: Transaction.cs
//@author: Christian Sassi
//@purpose: Object to hold a single transaction in account.
//@Generated: Nov 11, 2013
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSoftware.Bank
{
    public class Transaction
    {
        double itsBalanceAtTransaction;
        double itsPreviousBalance;
        string itsTransaction;
        DateTime itsTimeOfTransaction;
        Account itsAccount;
        int itsTransactionID;

        public Transaction(string theTransaction, Account itsAccount)
        {
            itsTransaction = theTransaction;
            itsTransactionID = 0;
        }

        public string TheTransaction
        {
            set
            {
                itsTransaction = value;
            }
            get
            {
                return itsTransaction;
            }
        }

        public DateTime TimeOfTransaction
        {
            set
            {
                itsTimeOfTransaction = value;
            }
            get
            {
                return itsTimeOfTransaction;
            }
        }

        public double BalanceAtTransaction
        {
            set
            {
                itsBalanceAtTransaction = value;
            }
            get
            {
                return itsBalanceAtTransaction;
            }
        }
        
        public double PreviousBalance
        {
            set
            {
                itsPreviousBalance = value;
            }
            get
            {
                return itsPreviousBalance;
            }
        }
        public Account Account
        {
            set
            {
                itsAccount = value;
            }
            get
            {
                return itsAccount;
            }
        }
    }
}
