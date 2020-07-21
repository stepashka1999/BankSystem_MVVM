using System.ComponentModel;

namespace BankSystem.Models
{
    /// <summary>
    /// Класс кредитнйо истории
    /// </summary>
    public class CreditHistory: INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменения данных поля
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        //Id кредитной истории
        private int id;
        /// <summary>
        /// Id кредитной истории
        /// </summary>
        public int Id { get => id; set { id = value; OnPropertyChanged(nameof(Id)); } }


        //Наименование кредитной истории
        private string name;
        /// <summary>
        /// Наименование кредитной истории
        /// </summary>
        public string Name { get => name; set { name = value; OnPropertyChanged(nameof(Name)); } }
        

        //Процент
        private int percent;
        /// <summary>
        /// Процент
        /// </summary>
        public int Percent { get => percent; set { percent = value; OnPropertyChanged(nameof(Percent)); } }

        
        /// <summary>
        /// Вызов события изменения данных поля
        /// </summary>
        /// <param name="name">Имя поля</param>
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public override string ToString()
        {
            return $"{Name} - {Percent}%";
        }

    }
}
