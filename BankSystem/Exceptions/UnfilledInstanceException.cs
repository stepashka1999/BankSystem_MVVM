using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Exceptions
{
    class UnfilledInstanceException:Exception
    {
        public UnfilledInstanceException() { }
        public UnfilledInstanceException(string objectName)
            :base($"Поля {objectName} заполнены.")
        { }
    }
}
