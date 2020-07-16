using BankSystem.Models;
using BankSystem.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BankSystem.ViewModels
{
    class AddOrgViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool IsActive => VerifyData();
        
        AddOrganisationWindow window;
        public BindingList<CreditHistory> CreditHistories { get; set; }
        public OrganisationModel Organisation { get; set;}


        private CreditHistory selectedItem;
        public CreditHistory SelectedItem { get => selectedItem; set { selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); } }


        private string nameOrg;
        public string NameOrg { get => nameOrg; set { nameOrg = value; OnPropertyChanged(nameof(nameOrg)); } }


        private string account;
        public string Account { get => account; set { account = value; OnPropertyChanged(nameof(Account)); } }


        private string amount;
        public string Amount { get => amount; set { amount = value; OnPropertyChanged(nameof(Amount)); } }

        public AddOrgViewModel(BankDbContext context, AddOrganisationWindow window)
        {
            this.window = window;
            Organisation = window.Organisation;

            if (IsFilled()) FillFields();

            context.CreditHistories.Load();
            CreditHistories = context.CreditHistories.Local.ToBindingList();
        }


        private void FillFields()
        {
            NameOrg = Organisation.Name;
            Account = Organisation.Account.ToString();
            Amount = Organisation.Amount.ToString();
            SelectedItem = Organisation.CreditHistory;
        }

        private bool IsFilled()
        {
            if (string.IsNullOrEmpty(Organisation.Name)) return false;

            return true;
        }

        private bool VerifyAmount()
        {
            var strAmount = Amount;

            if (string.IsNullOrEmpty(strAmount)) return false;

            decimal amount;
            if (decimal.TryParse(strAmount, out amount) == false) return false;

            if (amount < 0) return false;

            return true;
        }

        private bool VerifyAccount()
        {
            if (string.IsNullOrEmpty(Account)) return false;

            long account;
            if (AccountParse(Account, out account) == false) return false;

            if (account < 0) return false;

            if (account.ToString().Length != 16) return false;

            return true;
        }
        private bool VerifyData()
        {
            if (string.IsNullOrEmpty(NameOrg)) return false;
            if (VerifyAccount() == false) return false;
            if (VerifyAmount() == false) return false;
            if (SelectedItem == null) return false;
            return true;
        }

        private bool AccountParse(string text, out long account)
        {
            var res = text.Replace(" ", "");
            return long.TryParse(res, out account);
        }

        private long AccountParse(string text)
        {
            var res = text.Replace(" ", "");

            return long.Parse(res);
        }

        private void FillOrg()
        {
            Organisation.Name = NameOrg;
            Organisation.Account = AccountParse(Account);
            Organisation.Amount = decimal.Parse(Amount);
            Organisation.CreditHistory = SelectedItem;
        }

        public ICommand Add
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    FillOrg();

                    window.DialogResult = true;
                    window.Close();
                }, obj => IsActive == true);
            }
        }

        public ICommand Close
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

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
