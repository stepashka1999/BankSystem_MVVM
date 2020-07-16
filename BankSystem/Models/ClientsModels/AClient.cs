using BankSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class AClient: INotifyPropertyChanged
    {
        public static event Action<AClient, AClient> MoneySended;
        public static event Action<AClient, CreditModel> CreditClosed;
        public static event Action<AClient, DepositModel> DepositClosed;
        public static event Action<AClient> NotEnoughtMoney;


        public event PropertyChangedEventHandler PropertyChanged;

        private int id;
        public int Id { get => id; set { id = value; OnPropertyChanged(nameof(Id)); } }


        private long account;
        public long Account { get => account; set { account = value; OnPropertyChanged(nameof(Account)); } }


        private decimal amount;
        public decimal Amount { get => amount; set { amount = value; OnPropertyChanged(nameof(Amount)); } }

        public virtual string Info => "Lul";

        private CreditHistory creditHistory;
        public CreditHistory CreditHistory { get => creditHistory; set { creditHistory = value; OnPropertyChanged(nameof(CreditHistory)); } }

        public BindingList<CreditModel> Credits { get; set; }
        public BindingList<DepositModel> Deposits { get; set; }


        public AClient() { }

        public AClient(long Account, decimal Amount, CreditHistory creditHistory)
        {
            this.Account = Account;
            this.Amount = Amount;
            CreditHistory = creditHistory;
        }


        private protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Info)));
        }

        public bool WithdrawMoney(decimal amount)
        {
            if (Amount >= amount)
            {
                NotEnoughtMoney?.Invoke(this);
                return false;
            }

            Amount -= amount;
            return true;
        }

        public void PutMoney(decimal amount)
        {
            Amount += amount;
        }


        public void AddCredit(CreditModel credit)
        {
            Credits.Add(credit);
        }

        public void AddDeposit(DepositModel deposit)
        {
            Deposits.Add(deposit);
        }

        public void MakePayment()
        {
            if (Deposits != null)
            {
                foreach (var deposit in Deposits)
                {
                    deposit.MakePayment();
                }
            }

            if (Credits != null)
            {
                foreach (var credit in Credits)
                {
                    credit.MakePayment();
                }
            }
        }


        public void CloseCredit(CreditModel credit)
        {
            Credits.Remove(credit);
            CreditClosed?.Invoke(this, credit);
        }

        public void CloseDeposit(DepositModel deposit)
        {
            Deposits.Remove(deposit);
            DepositClosed?.Invoke(this, deposit);
        }

        public bool SendMoneyTo(AClient client, decimal amount)
        {
            if (client == null || amount <= 0) return false;
            if (Amount < amount)
            {
                NotEnoughtMoney?.Invoke(this);
                return false;
            }

            Amount -= amount;
            client.Amount += amount;
            MoneySended?.Invoke(this, client);

            return true;
        }

    }
}
