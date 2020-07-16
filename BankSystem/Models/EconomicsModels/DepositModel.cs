using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class DepositModel: AEcoItem
    {
        public static event Action<DepositModel> DepositClosed;
        public static event Action<AClient, DepositModel> MakedPayment;

        public DepositModel() { }
        public DepositModel(AClient Holder, decimal Amount, int Month): base(Holder, Amount, Month)
        {
            Payment = Amount * (Percent / 100) / 12;
        }


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

        public void Close()
        {
            Holder.CloseDeposit(this);
            DepositClosed(this);
        }
    }
}
