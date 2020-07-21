using System;

namespace BankSystem.Models
{
    /// <summary>
    /// Класс депозита
    /// </summary>
    public class DepositModel: AEcoItem
    {
        /// <summary>
        /// Событие закрытия депозита
        /// </summary>
        public static event Action<DepositModel> DepositClosed;
        /// <summary>
        /// Событие совершения выплаты по депозиту
        /// </summary>
        public static event Action<AClient, DepositModel> MakedPayment;



        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public DepositModel() { }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="Holder">Клиент</param>
        /// <param name="Amount">Сумма</param>
        /// <param name="Month">Спрок в месяцах</param>
        public DepositModel(AClient Holder, decimal Amount, int Month): base(Holder, Amount, Month)
        {
            Payment = Amount * (Percent / 100) / 12;
        }


        /// <summary>
        /// Совершение выплаты
        /// </summary>
        public override void MakePayment()
        {
            Amount += Payment;
            Month--;
            MakedPayment?.Invoke(Holder, this);

            if (Month == 0)
            {
                Holder.PutMoney(Amount);
                Close();
            }
        }

        /// <summary>
        /// Закрытие депозита
        /// </summary>
        public void Close()
        {
            Holder.CloseDeposit(this);
            DepositClosed(this);
        }
    }
}
