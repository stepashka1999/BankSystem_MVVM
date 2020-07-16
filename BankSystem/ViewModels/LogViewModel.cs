using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.ViewModels
{
    class LogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BankDbContext context;

        private LogModel selectedLog;
        public LogModel SelectedLog { get => selectedLog; set { selectedLog = value; OnPropertyChanged(nameof(SelectedLog)); } }

        public BindingList<LogModel> Logs { get; set; }


        public LogViewModel(string connection)
        {
            context = new BankDbContext(connection);

            context.Logs.Load();
            Logs = context.Logs.Local.ToBindingList();

            AClient.CreditClosed += AClient_CreditClosed;
            AClient.DepositClosed += AClient_DepositClosed;
            CreditModel.MakedPayment += CreditModel_MakedPayment;
            DepositModel.MakedPayment += DepositModel_MakedPayment;
            AClient.MoneySended += AClient_MoneySended;
            AClient.NotEnoughtMoney += AClient_NotEnoughtMoney;
        }

        private void DepositModel_MakedPayment(AClient arg1, DepositModel arg2)
        {
            var log = new LogModel() { Message = $"Выплата по депозиту ( {arg1} )" };
            context.Logs.Add(log);
            context.SaveChanges();
        }

        private void CreditModel_MakedPayment(AClient arg1, CreditModel arg2)
        {
            var log = new LogModel() { Message = $"Выплата по кредиту ( {arg1} )" };
            context.Logs.Add(log);
            context.SaveChanges();
        }

        private void AClient_NotEnoughtMoney(AClient obj)
        {
            var log = new LogModel() { Message = $"Клиенту {obj} недостаточно средств, для проведения операции" };
            context.Logs.Add(log);
            context.SaveChanges();
        }

        private void AClient_MoneySended(AClient arg1, AClient arg2)
        {
            var log = new LogModel() { Message = $"{arg1} перевел деньги {arg2}" };
            context.Logs.Add(log);
            context.SaveChanges();
        }
 
        private void AClient_DepositClosed(AClient arg1, DepositModel arg2)
        {
            var log = new LogModel() { Message = $"{arg1}, ваш депозит закрыт" };
            context.Logs.Add(log);
            context.SaveChanges();
        }

        private void AClient_CreditClosed(AClient arg1, CreditModel arg2)
        {
            var log = new LogModel() { Message = $"{arg1}, ваш кредит закрыт" };
            context.Logs.Add(log);
            context.SaveChanges();
        }

        private protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
