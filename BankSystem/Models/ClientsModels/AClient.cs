using System;
using System.ComponentModel;

namespace BankSystem.Models
{
    /// <summary>
    /// Абстрактный класс клиента
    /// </summary>
    public class AClient: INotifyPropertyChanged
    {
        /// <summary>
        /// Событие отправки денег
        /// </summary>
        public static event Action<AClient, AClient> MoneySended;
        /// <summary>
        /// Событие закрытия кредита
        /// </summary>
        public static event Action<AClient, CreditModel> CreditClosed;
        /// <summary>
        /// Событие закрытия депозита
        /// </summary>
        public static event Action<AClient, DepositModel> DepositClosed;
        /// <summary>
        /// События нехватки денег
        /// </summary>
        public static event Action<AClient> NotEnoughtMoney;


        /// <summary>
        /// Событие изменения данных
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        //Id клиента
        private int id;
        /// <summary>
        /// Id клиента
        /// </summary>
        public int Id { get => id; set { id = value; OnPropertyChanged(nameof(Id)); } }


        // Номер счета клиента
        private long account;
        /// <summary>
        /// Номер счета клиента
        /// </summary>
        public long Account { get => account; set { account = value; OnPropertyChanged(nameof(Account)); } }


        // Сумму счета клиента
        private decimal amount;
        /// <summary>
        /// Сумму счета клиента
        /// </summary>
        public decimal Amount { get => amount; set { amount = value; OnPropertyChanged(nameof(Amount)); } }


        /// <summary>
        /// Информация о клиенте
        /// </summary>
        public virtual string Info => "Lul";


        // Кредитная история
        private CreditHistory creditHistory;
        /// <summary>
        /// Кредитная история
        /// </summary>
        public CreditHistory CreditHistory { get => creditHistory; set { creditHistory = value; OnPropertyChanged(nameof(CreditHistory)); } }


        /// <summary>
        /// Коллекция кредитов
        /// </summary>
        public BindingList<CreditModel> Credits { get; set; }
        /// <summary>
        /// Коллекция депозитов
        /// </summary>
        public BindingList<DepositModel> Deposits { get; set; }

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public AClient() { }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="Account">Номер счета</param>
        /// <param name="Amount">Сумма счета</param>
        /// <param name="creditHistory">Кредитная история</param>
        public AClient(long Account, decimal Amount, CreditHistory creditHistory)
        {
            this.Account = Account;
            this.Amount = Amount;
            CreditHistory = creditHistory;
        }



        //----- Methods -----
        #region Methods

        /// <summary>
        /// Вызывает событие изменения параметров
        /// </summary>
        /// <param name="name">Имя измененных параметров</param>
        private protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Info)));
        }


        /// <summary>
        /// Вывод средств
        /// </summary>
        /// <param name="amount">Сумма</param>
        /// <returns></returns>
        public bool WithdrawMoney(decimal amount)
        {
            if (Amount >= amount)
            {
                NotEnoughtMoney?.Invoke(this);
                return false;
            }

            Amount -= amount;
            return true;
        }

        /// <summary>
        /// Пополнение счета
        /// </summary>
        /// <param name="amount">Сумма</param>
        public void PutMoney(decimal amount)
        {
            Amount += amount;
        }


        /// <summary>
        /// Добавление кредита
        /// </summary>
        /// <param name="credit">Кредит</param>
        public void AddCredit(CreditModel credit)
        {
            Credits.Add(credit);
        }

        /// <summary>
        /// Добавление депозита
        /// </summary>
        /// <param name="deposit">Депозит</param>
        public void AddDeposit(DepositModel deposit)
        {
            Deposits.Add(deposit);
        }


        /// <summary>
        /// Выплата по кредтиам и депозитам
        /// </summary>
        public void MakePayment()
        {
            if (Deposits != null)
            {
                foreach (var deposit in Deposits)
                {
                    deposit.MakePayment();
                }
            }

            if (Credits != null)
            {
                foreach (var credit in Credits)
                {
                    credit.MakePayment();
                }
            }
        }


        /// <summary>
        /// Закрытие кредита
        /// </summary>
        /// <param name="credit">Кредит</param>
        public void CloseCredit(CreditModel credit)
        {
            Credits.Remove(credit);
            CreditClosed?.Invoke(this, credit);
        }

        /// <summary>
        /// Закрытие депозита
        /// </summary>
        /// <param name="deposit">Депозит</param>
        public void CloseDeposit(DepositModel deposit)
        {
            Deposits.Remove(deposit);
            DepositClosed?.Invoke(this, deposit);
        }

        /// <summary>
        /// Перевод денег
        /// </summary>
        /// <param name="client">Клиент</param>
        /// <param name="amount">Сумма</param>
        /// <returns></returns>
        public bool SendMoneyTo(AClient client, decimal amount)
        {
            if (client == null || amount <= 0) return false;
            if (Amount < amount)
            {
                NotEnoughtMoney?.Invoke(this);
                return false;
            }

            Amount -= amount;
            client.Amount += amount;
            MoneySended?.Invoke(this, client);

            return true;
        }

        #endregion

    }
}
