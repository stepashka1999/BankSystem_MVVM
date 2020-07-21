using System;

namespace BankSystem.Exceptions
{
    /// <summary>
    /// Ошибка "Незаполенный экземпляр класса"
    /// </summary>
    class UnfilledInstanceException:Exception
    {
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public UnfilledInstanceException() { }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="objectName">Экземпляр класса</param>
        public UnfilledInstanceException(string objectName)
            :base($"Поля {objectName} заполнены.")
        { }
    }
}
