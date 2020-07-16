using BankSystem.Models;
using BankSystem.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BankSystem.ViewModels
{
    class AddEmployeeViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private AddEmployeeWindowView window;
        private EmployeeModel employee;
        
        
        private DepartamentModel selectedItem;
        public DepartamentModel SelectedItem { get => selectedItem; set { selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); } }


        public bool IsActive => VerifyData();

        public BindingList<DepartamentModel> Departaments { get; set; }


        private string firstName;
        public string FirstName { get => firstName; set { firstName = value; OnPropertyChanged(nameof(FirstName)); } }


        private string secondName;
        public string SecondName { get => secondName; set { secondName = value; OnPropertyChanged(nameof(SecondName)); } }


        string phone;
        public string Phone { get => phone; set { phone = value; OnPropertyChanged(nameof(Phone)); } }


        private string error;
        public string Error { get => error; set { error = value; OnPropertyChanged(nameof(Error)); } }


        public AddEmployeeViewModel(BankDbContext context, EmployeeModel employee, AddEmployeeWindowView window)
        {
            this.window = window;
            this.employee = employee;

            context.Departaments.Load();
            Departaments =  context.Departaments.Local.ToBindingList();
        }

        private bool VerifyData()
        {
            if (Phone == null) return false;

            if( Phone.Length != 17)
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

            if(SelectedItem == null)
            {
                Error = $"Департамент не выбран";
                return false;
            }

            return true;
        }

        private long PhoneParse(string text)
        {
            string phone = string.Empty;

            foreach(var symbol in text)
            {
                if (char.IsDigit(symbol)) phone += symbol;
            }

            return long.Parse(phone);
        }

        private void FillEmployee()
        {
            employee.FirstName = FirstName;
            employee.SecondName = SecondName;
            employee.Phone = PhoneParse(Phone);
            employee.Departament = SelectedItem;
        }

        public ICommand AddCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    FillEmployee();
                    window.DialogResult = true;
                    window.Close();
                }, obj => IsActive == true);
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    window.DialogResult = false;
                    window.Close();
                });
            }
        }

        private protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
