using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Input;

namespace BankSystem.ViewModels
{
    /// <summary>
    /// Класс ViewModel для произведения первого подключения
    /// </summary>
    class FirstConnectionViewModel: INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменения данных поля
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        //Имя SQL сервера
        private string serverName;
        /// <summary>
        /// Имя SQL сервера
        /// </summary>
        public string ServerName { get => serverName; set { serverName = value; OnPropertyChanged(nameof(ServerName)); } }
        
        
        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public FirstConnectionViewModel() { }

        /// <summary>
        /// Вызов события изменения данных поля
        /// </summary>
        /// <param name="name">Имя поля</param>
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        /// <summary>
        /// Команда подключения
        /// </summary>
        public ICommand ConnectCommand 
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    if (string.IsNullOrEmpty(ServerName) == false)
                    {
                        try
                        {
                            var conStr = new ConnectionStringSettings("BankDB", $"Data Source={ServerName};Initial Catalog=Bank_test_db;Integrated Security=True;Pooling=True", "System.Data.SqlClient");

                            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                            config.ConnectionStrings.ConnectionStrings.Add(conStr);
                            config.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("connectionStrings");

                            (obj as Window).DialogResult = true;
                            (obj as Window).Close();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }
                });
            }
        }


        /// <summary>
        /// Команда отмены подключения
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    (obj as Window).DialogResult = false;
                    (obj as Window).Close();
                });
            }
        }
    }
}
