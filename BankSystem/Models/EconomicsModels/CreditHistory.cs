using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class CreditHistory: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int id;
        public int Id { get => id; set { id = value; OnPropertyChanged(nameof(Id)); } }

        private string name;
        public string Name { get => name; set { name = value; OnPropertyChanged(nameof(Name)); } }
        
        private int percent;
        public int Percent { get => percent; set { percent = value; OnPropertyChanged(nameof(Percent)); } }

        
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public override string ToString()
        {
            return $"{Name} - {Percent}%";
        }

    }
}
