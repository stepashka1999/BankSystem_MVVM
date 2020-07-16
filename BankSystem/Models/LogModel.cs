using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class LogModel
    {
        private int id;
        public int Id { get => id; set { id = value; } }


        private string message;
        public string Message { get => message; set { message = value; } }

        public override string ToString()
        {
            return Message;
        }
    }
}
