using System.ComponentModel;
using System.Data.Entity;
using System.Windows.Input;
using BankSystem.Models;
using BankSystem.Views;

namespace BankSystem.ViewModels
{
    /// <summary>
    /// Класс ViewModel для работы с департаментами
    /// </summary>
    class DepartamentViewModel: INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменения данных поля
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Контекст БД Банка
        /// </summary>
        private BankDbContext context;


        //Выбранный сотрудник
        private EmployeeModel selectedEmployee;
        /// <summary>
        /// Выбранный сотрудник
        /// </summary>
        public EmployeeModel SelectedEmployee { get => selectedEmployee; set { selectedEmployee = value; OnPropertyChanged(nameof(SelectedEmployee)); } }


        //ВЫбранный департамент
        private DepartamentModel selectedDepartament;
        /// <summary>
        /// Выбранный департамент
        /// </summary>
        public DepartamentModel SelectedDepatament { get => selectedDepartament; set { selectedDepartament = value; OnPropertyChanged(nameof(SelectedDepatament)); } }


        /// <summary>
        /// Коллекция департаментов
        /// </summary>
        public BindingList<DepartamentModel> Departaments { get; set; }


        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public DepartamentViewModel()
        {
            context = new BankDbContext();

            context.Departaments.Load();
            context.Employees.Load();
            Departaments = context.Departaments.Local.ToBindingList();
        }


        //----- Methods -----
        #region Methods


        /// <summary>
        /// Вызов события изменения данных поля
        /// </summary>
        /// <param name="name">Имя поля</param>
        private protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        /// <summary>
        /// Добавление сотрудника
        /// </summary>
        private void AddEmployee()
        {
            var addEmplWindow = new AddEmployeeWindowView();
            addEmplWindow.ShowDialog();

            SelectedDepatament = SelectedDepatament;
        }


        /// <summary>
        /// Редактирование данных сотрудника
        /// </summary>
        private void EditEmployee()
        {
            var window = new EditEmployeeWindowView(SelectedEmployee);
            if (window.ShowDialog() == true)
            {
                context.Employees.Load();
                context.Departaments.Load();
            }
        }

        /// <summary>
        /// Удаление сотрудника
        /// </summary>
        private void DeleteEmployee()
        {
            context.Employees.Remove(SelectedEmployee);
            context.SaveChanges();
        }

        #endregion


        //----- Commands -----
        #region Commands


        /// <summary>
        /// Команда добавления сотрудника
        /// </summary>
        public ICommand AddEmployeeCommand
        {
            get
            {
                return new DelegateCommand( obj =>
                {
                    AddEmployee();
                }, obj => SelectedDepatament != null);
            }
        }


        /// <summary>
        /// Команда удаления сотрудника
        /// </summary>
        public ICommand DeleteEmployeeCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    DeleteEmployee();
                }, obj => SelectedEmployee != null);
            }
        }


        /// <summary>
        /// Команда редактирования сотрудника
        /// </summary>
        public ICommand EditEmployeeCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    EditEmployee();  
                }, obj => SelectedEmployee!= null);
            }
        }

        #endregion
    }
}
