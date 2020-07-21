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
    /// Класс ViewModel добавления кредита
    /// </summary>
    class AddCreditViewModel:INotifyPropertyChanged
    {
        /// <summary>
        /// Событе изменения данных
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        
        /// <summary>
        /// Клиент
        /// </summary>
        private AClient holder;         // Клиент

        /// <summary>
        /// Контекст БД
        /// </summary>
        private BankDbContext context;  // Контекст БД

        /// <summary>
        /// Кредит
        /// </summary>
        private CreditModel credit;     // Кредит


        /// <summary>
        /// Значение по-умолчанию для amount и month
        /// </summary>
        private static string defaultValue = "0";

        // Текст ошибки
        private string error;
        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string Error { get => error; set { error = value; OnPropertyChanged(nameof(Error)); } }


        // Сумма кредита
        private string amount = defaultValue;
        /// <summary>
        /// Сумма кредита
        /// </summary>
        public string Amount { get => amount; set { amount = value; OnPropertyChanged(nameof(Amount)); } }


        // Срок кредита в месяцах
        private string month = defaultValue;
        /// <summary>
        /// Срок кредита в месяцах
        /// </summary>
        public string Month { get => month; set { month = value; OnPropertyChanged(nameof(Month)); } }


        /// <summary>
        /// Конструктор ViewModel добавления кредита
        /// </summary>
        /// <param name="holder">Клиент</param>
        public AddCreditViewModel(AClient holder)
        {
            this.holder = holder;
            context = new BankDbContext();
            credit = new CreditModel();
        }

        //----- Methods -----
        #region Methods

        /// <summary>
        /// Верификация данных
        /// </summary>
        /// <returns></returns>
        private bool VerifyData()
        {
            if (decimal.Parse(Amount) == 0)
            {
                Error = "Сумма кредита не может быть равной нулю";
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
        /// Заполнение кредита
        /// </summary>
        private void FillCredit()
        {
            credit.HolderId = holder.Id;
            credit.Amount = decimal.Parse(Amount);
            credit.Month = int.Parse(Month);
            credit.Percent = holder.CreditHistory.Percent;
        }

        /// <summary>
        /// Добавление кредита в БД
        /// </summary>
        private void AddCredit()
        {
            FillCredit();
            holder.Amount += credit.Amount;

            context.Credits.Add(credit);
            context.SaveChanges();
        }

        /// <summary>
        /// Вызвает событие изменения данных
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
        /// Комманда добавления нового кредита
        /// </summary>
        public ICommand AddCreditCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    if (VerifyData())
                    {
                        AddCredit();

                        (obj as Window).DialogResult = true;
                        (obj as Window).Close();
                    }
                });
            }
        }

        /// <summary>
        /// Комманда отмены добавления нового кредита
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
