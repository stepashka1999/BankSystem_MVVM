using BankSystem.Models;
using BankSystem.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BankSystem.ViewModels
{
    /// <summary>
    /// ViewModel добавления депозита
    /// </summary>
    class AddDepositViewModel:INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменения данных
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        

        /// <summary>
        /// Значение по умлчаниею для Month и Amount
        /// </summary>
        private static string defaultValue = "0";
        
        /// <summary>
        /// Клиент
        /// </summary>
        private AClient holder;         // Клиент

        /// <summary>
        /// Депозит
        /// </summary>
        private DepositModel deposit;   // Депозит

        /// <summary>
        /// Контекст БД
        /// </summary>
        private BankDbContext context;  // Контекст БД


        //Текст ошибки
        private string error;
        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string Error { get => error; set { error = value; OnPropertyChanged(nameof(Error)); } }


        //Текущая сумма депозита
        private string currentAmount = defaultValue;
        /// <summary>
        /// Текущая сумма депозита
        /// </summary>
        public string CurrentAmount { get => currentAmount; set { currentAmount = value; OnPropertyChanged(nameof(CurrentAmount)); } }
        

        /// <summary>
        /// Максимальная сумма депозита
        /// </summary>
        public decimal MaxAmount { get; set; }


        // Колиечество месяцев
        private string month = defaultValue;
        /// <summary>
        /// Колиечество месяцев
        /// </summary>
        public string Month { get => month; set { month = value; OnPropertyChanged(nameof(Month)); } }




        /// <summary>
        /// Конструктор ViewModel добавления депозита
        /// </summary>
        /// <param name="client">Выбранный клиент</param>
        public AddDepositViewModel(AClient client)
        {
            holder = client;
            deposit = new DepositModel();
            MaxAmount = holder.Amount;
            context = new BankDbContext();
        }


        //------ Methods ----------
        #region Methods

        /// <summary>
        /// Верификация данных
        /// </summary>
        /// <returns></returns>
        /// <returns></returns>
        private bool VerifyData()
        {
            if(decimal.Parse(CurrentAmount)==0)
            {
                Error = "Сумма депозита не может быть равной нулю";
                return false;
            }

            if (decimal.Parse(Month) == 0)
            {
                Error = "Количество месяцев не может быть равным нулю";
                return false;
            }


            return true;
        }

        /// <summary>
        /// Вызов события изменения данных
        /// </summary>
        /// <param name="name"></param>
        private protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Добавление депозита в БД
        /// </summary>
        private void AddDeposit()
        {
            deposit.HolderId = holder.Id;
            deposit.Amount = decimal.Parse(CurrentAmount);
            deposit.Month = int.Parse(Month);
            deposit.Percent = holder.CreditHistory.Percent;

            holder.Amount -= deposit.Amount;

            context.Deposits.Add(deposit);
            context.SaveChanges();
            context.Dispose();
        }

        #endregion

        //------ Commands ---------
        #region Commands

        /// <summary>
        /// Комманда добавления нового депозита
        /// </summary>
        public ICommand AddDepositCommand
        {
            get
            {
                return new DelegateCommand(obj=>
                {
                    if (VerifyData())
                    {
                        AddDeposit();

                        (obj as Window).DialogResult = true;
                        (obj as Window).Close();
                    }
                });
            }
        }

        /// <summary>
        /// Комманда отмены добавления депозита
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
