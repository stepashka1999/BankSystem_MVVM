using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    static class ExtensionMethods
    {
        public static void Add(this CreditModel credit, AClient client)
        {
            client.AddCredit(credit);
        }

        public static void Add(this DepositModel deposit, AClient client)
        {
            client.AddDeposit(deposit);
        }
    }
}
