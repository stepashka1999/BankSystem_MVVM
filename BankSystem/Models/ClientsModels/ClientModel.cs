namespace BankSystem.Models
{
    /// <summary>
    /// Класс клиента
    /// </summary>
    public class ClientModel:AClient
    {
        // Имя
        private string firstName;
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get => firstName; set { firstName = value; OnPropertyChanged(nameof(FirstName)); } }


        // Фамилия
        private string secondName;
        /// <summary>
        /// Фамилия
        /// </summary>
        public string SecondName { get => secondName; set { secondName = value; OnPropertyChanged(nameof(SecondName)); } }


        // Статус (Vip или нет)
        private bool isVip;
        /// <summary>
        /// Статус (Vip или нет)
        /// </summary>
        public bool IsVip { get => isVip; set { isVip = value; OnPropertyChanged(nameof(IsVip));} }


        /// <summary>
        /// Информация о клиенте
        /// </summary>
        public override string Info { get => $"First Name: {FirstName}\n" +
              $"Second Name: {SecondName}\n" +
              $"VIP: {IsVip}\n" +
              $"Account: {Account}\n" +
              $"Ammount: {Amount}";
        }



        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public ClientModel() { }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="fName">Имя</param>
        /// <param name="sName">Фамилия</param>
        /// <param name="vip">Статус (Vip или нет)</param>
        /// <param name="Account">Номер счета</param>
        /// <param name="Amount">Сумма счета</param>
        /// <param name="creditHistory">Кредитная история</param>
        public ClientModel(string fName, string sName, bool vip, long Account, decimal Amount, CreditHistory creditHistory)
            :base(Account, Amount, creditHistory)
        {
            FirstName = fName;
            SecondName = sName;
            IsVip = vip;
        }


        //----- Methods -----
        #region Methods


        /// <summary>
        /// Изменения статуса Vip
        /// </summary>
        /// <param name="newVip">Новы статус</param>
        public void ChangeVip(bool newVip)
        {
            IsVip = newVip;
        }

        /// <summary>
        /// Изменение данных
        /// </summary>
        /// <param name="fName">Имя</param>
        /// <param name="sName">Фамилия</param>
        /// <param name="vip">Статус</param>
        public void EditData(string fName, string sName, bool vip)
        {
            FirstName = fName;
            SecondName = sName;
            IsVip = vip;
        }


        public override string ToString()
        {
            return $"{FirstName} {SecondName}";
        }

        #endregion
    }
}
