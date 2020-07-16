using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class AEcoItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int id;
        public int Id { get => id; set { id = value; OnPropertyChanged(nameof(Id)); } }

        
        private int holderId;
        public int HolderId { get => holderId; set { holderId = value; OnPropertyChanged(nameof(HolderId)); } }


        private AClient holder;
        public AClient Holder { get => holder; set { holder = value; OnPropertyChanged(nameof(Holder)); } }


        private decimal amount;
        public decimal Amount { get => amount; set { amount = value; OnPropertyChanged(nameof(Amount)); } }


        private int percent;
        public int Percent { get => percent; set { percent = value; OnPropertyChanged(nameof(Percent)); } }


        private int month;
        public int Month { get => month; set { month = value; OnPropertyChanged(nameof(Month)); } }


        private decimal payment;
        public decimal Payment { get => payment; set { payment = value; OnPropertyChanged(nameof(Payment)); } }



        public AEcoItem() { }
        public AEcoItem(AClient Holder, decimal Amount, int Month)
        {
            HolderId = Holder.Id;
            Percent = Holder.CreditHistory.Percent;
            this.Holder = Holder;
            this.Amount = Amount;
            this.Month = Month;
        }

        public virtual void MakePayment()
        {
            throw new NotImplementedException();
        }




        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public override string ToString()
        {
            return $"{Amount} | {Percent} % | {Month} M";
        }
    }
}
