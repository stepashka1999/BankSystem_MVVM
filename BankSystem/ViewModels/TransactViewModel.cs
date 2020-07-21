using BankSystem.Models;
using BankSystem.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BankSystem.ViewModels
{
    /// <summary>
    /// Класс ViewModel для переводов
    /// </summary>
    class TransactViewModel: INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменения данных
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
       
        /// <summary>
        /// Клиент
        /// </summary>
        private AClient client;             // Клиент
        /// <summary>
        /// Контекст БД
        /// </summary>
        private BankDbContext context;      // Контекст БД


        // Текст ошибки
        private string error;
        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string Error { get => error; set { error = value; OnPropertyChanged(nameof(Error)); } }


        // Выбранный клиент
        private AClient selectedClient;
        /// <summary>
        /// Выбранный клиент
        /// </summary>
        public AClient SelectedClient { get=> selectedClient; set { selectedClient = value; OnPropertyChanged(nameof(SelectedClient)); } }


        // Типа перевода
        private int transactType;
        /// <summary>
        /// Типа перевода
        /// </summary>
        public int TransactType { get=> transactType; set { transactType = value; OnPropertyChanged(nameof(TransactType)); } }


        // Сумма перевода
        private string amount;
        /// <summary>
        /// Сумма перевода
        /// </summary>
        public string Amount { get => amount; set { amount = value; OnPropertyChanged(nameof(Amount)); } }


        /// <summary>
        /// Коллекция клиентов
        /// </summary>
        public List<AClient> Clients { get; set; } = new List<AClient>();


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="client">Клиент</param>
        public TransactViewModel(AClient client)
        {
            this.client = client;
            context = new BankDbContext();

            context.Clients.Load();
            context.Organisations.Load();

            Clients.AddRange(context.Clients.Local.ToList());
            Clients.AddRange(context.Organisations.Local.ToList());

            Clients.Remove(client);
        }



        //----- Methods -----
        #region Mehtods

        /// <summary>
        /// Верификация данных
        /// </summary>
        /// <returns>Результат верификации</returns>
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

        /// <summary>
        /// Верификация суммы перевода
        /// </summary>
        /// <returns></returns>
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

            if (TransactType == 0 && client.Amount < amount)
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


        /// <summary>
        /// Перевод выбранному клиенту
        /// </summary>
        private void TransactTo()
        {
            client.SendMoneyTo(SelectedClient, decimal.Parse(Amount));
            context.SaveChanges();
        }

        /// <summary>
        /// Запрос денег у выбранного клиента
        /// </summary>
        private void TransactFrom()
        {
            SelectedClient.SendMoneyTo(client, decimal.Parse(Amount));
            context.SaveChanges();
        }

        /// <summary>
        /// Перевод
        /// </summary>
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


        /// <summary>
        /// Вызывает событие изменения данных
        /// </summary>
        /// <param name="name">Имя измененных данных</param>
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion


        //----- Commands -----
        #region Commands

        /// <summary>
        /// Команда перевода
        /// </summary>
        public ICommand TransactCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    if (VerifyData())
                    {
                        Transact();
                        (obj as Window).DialogResult = true;
                        (obj as Window).Close();
                    }
                });
            }
        }

        /// <summary>
        /// Команда отмены перевода
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    (obj as Window).DialogResult = false;
                    (obj as Window).Close();
                });
            }
        }

        #endregion

    }
}
