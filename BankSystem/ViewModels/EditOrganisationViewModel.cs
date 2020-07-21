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
    class EditOrganisationViewModel:INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменения данных
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Состояние кнопки Add
        /// </summary>
        private bool IsActive { get; set; }     // Состояние кнопки Add

        /// <summary>
        /// Организация
        /// </summary>
        private OrganisationModel organisation; // Организация

        /// <summary>
        /// Контекст БД
        /// </summary>
        private BankDbContext context;          // Контекст БД


        /// <summary>
        /// Коллекция вариантов кредитной истории
        /// </summary>
        public BindingList<CreditHistory> CreditHistories { get; set; }

        //Кредитная история
        private CreditHistory selectedItem;
        /// <summary>
        /// Кредитная история
        /// </summary>
        public CreditHistory SelectedItem { get => selectedItem; set { selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); } }

        
        //Текст ошибки
        private string error;
        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string Error { get => error; set { error = value; OnPropertyChanged(nameof(Error)); } }


        //Название организации
        private string nameOrg;
        /// <summary>
        /// Название организации
        /// </summary>
        public string NameOrg { get => nameOrg; set { nameOrg = value; OnPropertyChanged(nameof(nameOrg)); } }


        //Номер аккаунта
        private string account;
        /// <summary>
        /// Номер аккаунта
        /// </summary>
        public string Account { get => account; set { account = value; OnPropertyChanged(nameof(Account)); } }


        //Сумма счета
        private string amount;
        /// <summary>
        /// Сумма счета
        /// </summary>
        public string Amount { get => amount; set { amount = value; OnPropertyChanged(nameof(Amount)); } }



        /// <summary>
        /// Конструктор
        /// </summary>
        public EditOrganisationViewModel(OrganisationModel organisation)
        {
            this.organisation = organisation;
            context = new BankDbContext();

            context.CreditHistories.Load();
            CreditHistories = context.CreditHistories.Local.ToBindingList();

            FillFields();
        }



        //----- Methods -----
        #region Methods

        private void FillFields()
        {
            NameOrg = organisation.Name;
            Account = organisation.Account.ToString();
            Amount = organisation.Amount.ToString();
            SelectedItem = organisation.CreditHistory;
        }


        /// <summary>
        /// Верификация суммы аккаунта
        /// </summary>
        /// <returns>Результат верификации</returns>
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
        /// Верификация номера аккаунта
        /// </summary>
        /// <returns>Результат верификации</returns>
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
        private void VerifyData()
        {
            if (string.IsNullOrEmpty(NameOrg)) throw new Exception("Имя заполенено неверно");
            if (VerifyAccount() == false) throw new Exception("Номер аккаунта заполнен неверно");
            if (VerifyAmount() == false) throw new Exception("Сумма аккаунта заполнена неверно");
            if (SelectedItem == null) throw new Exception("Не выбрана кредитная история");

        }

        /// <summary>
        /// Общая верификация с обработкой ошибок
        /// </summary>
        /// <returns>Результат верификации</returns>
        private bool Verify()
        {
            try
            {
                VerifyData();
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
        /// <returns>Результат успешности парсинга</returns>
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
        /// Заполнение полей организации
        /// </summary>
        private void FillOrg()
        {
            organisation.Name = NameOrg;
            organisation.Account = AccountParse(Account);
            organisation.Amount = decimal.Parse(Amount);
            organisation.CreditHistory = SelectedItem;
        }
        
        /// <summary>
        ///Добавление организации в БД 
        /// </summary>
        private void EditOrganisation()
        {
            FillOrg();

            context.SaveChanges();
            context.Dispose();
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
        /// Команда добавления новой организации 
        /// </summary>
        public ICommand Add
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    if (IsActive)
                    {
                        EditOrganisation();

                        (obj as Window).DialogResult = true;
                        (obj as Window).Close();
                    }
                });
            }
        }


        /// <summary>
        /// Команда отмены добавления новой организации
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
                    IsActive = Verify();
                });
            }
        }

        #endregion

    }
}
