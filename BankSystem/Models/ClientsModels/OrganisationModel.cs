using System;

namespace BankSystem.Models
{
    /// <summary>
    /// Класс организации
    /// </summary>
    public class OrganisationModel: AClient
    {
        /// <summary>
        /// Событие изменения данных
        /// </summary>
        public event Action<OrganisationModel> DataChanged;


        // Имя
        private string name;
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get => name; set { name = value; OnPropertyChanged(nameof(Name)); OnPropertyChanged(nameof(Info)); } }


        /// <summary>
        /// Информация о организации
        /// </summary>
        public override string Info
        {
            get => $"Name: {Name}\n" +
                   $"Account: {Account}\n" +
                   $"Ammount: {Amount}";
        }



        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public OrganisationModel() { }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <param name="Account">Номер счета</param>
        /// <param name="Amount">Сумма счета</param>
        /// <param name="creditHistory">Кредитная история</param>
        public OrganisationModel(string Name, long Account, decimal Amount, CreditHistory creditHistory)
            : base(Account, Amount, creditHistory)
        {
            this.Name = Name;
        }



        /// <summary>
        /// Изменени данных организации
        /// </summary>
        /// <param name="name">Имя</param>
        public void Edit(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
