using System;
using System.ComponentModel;

namespace BankSystem.Models
{
    /// <summary>
    /// Абстрактный класс объединяющий кредит и депозит
    /// </summary>
    public class AEcoItem : INotifyPropertyChanged
    {
        /// <summary>
        /// Собюытие изменения данных поля
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        // Id AEcoItem (CreditModel or DepositModel)
        private int id;
        /// <summary>
        /// Id AEcoItem (CreditModel or DepositModel)
        /// </summary>
        public int Id { get => id; set { id = value; OnPropertyChanged(nameof(Id)); } }


        // Id клиента
        private int holderId;
        /// <summary>
        /// Id клиента
        /// </summary>
        public int HolderId { get => holderId; set { holderId = value; OnPropertyChanged(nameof(HolderId)); } }


        //Клиент
        private AClient holder;
        /// <summary>
        /// Клиент
        /// </summary>
        public AClient Holder { get => holder; set { holder = value; OnPropertyChanged(nameof(Holder)); } }


        //Сумма
        private decimal amount;
        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Amount { get => amount; set { amount = value; OnPropertyChanged(nameof(Amount)); } }


        //Процент
        private int percent;
        /// <summary>
        /// Процент
        /// </summary>
        public int Percent { get => percent; set { percent = value; OnPropertyChanged(nameof(Percent)); } }


        //Срок в месяцах
        private int month;
        /// <summary>
        /// Срок в месяцах
        /// </summary>
        public int Month { get => month; set { month = value; OnPropertyChanged(nameof(Month)); } }


        //Сумма выплаты
        private decimal payment;
        /// <summary>
        /// Сумма выплаты
        /// </summary>
        public decimal Payment { get => payment; set { payment = value; OnPropertyChanged(nameof(Payment)); } }



        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public AEcoItem() { }
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="Holder">КЛиента</param>
        /// <param name="Amount">Сумма</param>
        /// <param name="Month">Срок в месяцах</param>
        public AEcoItem(AClient Holder, decimal Amount, int Month)
        {
            HolderId = Holder.Id;
            Percent = Holder.CreditHistory.Percent;
            this.Holder = Holder;
            this.Amount = Amount;
            this.Month = Month;
        }


        /// <summary>
        /// Соверщение выплаты
        /// </summary>
        public virtual void MakePayment()
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// Вызов события изменения данных поля
        /// </summary>
        /// <param name="name">Имя поля</param>
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
