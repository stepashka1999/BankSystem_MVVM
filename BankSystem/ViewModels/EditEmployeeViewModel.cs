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
    /// Класс ViewModel редактирование сотрудника
    /// </summary>
    class EditEmployeeViewModel: INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменения данных
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        
        /// <summary>
        /// Контекст БД
        /// </summary>
        private BankDbContext context;          // Контекст БД

        /// <summary>
        /// Сотрудник
        /// </summary>
        private EmployeeModel employee;         // Сотрудник


        /// <summary>
        /// Состояние кнопки Edit
        /// </summary>
        public bool IsActive => VerifyData();


        //Имя
        private string firstName;
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get => firstName; set { firstName = value; OnPropertyChanged(nameof(FirstName)); } }

        //Фамилия
        private string secondName;
        /// <summary>
        /// Фамилия
        /// </summary>
        public string SecondName { get => secondName; set { secondName = value; OnPropertyChanged(nameof(SecondName)); } }

        //Номер телефона
        string phone;
        /// <summary>
        /// Номер телефона
        /// </summary>
        public string Phone { get => phone; set { phone = value; OnPropertyChanged(nameof(Phone)); } }


        //Текст ошибки
        private string error;
        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string Error { get => error; set { error = value; OnPropertyChanged(nameof(Error)); } }


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="employee"></param>
        public EditEmployeeViewModel(EmployeeModel employee)
        {
            context = new BankDbContext();
            this.employee = employee;

            context.Departaments.Load();
            Departaments = context.Departaments.Local.ToBindingList();

            FillFields();
        }


        //----- Methods -----
        #region Methods

        /// <summary>
        /// Заполение полей текущими данными
        /// </summary>
        private void FillFields()
        {
            SelectedItem = employee.Departament;
            FirstName = employee.FirstName;
            SecondName = employee.SecondName;
            Phone = employee.Phone.ToString();
        }

        /// <summary>
        /// Верификация данных
        /// </summary>
        /// <returns></returns>
        private bool VerifyData()
        {
            if (Phone == null) return false;

            if (Phone.Length != 17 && Phone.Length != 11)
            {
                Error = "Номер телефона введен неверно";
                return false;
            }

            var res = PhoneParse(Phone).ToString();
            if (res.Length != 11)
            {
                Error = "Номер телефона введен неверно";
                return false;
            }

            if (SelectedItem == null)
            {
                Error = $"Департамент не выбран";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Парсинг номера телефона
        /// </summary>
        /// <param name="text">Номер телефона в string</param>
        /// <returns>Номекр телефона long</returns>
        private long PhoneParse(string text)
        {
            string phone = string.Empty;

            foreach (var symbol in text)
            {
                if (char.IsDigit(symbol)) phone += symbol;
            }

            return long.Parse(phone);
        }

        /// <summary>
        /// Изменение данных сотрудника
        /// </summary>
        private void EditEmployee()
        {
            employee.FirstName = FirstName;
            employee.SecondName = SecondName;
            employee.Phone = PhoneParse(Phone);
            employee.Departament = SelectedItem;

            context.SaveChanges();
            context.Dispose();
        }

        /// <summary>
        /// Вызов события изменения данных
        /// </summary>
        /// <param name="name">Имя измененных данных</param>
        private protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion


        //----- Commands -----
        #region Commands

        /// <summary>
        /// Команда изменения данных сотрудника
        /// </summary>
        public ICommand AddCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    if (IsActive)
                    {
                        EditEmployee();
                        (obj as Window).DialogResult = true;
                        (obj as Window).Close();
                    }
                }, obj => IsActive == true);
            }
        }

        /// <summary>
        /// Команда отмены изменений данных сотрудника 
        /// </summary>
        public ICommand CloseCommand
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
                return new DelegateCommand(obj=>
                {
                    _ = IsActive;
                });
            }
        }


        #endregion

    }
}
