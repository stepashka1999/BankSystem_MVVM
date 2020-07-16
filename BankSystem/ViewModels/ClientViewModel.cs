using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BankSystem.ViewModels
{
    class ClientViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BankDbContext context;


        private ClientModel selectedClient;
        public ClientModel SelectedClient { get => selectedClient; set { selectedClient = value; OnPropertyChanged(nameof(SelectedClient));  } }

        public BindingList<ClientModel> Clients { get; set; }


        public ClientViewModel(string connection)
        {
            context = new BankDbContext(connection);
            context.Clients.Load();
            Clients = context.Clients.Local.ToBindingList();
        }


        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void RemoveClient(ClientModel client)
        {
            if (client == null) return;
            context.Clients.Remove(client);
            context.SaveChanges();
        }
        //Commands
        public ICommand DeleteClient
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    RemoveClient(SelectedClient);
                },
                obj => Clients.Count > 0);
            }
        }
    }
}
