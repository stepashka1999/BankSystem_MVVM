using BankSystem.Models;
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
    class EditClientViewModel:INotifyPropertyChanged
    {

        /// <summary>
        /// Событие изменения данных
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;   // Событие изменения данных

        /// <summary>
        /// Контекст БД
        /// </summary>
        private BankDbContext context;  // Контекст БД

        /// <summary>
        /// Клиент
        /// </summary>
        private ClientModel client;     // Клиент


        /// <summary>
        /// Состояние кнопки Add
        /// </summary>
        private bool IsActive { get; set; }


        /// <summary>
        /// Коллекция вариантов кредитной истории
        /// </summary>
        public BindingList<CreditHistory> CreditHistories { get; set; }


        // Текст ошибки
        private string error;
        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string Error { get => error; set { error = value; OnPropertyChanged(nameof(Error)); } }


        // Кредитная история
        private CreditHistory selectedItem;
        /// <summary>
        /// Кредитная история
        /// </summary>
        public CreditHistory SelectedItem { get => selectedItem; set { selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); } }


        // Имя
        private string firstName;
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get => firstName; set { firstName = value; OnPropertyChanged(nameof(FirstName)); } }


        // Фамилия
        private string secondName;
        /// <summary>
        /// Фамилия
        /// </summary>
        public string SecondName { get => secondName; set { secondName = value; OnPropertyChanged(nameof(SecondName)); } }


        // Статус клиента(VIP или нет)
        private bool isVip;
        /// <summary>
        /// Статус клиента(VIP или нет)
        /// </summary>
        public bool IsVip { get => isVip; set { isVip = value; OnPropertyChanged(nameof(IsVip)); } }


        // Номер счета
        private string account;
        /// <summary>
        /// Номер счета
        /// </summary>
        public string Account { get => account; set { account = value; OnPropertyChanged(nameof(Account)); } }


        // Сумма счета
        private string amount;
        /// <summary>
        /// Сумма счета
        /// </summary>
        public string Amount { get => amount; set { amount = value; OnPropertyChanged(nameof(Amount)); } }




        /// <summary>
        /// Конструктор ViewModel добавления клиента
        /// </summary>
        public EditClientViewModel(ClientModel client)
        {
            this.client = client;
            context = new BankDbContext();

            context.CreditHistories.Load();
            CreditHistories = context.CreditHistories.Local.ToBindingList();

            FillFields();
        }

        //----- Methods -----
        #region Methods

        /// <summary>
        /// Заполнение полей даныыми клиента
        /// </summary>
        private void FillFields()
        {
            FirstName = client.FirstName;
            SecondName = client.SecondName;
            IsVip = client.IsVip;
            Account = client.Account.ToString();
            Amount = client.Amount.ToString();
            SelectedItem = client.CreditHistory;
        }


        /// <summary>
        /// Верификация суммы счета
        /// </summary>
        /// <returns>Рузальтат верификации</returns>
        private bool VerifyAmount()
        {
            var strAmount = Amount;

            if (string.IsNullOrEmpty(strAmount)) return false;

            decimal amount;
            if (decimal.TryParse(strAmount, out amount) == false) return false;

            if (amount < 0) return false;

            return true;
        }

        /// <summary>
        /// Верификация номера счета
        /// </summary>
        /// <returns>Реузльтат верификации</returns>
        private bool VerifyAccount()
        {
            if (string.IsNullOrEmpty(Account)) return false;

            long account;
            if (AccountParse(Account, out account) == false) return false;

            if (account < 0) return false;

            if (account.ToString().Length != 16) return false;

            return true;
        }

        /// <summary>
        /// Общая верификация
        /// </summary>
        private void Verify()
        {
            if (string.IsNullOrEmpty(FirstName))
            {
                throw new Exception("Имя заполнено неверно");
            }
            if (string.IsNullOrEmpty(SecondName))
            {
                throw new Exception("Фамилия заполнено неверно");
            }
            if (VerifyAccount() == false)
            {
                throw new Exception("Аккаун заполнен неверно");
            }
            if (VerifyAmount() == false)
            {
                throw new Exception("Сумма заполнено неверно");
            }
            if (SelectedItem == null)
            {
                throw new Exception("Кредитная история не выбрана");
            }
        }

        /// <summary>
        /// Верификация данных
        /// </summary>
        /// <returns>рузальтат верификации</returns>
        private bool VerifyData()
        {
            try
            {
                Verify();
            }
            catch (Exception e)
            {
                Error = e.Message;
                return false;
            }


            return true;
        }


        /// <summary>
        /// Парсинг номера аккаунта
        /// </summary>
        /// <param name="text">Номер аккаунта в string</param>
        /// <param name="account">Переменная для хранения результата парсинга в long</param>
        /// <returns>Успешность парсинга</returns>
        private bool AccountParse(string text, out long account)
        {
            var res = text.Replace(" ", "");
            return long.TryParse(res, out account);
        }

        /// <summary>
        /// Парсинг номера аккаунта
        /// </summary>
        /// <param name="text">Номер аккаунта в string</param>
        /// <returns>Номер аккаунта в long</returns>
        private long AccountParse(string text)
        {
            var res = text.Replace(" ", "");

            return long.Parse(res);
        }


        /// <summary>
        /// Заполнение полей клиента
        /// </summary>
        private void FillClient()
        {
            client.FirstName = FirstName;
            client.SecondName = SecondName;
            client.IsVip = IsVip;
            client.Account = AccountParse(Account);
            client.Amount = decimal.Parse(Amount);
            client.CreditHistory = SelectedItem;
        }

        /// <summary>
        /// Добавление клиента в БД
        /// </summary>
        private void EditClient()
        {
            FillClient();
            context.SaveChanges();
            context.Dispose();
        }


        /// <summary>
        /// Вызов события изменения данных
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
        /// Команда добавления нового клиента
        /// </summary>
        public ICommand Add
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    if (IsActive)
                    {
                        EditClient();

                        (obj as Window).DialogResult = true;
                        (obj as Window).Close();
                    }
                });
            }
        }


        /// <summary>
        /// Команда отмены добавления нового клиента
        /// </summary>
        public ICommand Close
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

        /// <summary>
        /// Команда верификации данных
        /// </summary>
        public ICommand VerifyCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    IsActive = VerifyData();
                });
            }
        }

        #endregion
    }
}
