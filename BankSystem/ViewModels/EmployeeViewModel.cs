using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Models;

namespace BankSystem.ViewModels
{
    class EmployeeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BankDbContext context;

        private EmployeeModel selectedEmployee;
        public EmployeeModel SelectedEmployee { get => selectedEmployee; set { selectedEmployee = value; OnPropertyChanged(nameof(SelectedEmployee)); } }

        public BindingList<EmployeeModel> Employees { get; set; }


        public EmployeeViewModel(string connection)
        {
            context = new BankDbContext(connection);
            context.Employees.Load();
            Employees = context.Employees.Local.ToBindingList();
        }

        private protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
