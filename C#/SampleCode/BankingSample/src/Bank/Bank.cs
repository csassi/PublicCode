///////////////////////////////////////////////////////////
//@file: Bank.cs
//@author: Christian Sassi
//@purpose: Object to hold customer information.
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
    public class Bank
    {
        ConcurrentBag<Customer> itsCustomers;
        string itsBankName;
        public Bank(string theBankName)
        {
            BankName = theBankName;
            itsCustomers = new ConcurrentBag<Customer>();

            //Add default customers
            Customer newCustomer = new Customer("CUST1");
            newCustomer.addNewAccount("Checking", 100.00);
            newCustomer.addNewAccount("Savings", 10.00);
            itsCustomers.Add(newCustomer);
            
            Customer newCustomer1 = new Customer("CUST2");
            newCustomer1.addNewAccount("Checking", 110.00);
            newCustomer1.addNewAccount("Savings", 10.00);
            itsCustomers.Add(newCustomer1);
            
            Customer newCustomer2 = new Customer("CUST3");
            newCustomer2.addNewAccount("Checking", 20.00);
            newCustomer2.addNewAccount("Savings", 200.00);
            itsCustomers.Add(newCustomer2);

            Customer newCustomer3 = new Customer("CUST4");
            newCustomer3.addNewAccount("Checking", 0);
            newCustomer3.addNewAccount("Savings", 10.00);
            itsCustomers.Add(newCustomer3);

            Customer newCustomer4 = new Customer("CUST5");
            newCustomer4.addNewAccount("Checking", 1.00);
            newCustomer4.addNewAccount("Savings", 10.00);
            itsCustomers.Add(newCustomer4);

            Customer newCustomer5 = new Customer("CUST6");
            newCustomer5.addNewAccount("Checking", 50.00);
            newCustomer5.addNewAccount("Savings", 10.00);
            itsCustomers.Add(newCustomer5);

            Customer newCustomer6 = new Customer("CUST7");
            newCustomer6.addNewAccount("Checking", 1010.00);
            newCustomer6.addNewAccount("Savings", 10.00);
            itsCustomers.Add(newCustomer6);
        }

        public string BankName
        {
            set
            {
                itsBankName = value;
            }
            get 
            {
                return itsBankName;
            }
        }

        public ConcurrentBag<Customer> Customers
        {
            get
            {
                return itsCustomers;
            }
        }

        public void doAddCustomer(string ClientID)
        {
            if (ClientID != "")
            {
                if (getCustomer(ClientID) == null)
                {
                    Customer newCustomer = new Customer(ClientID);
                    newCustomer.addNewAccount("Checking", 100.00);
                    newCustomer.addNewAccount("Savings", 0);
                    itsCustomers.Add(newCustomer);
                }
            }
            else
            {
                Console.WriteLine("ERROR when adding Client. ClientID Cannot be empty.");
            }
        }
        public void doTransfer(string theAmount, string theAccount, string toAccount, string theClientID)
        {
            string transactionStringFromAccount = "-" + theAmount;
            string transactionStringToAccount = "+" + theAmount;
            Customer aCustomer = getCustomer(theClientID);

            if (aCustomer == null)
            {
                Console.WriteLine("ERROR when getting Customer. Customer NOT found.");
                return;
            }

            Account custAccountFromAccount = aCustomer.getAccount(theAccount);
            Account custAccountToAccount = aCustomer.getAccount(toAccount);

            if (custAccountFromAccount == null || custAccountToAccount == null)
            {
                Console.WriteLine("ERROR when getting Account. Account NOT found.");
                return;
            }

            Transaction newTransFromAccount = new Transaction(transactionStringFromAccount, custAccountFromAccount);
            Transaction newTransToAccount = new Transaction(transactionStringToAccount, custAccountToAccount);

            newTransFromAccount.TimeOfTransaction = DateTime.Now;
            newTransFromAccount.PreviousBalance = custAccountFromAccount.AccountBalance;
            newTransFromAccount.BalanceAtTransaction = custAccountFromAccount.AccountBalance - Convert.ToDouble(theAmount);
            custAccountFromAccount.AccountBalance -= Convert.ToDouble(theAmount);
            custAccountFromAccount.addTransaction(newTransFromAccount);

            newTransToAccount.TimeOfTransaction = DateTime.Now;
            newTransToAccount.PreviousBalance = custAccountToAccount.AccountBalance;
            newTransToAccount.BalanceAtTransaction = custAccountToAccount.AccountBalance + Convert.ToDouble(theAmount);
            custAccountToAccount.AccountBalance += Convert.ToDouble(theAmount);
            custAccountToAccount.addTransaction(newTransToAccount);
        }
        public void doWithdawl(string theAmount, string theAccount, string theClientID)
        {
            string transactionString = "-" + theAmount;
            Customer aCustomer = getCustomer(theClientID);
            if(aCustomer == null)
            {
                Console.WriteLine("ERROR when getting Customer. Customer NOT found.");
                return;
            }

            Account custAccount = aCustomer.getAccount(theAccount);
            if(custAccount == null)
            {
                Console.WriteLine("ERROR when getting Account. Account NOT found.");
                return;
            }

            Transaction newTrans = new Transaction(transactionString, custAccount);
            newTrans.TimeOfTransaction = DateTime.Now;
            newTrans.PreviousBalance = custAccount.AccountBalance;
            newTrans.BalanceAtTransaction = custAccount.AccountBalance - Convert.ToDouble(theAmount);
            custAccount.AccountBalance -= Convert.ToDouble(theAmount);
            custAccount.addTransaction(newTrans);
        }

        public void doDeposit(string theAmount, string theAccount, string theClientID)
        {
            string transactionString = "+" + theAmount;
            Customer aCustomer = getCustomer(theClientID);
            if (aCustomer == null)
            {
                Console.WriteLine("ERROR when getting Customer. Customer NOT found.");
                return;
            }

            Account custAccount = aCustomer.getAccount(theAccount);
            if (custAccount == null)
            {
                Console.WriteLine("ERROR when getting Account. Account NOT found.");
                return;
            }

            Transaction newTrans = new Transaction(transactionString, custAccount);
            newTrans.TimeOfTransaction = DateTime.Now;
            newTrans.PreviousBalance = custAccount.AccountBalance;
            newTrans.BalanceAtTransaction = custAccount.AccountBalance + Convert.ToDouble(theAmount);
            custAccount.AccountBalance += Convert.ToDouble(theAmount);
            custAccount.addTransaction(newTrans);
        }

        public Customer getCustomer(string theClientID)
        {
            foreach(Customer customer in itsCustomers)
            {
                if (customer.ID == theClientID)
                {
                    return customer;
                }
            }
            return null;
        }

        public void PrintAllCustomers()
        {
            Console.WriteLine("--Recognized Customers--");
            foreach(Customer customer in itsCustomers)
            {
                Console.WriteLine("Customer <" + customer.ID + ">");
            }
        }
        public void PrintAllCustomersAccounts()
        {
            Console.WriteLine("--Recognized Customers--");
            foreach (Customer customer in itsCustomers)
            {
                Console.WriteLine("Customer <" + customer.ID + ">");
                foreach (Account account in customer.Accounts)
                {
                    Console.WriteLine("\tAccount <" + account.AccountName + "> Balance: $" + account.AccountBalance);
                }
            }
        }
        public void PrintTop5CustomersAccounts()
        {
            Console.WriteLine("--Top 5 Customers--");
            List<Customer> theCustomers = itsCustomers.OrderBy(o => o.TotalAccountBalance).ToList();

            int j = 0;
            for (int i = theCustomers.Count - 1; i >= 0; --i)
            {
                if (j >= 5)
                {
                    break;
                }
                Console.WriteLine("\tAccount Holder <" + theCustomers[i].ID + "> Accumulated Balance: $" + theCustomers[i].TotalAccountBalance);
                j++;
            }
        }

        public void PrintBottom5CustomersAccounts()
        {
            Console.WriteLine("--Bottom 5 Customers--");
            List<Customer> theCustomers = itsCustomers.OrderBy(o => o.TotalAccountBalance).ToList();

            int j = 0;
            for (int i = 0; i < itsCustomers.Count; ++i)
            {
                if (j >= 5)
                {
                    break;
                }
                Console.WriteLine("\tAccount Holder <" + theCustomers[i].ID + "> Accumulated Balance: $" + theCustomers[i].TotalAccountBalance);
                j++;
            }
        }
    }
}
