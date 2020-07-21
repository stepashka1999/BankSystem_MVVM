using BankSystem.Models;
using System.ComponentModel;
using System.Data.Entity;
using System.Windows;
using System.Windows.Input;

namespace BankSystem.ViewModels
{

    /// <summary>
    /// ViewModel добавления нового сотрудника
    /// </summary>
    class AddEmployeeViewModel:INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменения данных
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Новый сотрудник
        /// </summary>
        private EmployeeModel employee;
       
        /// <summary>
        /// Контекст данных
        /// </summary>
        private BankDbContext context;
        
        
        // Выбранный департамент
        private DepartamentModel selectedItem;
        /// <summary>
        /// Выбранный департамент
        /// </summary>
        public DepartamentModel SelectedItem { get => selectedItem; set { selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); } }


        // Состояние активности кнопки Add
        private bool isActive;
        /// <summary>
        /// Состояние активности кнопки Add
        /// </summary>
        private bool IsActive { get => isActive; set { isActive = value; OnPropertyChanged(nameof(IsActive)); } }


        /// <summary>
        /// Коллекция департаментов
        /// </summary>
        public BindingList<DepartamentModel> Departaments { get; set; }


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


        // Номер телефона
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



        //-------- Конструкторы -----------
        /// <summary>
        /// Конструктор ViewModel добавления сотрудника
        /// </summary>
        /// <param name="context">Контектс базы данных</param>
        /// <param name="employee">Сотрудник</param>
        public AddEmployeeViewModel()
        {
            employee = new EmployeeModel();
            context = new BankDbContext();

            context.Departaments.Load();
            Departaments = context.Departaments.Local.ToBindingList();
        }

        //------------ Methods --------------

        #region Methods

        /// <summary>
        /// Добавление сотрудника в БД
        /// </summary>
        private void AddEmployee()
        {
            FillEmployee();
            context.Employees.Add(employee);
            context.SaveChanges();
            context.Dispose();
        }

        /// <summary>
        /// Верификация данных
        /// </summary>
        /// <returns>Рузальтат верификации</returns>
        private bool VerifyData()
        {
            if(string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(SecondName))
            {
                Error = "Имя или Фамилия не введены";
                return false;
            }

            if (Phone == null) return false;

            var res = PhoneParse(Phone).ToString();
            if (res.Length != 11)
            {
                Error = "Номер телефона введен неверно";
                return false;
            }

            if(SelectedItem == null)
            {
                Error = $"Департамент не выбран";
                return false;
            }


            Error = "";
            return true;
        }

        /// <summary>
        /// Парсинг номера телефона из string в long
        /// </summary>
        /// <param name="text">Номер телефона в string</param>
        /// <returns>Номер телефона в long</returns>
        private long PhoneParse(string text)
        {
            string phone = string.Empty;
            long res;

            foreach(var symbol in text)
            {
                if (char.IsDigit(symbol)) phone += symbol;
            }

            if (string.IsNullOrEmpty(phone) || long.TryParse(phone, out res) == false)
                return default(long);

            return res;
        }

        /// <summary>
        /// Заполение полей сотрудника
        /// </summary>
        private void FillEmployee()
        {
            employee.FirstName = FirstName;
            employee.SecondName = SecondName;
            employee.Phone = PhoneParse(Phone);
            employee.Departament = SelectedItem;
        }

        /// <summary>
        /// Метод вызывающий событие изменения данных
        /// </summary>
        /// <param name="name">Наименование измененных данные</param>
        private protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion


        //----------- Commands --------------

        #region Commands

        /// <summary>
        /// Команда добавления сотрудника
        /// </summary>
        public ICommand AddCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    AddEmployee();
                    (obj as Window).Close();
                },
                obj => IsActive == true);
            }
        }


        /// <summary>
        /// Комманда отмены создания сотрудника
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
        /// Комманда верификации данных
        /// </summary>
        public ICommand VerifyDataCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    Phone = (obj as string);
                    IsActive = VerifyData();
                });
            }
        }

        #endregion

    }
}
