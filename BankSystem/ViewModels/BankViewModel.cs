using BankSystem.Models;
using BankSystem.Views;
using System.Configuration;
using System.Windows;

namespace BankSystem.ViewModels
{
    /// <summary>
    /// Клас ViewModel для контекста БД Банка
    /// </summary>
    class BankViewModel
    {
        /// <summary>
        /// Контекст БД
        /// </summary>
        private BankDbContext context;

        /// <summary>
        /// Имя подключения
        /// </summary>
        private string connectionName = "BankDB";


        /// <summary>
        /// ViewModel для работы департаментами
        /// </summary>
        public DepartamentViewModel DepartamentViewModel { get; set; }

        /// <summary>
        /// ViewModel для работы с логами
        /// </summary>
        public LogViewModel LogViewModel { get; set; }

        /// <summary>
        /// ViewModel для работы с клиентами
        /// </summary>
        public AClientViewModel AClientViewModel { get; set; }


        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public BankViewModel()
        {

            if (ConfigurationManager.ConnectionStrings[connectionName] == null)
            {
                if (WriteConntection() == false) return;
            };
            
            context = new BankDbContext();

            AClientViewModel = new AClientViewModel();
            DepartamentViewModel = new DepartamentViewModel();
            LogViewModel = new LogViewModel();
        }


        /// <summary>
        /// Запись подключения в AppConfig
        /// </summary>
        /// <returns></returns>
        private bool WriteConntection()
        {
            var window = new FirstLoadView();
            if(window.ShowDialog() == false)
            {
                Application.Current.MainWindow.Close();
                return false;
            }

            return true;
        }
    }
}
