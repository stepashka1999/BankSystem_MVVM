using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class EmployeeModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<EmployeeModel> DataChanged;

        private int id;
        public int Id { get => id; set { id = value; OnPropertyChanged(nameof(id)); } }

        
        private string firstName;
        public string FirstName { get => firstName; set { firstName = value; OnPropertyChanged(nameof(FirstName)); } }


        private string secondName;
        public string SecondName { get => secondName; set { secondName = value; OnPropertyChanged(nameof(SecondName)); } }


        private long phone;
        public long Phone { get => phone; set { phone = value; OnPropertyChanged(nameof(Phone)); } }


        private DepartamentModel departament;
        public DepartamentModel Departament { get => departament; set { departament = value; OnPropertyChanged(nameof(Departament)); } }


        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public EmployeeModel() { }
        public EmployeeModel(string fName, string sName, long phone, DepartamentModel dep)
        {
            FirstName = fName;
            SecondName = sName;
            Phone = phone;
            Departament = dep;       
        }

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
