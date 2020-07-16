using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class ClientModel:AClient
    {
        private string firstName;
        public string FirstName { get => firstName; set { firstName = value; OnPropertyChanged(nameof(FirstName)); } }


        private string secondName;
        public string SecondName { get => secondName; set { secondName = value; OnPropertyChanged(nameof(SecondName)); } }


        private bool isVip;
        public bool IsVip { get => isVip; set { isVip = value; OnPropertyChanged(nameof(IsVip));} }

        public override string Info { get => $"First Name: {FirstName}\n" +
              $"Second Name: {SecondName}\n" +
              $"VIP: {IsVip}\n" +
              $"Account: {Account}\n" +
              $"Ammount: {Amount}\n"+
              $"Credits: {(Credits == null ? 0 : Credits.Count)}\n" +
              $"Deposits: {(Deposits == null? 0 : Deposits.Count)}";
        }
        public ClientModel() { }
        public ClientModel(string fName, string sName, bool vip, long Account, decimal Amount, CreditHistory creditHistory)
            :base(Account, Amount, creditHistory)
        {
            FirstName = fName;
            SecondName = sName;
            IsVip = vip;
        }

        public void ChangeVip(bool newVip)
        {
            IsVip = newVip;
        }

        public void EditData(string fName, string sName, bool vip)
        {
            FirstName = fName;
            SecondName = sName;
            IsVip = vip;
        }

        public override string ToString()
        {
            return $"{FirstName} {SecondName}";
        }
    }
}
