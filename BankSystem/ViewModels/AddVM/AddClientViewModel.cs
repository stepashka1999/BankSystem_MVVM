using BankSystem.Models;
using BankSystem.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BankSystem.ViewModels
{
    class AddClientViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool IsActive => VerifyData();

        AddClientWindowView window;
        public BindingList<CreditHistory> CreditHistories { get; set; }
        public ClientModel Client { get; set; }


        private CreditHistory selectedItem;
        public CreditHistory SelectedItem { get => selectedItem; set { selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); } }


        private string firstName;
        public string FirstName { get => firstName; set { firstName = value; OnPropertyChanged(nameof(FirstName)); } }


        private string secondName;
        public string SecondName { get => secondName; set { secondName = value; OnPropertyChanged(nameof(SecondName)); } }


        private bool isVip;
        public bool IsVip { get => isVip; set { isVip = value; OnPropertyChanged(nameof(IsVip)); } }


        private string account;
        public string Account { get => account; set { account = value; OnPropertyChanged(nameof(Account)); } }


        private string amount;
        public string Amount { get => amount; set { amount = value; OnPropertyChanged(nameof(Amount)); } }

        public AddClientViewModel() { } //Temp shit

        public AddClientViewModel(BankDbContext context, AddClientWindowView window)
        {
            this.window = window;
            Client = window.Client;

            if (IsFilled()) FillFields();

            context.CreditHistories.Load();
            CreditHistories = context.CreditHistories.Local.ToBindingList();
        }

        private void FillFields()
        {
            FirstName = Client.FirstName;
            SecondName = Client.SecondName;
            IsVip = Client.IsVip;
            Account = Client.Account.ToString();
            Amount = Client.Amount.ToString();
            SelectedItem = Client.CreditHistory;
        }

        private bool IsFilled()
        {
            if (string.IsNullOrEmpty(Client.FirstName)) return false;

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
            if (string.IsNullOrEmpty(FirstName)) return false;
            if (string.IsNullOrEmpty(SecondName)) return false;
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

        private void FillClient()
        {
            Client.FirstName = FirstName;
            Client.SecondName = SecondName;
            Client.IsVip = IsVip;
            Client.Account = AccountParse(Account);
            Client.Amount = decimal.Parse(Amount);
            Client.CreditHistory = SelectedItem;
        }

        public ICommand Add
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    FillClient();

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
