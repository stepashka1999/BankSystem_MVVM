using BankSystem.Models;
using BankSystem.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BankSystem.ViewModels
{
    class AddDepositViewModel
    {
        private AClient holder;
        private AddDepositMindowView window;
        private DepositModel deposit;

        public string CurrentAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public string Month { get; set; }

        public AddDepositViewModel(AClient client, DepositModel deposit, AddDepositMindowView window)
        {
            holder = client;
            this.deposit = deposit;
            this.window = window;
            MaxAmount = holder.Amount;
        }

        public ICommand AddDepositCommand
        {
            get
            {
                return new DelegateCommand(obj=>
                {
                    deposit.HolderId = holder.Id;
                    deposit.Amount = decimal.Parse(CurrentAmount);
                    deposit.Month = int.Parse(Month);
                    deposit.Percent = holder.CreditHistory.Percent;

                    holder.Amount -= deposit.Amount;

                    window.DialogResult = true;
                    window.Close();
                });
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    window.DialogResult = false;
                    window.Close();
                });
            }
        }
    }
}
