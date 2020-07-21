using System;

namespace BankSystem.Models
{
    /// <summary>
    /// Класс кредита
    /// </summary>
    public class CreditModel: AEcoItem
    {
        /// <summary>
        /// Событие закрытия кредита
        /// </summary>
        public static event Action<CreditModel> CreditClosed;
        /// <summary>
        /// События совершения платы по кредиту
        /// </summary>
        public static event Action<AClient, CreditModel> MakedPayment;


        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public CreditModel() { }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="Holder">Клиент</param>
        /// <param name="Amount">Сумма</param>
        /// <param name="Month">Срок в месяцах</param>
        public CreditModel(AClient Holder, decimal Amount, int Month):base(Holder, Amount, Month)
        {
            Payment = Amount / Month;
        }


        /// <summary>
        /// Выплата по кредиту
        /// </summary>
        public override void MakePayment()
        {
            if(Holder.WithdrawMoney(Payment))
            {
                Amount -= Payment;
                Month--;
                MakedPayment?.Invoke(Holder, this);
                
                if(Amount == 0 || Month == 0)
                {
                    Close();
                }
            }
        }


        /// <summary>
        /// Закрытие кредита
        /// </summary>
        public void Close()
        {
            Holder.CloseCredit(this);
            CreditClosed?.Invoke(this);
        }

    }
}
