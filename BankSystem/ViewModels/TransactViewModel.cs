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

    class TransactViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
       
        AClient Client;
        TransactWindowView window;

        private string error;
        public string Error { get => error; set { error = value; OnPropertyChanged(nameof(Error)); } }

        
        private AClient selectedClient;
        public AClient SelectedClient { get=> selectedClient; set { selectedClient = value; OnPropertyChanged(nameof(SelectedClient)); } }


        private int transactType;
        public int TransactType { get=> transactType; set { transactType = value; OnPropertyChanged(nameof(TransactType)); } }


        private string amount;
        public string Amount { get => amount; set { amount = value; OnPropertyChanged(nameof(Amount)); } }


        public List<AClient> Clients { get; set; } = new List<AClient>();

        public TransactViewModel() { }
        public TransactViewModel(BankDbContext context, AClient client, TransactWindowView window)
        {
            Client = client;
            this.window = window;

            context.Clients.Load();
            context.Organisations.Load();

            Clients.AddRange(context.Clients.Local.ToList());
            Clients.AddRange(context.Organisations.Local.ToList());

            Clients.Remove(Client);
        }

        private bool VerifyData()
        {
            if (SelectedClient == null)
            {
                Error = "Клиент не выбран";
                return false;
            }
            
            if (VerifayAmount() == false) return false;
            
            if (TransactType != 0 && TransactType != 1)
            {
                Error = "Не выбран тип перевода";
                return false;
            }

            Error = "";
            return true;
        }

        private bool VerifayAmount()
        {
            decimal amount;
            if (decimal.TryParse(Amount, out amount) == false)
            {
                Error = "Неверно введена сумма";
                return false;
            }

            if (amount <= 0)
            {
                Error = "Сумма отрицательная или равна нулю";
                return false;
            }

            if (TransactType == 0 && Client.Amount < amount)
            {
                Error = "Недостаточно средств";
                return false;
            }

            if (TransactType == 1 && SelectedClient.Amount < amount)
            {
                Error = "Недостаточно средств";
                return false;
            }

            return true;
        }

        private void TransactTo()
        {
            Client.SendMoneyTo(SelectedClient, decimal.Parse(Amount));
        }

        private void TransactFrom()
        {
            SelectedClient.SendMoneyTo(Client, decimal.Parse(Amount));
        }

        private void Transact()
        {
            switch(TransactType)
            {
                case 0:
                    TransactTo();
                    break;
                case 1:
                    TransactFrom();
                    break;
            }
        }


        public ICommand TransactCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    if (VerifyData())
                    {
                        Transact();
                        window.DialogResult = true;
                        window.Close();
                    }
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
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
