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
    class EditEmployeeViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BankDbContext context;
        private EditEmployeeWindowView window;
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


        public EditEmployeeViewModel(BankDbContext context, EmployeeModel employee, EditEmployeeWindowView window)
        {
            this.context = context;
            this.window = window;
            this.employee = employee;

            context.Departaments.Load();
            Departaments = context.Departaments.Local.ToBindingList();

            SelectedItem = employee.Departament;
            FirstName = employee.FirstName;
            SecondName = employee.SecondName;
            Phone = employee.Phone.ToString();
        }

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

        private long PhoneParse(string text)
        {
            string phone = string.Empty;

            foreach (var symbol in text)
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
