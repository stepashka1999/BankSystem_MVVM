using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Models
{
    public class CreditModel: AEcoItem
    {
        public static event Action<CreditModel> CreditClosed;
        public static event Action<AClient, CreditModel> MakedPayment;


        public CreditModel() { }
        public CreditModel(AClient Holder, decimal Amount, int Month):base(Holder, Amount, Month)
        {
            Payment = Amount / Month;
        }

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

        public void Close()
        {
            Holder.CloseCredit(this);
            CreditClosed?.Invoke(this);
        }

    }
}
