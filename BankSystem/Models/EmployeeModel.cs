using System;
using System.ComponentModel;

namespace BankSystem.Models
{
    /// <summary>
    /// Класс сотрудника
    /// </summary>
    public class EmployeeModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменения данных поля
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Событие изменения данных сотрудника
        /// </summary>
        public event Action<EmployeeModel> DataChanged;


        //Id сотрудника
        private int id;
        /// <summary>
        /// Id сотрудника
        /// </summary>
        public int Id { get => id; set { id = value; OnPropertyChanged(nameof(id)); } }

        
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
        private long phone;
        /// <summary>
        /// Номер телефона
        /// </summary>
        public long Phone { get => phone; set { phone = value; OnPropertyChanged(nameof(Phone)); } }


        //Департамент
        private DepartamentModel departament;
        /// <summary>
        /// Департамент
        /// </summary>
        public DepartamentModel Departament { get => departament; set { departament = value; OnPropertyChanged(nameof(Departament)); } }


        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public EmployeeModel() { }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="fName">Имя</param>
        /// <param name="sName">Фамилия</param>
        /// <param name="phone">Номер телефона</param>
        /// <param name="dep">Департамент</param>
        public EmployeeModel(string fName, string sName, long phone, DepartamentModel dep)
        {
            FirstName = fName;
            SecondName = sName;
            Phone = phone;
            Departament = dep;       
        }


        /// <summary>
        /// Вызывает событие изменения данных поля
        /// </summary>
        /// <param name="name">Имя поля</param>
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        /// <summary>
        /// Изменение данных
        /// </summary>
        /// <param name="fName">Имя</param>
        /// <param name="sName">Фамилия</param>
        /// <param name="phone">Нмоер телефона</param>
        public void EditData(string fName, string sName, long phone)
        {
            FirstName = fName;
            SecondName = sName;
            Phone = phone;
            DataChanged?.Invoke(this);
        }

        public override string ToString()
        {
            return $"{FirstName} {SecondName}";
        }
    }
}
