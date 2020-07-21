using System;
using System.ComponentModel;

namespace BankSystem.Models
{
    /// <summary>
    /// Класс департамента
    /// </summary>
    public class DepartamentModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменения данных поля
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Событие перевода сотрудника в другой отдел
        /// </summary>
        public event Action<DepartamentModel,DepartamentModel, EmployeeModel> EmployeeTransacted;
        /// <summary>
        /// Событие добавленипя сотрудника
        /// </summary>
        public event Action<DepartamentModel, EmployeeModel> AddedEmployee;
        /// <summary>
        /// Событие удаления сотрудника
        /// </summary>
        public event Action<DepartamentModel, EmployeeModel> RemovedEmployee;


        // Id департамента
        private int id;
        /// <summary>
        /// Id сотрудника
        /// </summary>
        public int Id { get => id; set { id = value; OnPropertyChanged(nameof(Id)); } }


        //Имя департамента
        private string name;
        /// <summary>
        /// Имя департамента
        /// </summary>
        public string Name { get => name; set { name = value; OnPropertyChanged(nameof(Name)); } }


        /// <summary>
        /// Коллекция сотрудников
        /// </summary>
        public BindingList<EmployeeModel> Employees { get; set; }



        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public DepartamentModel() { }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="Name">Имя</param>
        public DepartamentModel(string Name)
        {
            Employees = new BindingList<EmployeeModel>();
            this.Name = Name;
        }



        /// <summary>
        /// Добавление сотрудника
        /// </summary>
        /// <param name="empl">Сотрудник</param>
        public void AddEmployee(EmployeeModel empl)
        {
            Employees.Add(empl);
            AddedEmployee?.Invoke(this, empl);
        }

        /// <summary>
        /// Удаление сотрудника
        /// </summary>
        /// <param name="empl">Сотрудник</param>
        public void RemoveEmployee(EmployeeModel empl)
        {
            Employees.Remove(empl);
            RemovedEmployee?.Invoke(this, empl);
        }


        /// <summary>
        /// Перевод сотрудника
        /// </summary>
        /// <param name="dep">Департамент</param>
        /// <param name="empl">Сотрудник</param>
        public void TransactEmployee(DepartamentModel dep, EmployeeModel empl)
        {
            Employees.Remove(empl);
            dep.AddEmployee(empl);
            EmployeeTransacted?.Invoke(this, dep, empl);
        }


        /// <summary>
        /// Вызывает событие изменения данных поля
        /// </summary>
        /// <param name="name">Имя поля</param>
        private protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
