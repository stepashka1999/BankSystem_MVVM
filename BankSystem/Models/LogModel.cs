namespace BankSystem.Models
{
    /// <summary>
    /// Класс лога
    /// </summary>
    public class LogModel
    {
        //Id лога
        private int id;
        /// <summary>
        /// Id лога
        /// </summary>
        public int Id { get => id; set { id = value; } }


        //Сообщение лога
        private string message;
        /// <summary>
        /// Сообщение лога
        /// </summary>
        public string Message { get => message; set { message = value; } }

        public override string ToString()
        {
            return Message;
        }
    }
}
