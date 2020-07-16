using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem
{
    class NotEnoughtMoneyException: Exception
    {
        public NotEnoughtMoneyException() { }
        public NotEnoughtMoneyException(AClient holder)
            : base($"У клиента {holder} не хватает средств, для проведения операции.")
        { }

    }
}
