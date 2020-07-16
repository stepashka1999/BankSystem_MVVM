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
    class AddCreditViewModel
    {
        private AClient holder;
        private AddCreditWindowView window;
        private CreditModel credit;

        public string Amount { get; set; }
        public string Month { get; set; }

        public AddCreditViewModel(AClient holder, AddCreditWindowView window, CreditModel credit)
        {
            this.holder = holder;
            this.window = window;
            this.credit = credit;
        }


        public ICommand AddCreditCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    credit.HolderId = holder.Id;
                    credit.Amount = decimal.Parse(Amount);
                    credit.Month = int.Parse(Month);
                    credit.Percent = holder.CreditHistory.Percent;

                    holder.Amount += credit.Amount;

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
