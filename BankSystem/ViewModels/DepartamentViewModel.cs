using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BankSystem.Models;
using BankSystem.Views;

namespace BankSystem.ViewModels
{
    class DepartamentViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private BankDbContext context;


        private EmployeeModel selectedEmployee;
        public EmployeeModel SelectedEmployee { get => selectedEmployee; set { selectedEmployee = value; OnPropertyChanged(nameof(SelectedEmployee)); } }


        private DepartamentModel selectedDepartament;
        public DepartamentModel SelectedDepatament { get => selectedDepartament; set { selectedDepartament = value; OnPropertyChanged(nameof(SelectedDepatament)); } }

        public BindingList<DepartamentModel> Departaments { get; set; }


        public DepartamentViewModel(string connection)
        {
            context = new BankDbContext(connection);

            context.Departaments.Load();
            context.Employees.Load();
            Departaments = context.Departaments.Local.ToBindingList();
        }

        private protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void AddEmployee()
        {
            var empl = new EmployeeModel();
            var addEmplWindow = new AddEmployeeWindowView(context, empl);
            if(addEmplWindow.ShowDialog() == true)
            {
                context.Employees.Add(empl);
                context.SaveChanges();
                SelectedDepatament = SelectedDepatament;
            }
        }

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


        private void DeleteEmployee()
        {
            context.Employees.Remove(SelectedEmployee);
            context.SaveChanges();
        }

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

        private void EditEmployee()
        {
            var window = new EditEmployeeWindowView(context, SelectedEmployee);
            if (window.ShowDialog() == true)
            {
                context.SaveChanges();
            }
        }

    }
}
