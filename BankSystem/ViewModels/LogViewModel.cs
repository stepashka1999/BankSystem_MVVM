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
    /// <summary>
    /// Класс ViewModel для работы с логами
    /// </summary>
    class LogViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменения данных поля
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Контекст БД Банка
        /// </summary>
        private BankDbContext context;


        //Выбранный лог
        private LogModel selectedLog;
        /// <summary>
        /// Выбранный лог
        /// </summary>
        public LogModel SelectedLog { get => selectedLog; set { selectedLog = value; OnPropertyChanged(nameof(SelectedLog)); } }


        /// <summary>
        /// Коллекция логов
        /// </summary>
        public BindingList<LogModel> Logs { get; set; }


        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public LogViewModel()
        {
            context = new BankDbContext();

            context.Logs.Load();
            Logs = context.Logs.Local.ToBindingList();

            AClient.CreditClosed += AClient_CreditClosed;
            AClient.DepositClosed += AClient_DepositClosed;
            CreditModel.MakedPayment += CreditModel_MakedPayment;
            DepositModel.MakedPayment += DepositModel_MakedPayment;
            AClient.MoneySended += AClient_MoneySended;
            AClient.NotEnoughtMoney += AClient_NotEnoughtMoney;
        }


        /// <summary>
        /// Обработчик события выплаты по депозитам
        /// </summary>
        /// <param name="arg1">Клиент</param>
        /// <param name="arg2">Депозит</param>
        private void DepositModel_MakedPayment(AClient arg1, DepositModel arg2)
        {
            var log = new LogModel() { Message = $"Выплата по депозиту ( {arg1} )" };
            context.Logs.Add(log);
            context.SaveChanges();
        }


        /// <summary>
        /// Обработчик события выплаты по кредитам
        /// </summary>
        /// <param name="arg1">Клиент</param>
        /// <param name="arg2">Кредит</param>
        private void CreditModel_MakedPayment(AClient arg1, CreditModel arg2)
        {
            var log = new LogModel() { Message = $"Выплата по кредиту ( {arg1} )" };
            context.Logs.Add(log);
            context.SaveChanges();
        }


        /// <summary>
        /// Обработчик события нехватки деняг
        /// </summary>
        /// <param name="obj">Клиент</param>
        private void AClient_NotEnoughtMoney(AClient obj)
        {
            var log = new LogModel() { Message = $"Клиенту {obj} недостаточно средств, для проведения операции" };
            context.Logs.Add(log);
            context.SaveChanges();
        }


        /// <summary>
        /// Обработчик события перевода денег
        /// </summary>
        /// <param name="arg1">Клиент который перевел</param>
        /// <param name="arg2">Клиент которому перевели</param>
        private void AClient_MoneySended(AClient arg1, AClient arg2)
        {
            var log = new LogModel() { Message = $"{arg1} перевел деньги {arg2}" };
            context.Logs.Add(log);
            context.SaveChanges();
        }
 
        /// <summary>
        /// Обработчик события закрытия депозита
        /// </summary>
        /// <param name="arg1">Клиент</param>
        /// <param name="arg2">Депозит</param>
        private void AClient_DepositClosed(AClient arg1, DepositModel arg2)
        {
            var log = new LogModel() { Message = $"{arg1}, ваш депозит закрыт" };
            context.Logs.Add(log);
            context.SaveChanges();
        }

        /// <summary>
        /// Обработчик события закрытия кредита
        /// </summary>
        /// <param name="arg1">Клиент</param>
        /// <param name="arg2">Кредит</param>
        private void AClient_CreditClosed(AClient arg1, CreditModel arg2)
        {
            var log = new LogModel() { Message = $"{arg1}, ваш кредит закрыт" };
            context.Logs.Add(log);
            context.SaveChanges();
        }


        /// <summary>
        /// Вызов события изменения данных поля
        /// </summary>
        /// <param name="name">Имя поля</param>
        private protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
