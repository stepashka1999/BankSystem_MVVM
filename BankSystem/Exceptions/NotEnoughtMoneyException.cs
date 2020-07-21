using BankSystem.Models;
using System;

namespace BankSystem
{
    /// <summary>
    /// Ошибка "Недостаточно денег"
    /// </summary>
    class NotEnoughtMoneyException: Exception
    {
        /// <summary>
        /// Конструктрок
        /// </summary>
        public NotEnoughtMoneyException() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="holder">Клиент</param>
        public NotEnoughtMoneyException(AClient holder)
            : base($"У клиента {holder} не хватает средств, для проведения операции.")
        { }

    }
}
