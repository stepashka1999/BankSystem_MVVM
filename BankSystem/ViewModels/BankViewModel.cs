using BankSystem.Models;
using BankSystem.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;


namespace BankSystem.ViewModels
{
    class BankViewModel
    {
        private BankDbContext context; 

        public EmployeeViewModel EmployeeViewModel { get; set; }
        public DepartamentViewModel DepartamentViewModel { get; set; }
        public LogViewModel LogViewModel { get; set; }
        public AClientViewModel AClientViewModel { get; set; }
        public EcoItemViewModel EcoItemViewModel { get; set; }

        public BankViewModel()
        {
            string connectionName;
            if (ConfigurationManager.ConnectionStrings["BankDB"] == null)
            {
                if (SetConnectionName() == false) return;
            }

            connectionName = ConfigurationManager.ConnectionStrings["BankDB"].Name;
            
            context = new BankDbContext(connectionName);

            AClientViewModel = new AClientViewModel(connectionName);
            DepartamentViewModel = new DepartamentViewModel(connectionName);
            LogViewModel = new LogViewModel(connectionName);
            EmployeeViewModel = new EmployeeViewModel(connectionName);
            EcoItemViewModel = new EcoItemViewModel(context);
        }

        private bool SetConnectionName()
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
