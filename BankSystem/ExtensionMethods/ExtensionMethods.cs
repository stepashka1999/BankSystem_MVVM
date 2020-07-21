using BankSystem.Models;

namespace BankSystem
{
    /// <summary>
    /// Статический класс с методами расширения
    /// </summary>
    static class ExtensionMethods
    {
        /// <summary>
        /// Добавление кредита к клиенту
        /// </summary>
        /// <param name="credit">Кредит</param>
        /// <param name="client">Клиент</param>
        public static void Add(this CreditModel credit, AClient client)
        {
            client.AddCredit(credit);
        }

        /// <summary>
        /// Добавление депозита клиенту
        /// </summary>
        /// <param name="deposit">Депозит</param>
        /// <param name="client">Клиент</param>
        public static void Add(this DepositModel deposit, AClient client)
        {
            client.AddDeposit(deposit);
        }
    }
}
