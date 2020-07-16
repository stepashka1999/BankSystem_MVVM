using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class OrganisationModel: AClient
    {
        public event Action<OrganisationModel> DataChanged;


        private string name;
        public string Name { get => name; set { name = value; OnPropertyChanged(nameof(Name)); OnPropertyChanged(nameof(Info)); } }


        public OrganisationModel() { }
        public OrganisationModel(string Name, long Account, decimal Amount, CreditHistory creditHistory)
            : base(Account, Amount, creditHistory)
        {
            this.Name = Name;
        }

        public override string Info { get => $"Name: {Name}\n" +
                                    $"Account: {Account}\n" +
                                    $"Ammount: {Amount}\n" +
                                    $"Credits: {(Credits == null ? 0 : Credits.Count)}\n" +
                                    $"Deposits: {(Deposits == null ? 0 : Deposits.Count)}";
        }
        public void Edit(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
