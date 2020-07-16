using BankSystem.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BankSystem.ViewModels
{
    class FirstConnectionViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        FirstLoadView window;
        private string serverName;
        public string ServerName { get => serverName; set { serverName = value; OnPropertyChanged(nameof(ServerName)); } }
        
        
        public FirstConnectionViewModel() { }
        public FirstConnectionViewModel(FirstLoadView window)
        {
            this.window = window;
        }


        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


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
                            //ConfigurationManager.ConnectionStrings["BankDB"].ConnectionString = $"Data Source={ServerName};Initial Catalog=Bank_test_db;Integrated Security=True;Pooling=True";
                            //ConfigurationManager.ConnectionStrings["BankDB"].ProviderName = "System.Data.SqlClient";
                            var conStr = new ConnectionStringSettings("BankDB", $"Data Source={ServerName};Initial Catalog=Bank_test_db;Integrated Security=True;Pooling=True", "System.Data.SqlClient");

                            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                            config.ConnectionStrings.ConnectionStrings.Add(conStr);
                            config.Save(ConfigurationSaveMode.Modified);
                            ConfigurationManager.RefreshSection("connectionStrings");

                            window.DialogResult = true;
                            window.Close();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }
                });
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    window.DialogResult = false;
                    window.Close();
                });
            }
        }
    }
}
