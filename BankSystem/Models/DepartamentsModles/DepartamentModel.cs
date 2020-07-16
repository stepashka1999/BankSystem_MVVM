using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class DepartamentModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<DepartamentModel,DepartamentModel, EmployeeModel> EmployeeTransacted;
        public event Action<DepartamentModel, EmployeeModel> AddedEmployee;
        public event Action<DepartamentModel, EmployeeModel> RemovedEmployee;

        private int id;
        public int Id { get => id; set { id = value; OnPropertyChanged(nameof(Id)); } }

        private string name;
        public string Name { get => name; set { name = value; OnPropertyChanged(nameof(Name)); } }

        public BindingList<EmployeeModel> Employees { get; set; }


        public DepartamentModel() { }
        public DepartamentModel(string Name)
        {
            Employees = new BindingList<EmployeeModel>();
            this.Name = Name;
        }


        public void AddEmployee(EmployeeModel empl)
        {
            Employees.Add(empl);
            AddedEmployee?.Invoke(this, empl);
        }

        public void RemoveEmployee(EmployeeModel empl)
        {
            Employees.Remove(empl);
            RemovedEmployee?.Invoke(this, empl);
        }

        public void TransactEmployee(DepartamentModel dep, EmployeeModel empl)
        {
            Employees.Remove(empl);
            dep.AddEmployee(empl);
            EmployeeTransacted?.Invoke(this, dep, empl);
        }


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
