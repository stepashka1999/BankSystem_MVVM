using BankSystem.Models;
using BankSystem.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BankSystem.ViewModels
{
    class AClientViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private BankDbContext context;


        private AClient selectedItem;
        public AClient SelectedItem { get => selectedItem; set { selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); } }


        private AEcoItem selectedEcoItem;
        public AEcoItem SelectedEcoItem { get => selectedEcoItem; set { selectedEcoItem = value; OnPropertyChanged(nameof(SelectedEcoItem)); } }


        public BindingList<ClientModel> Clients { get; set; }
        public BindingList<OrganisationModel> Organisations { get; set; }
        public BindingList<CreditModel> Credits { get; set; }
        public BindingList<DepositModel> Deposits { get; set; }

        public AClientViewModel(string connectionString)
        {
            context = new BankDbContext(connectionString);

            context.Clients.Load();
            context.Organisations.Load();
            context.Credits.Load();
            context.Deposits.Load();

            Clients = context.Clients.Local.ToBindingList();
            Organisations = context.Organisations.Local.ToBindingList();
            Credits = context.Credits.Local.ToBindingList();
            Deposits = context.Deposits.Local.ToBindingList();
        }

        private void Remove(AClient obj)
        {
            if (obj is OrganisationModel)
            {
                context.Organisations.Remove((OrganisationModel)obj);
                context.SaveChanges();
            }
            else if (obj is ClientModel)
            {
                context.Clients.Remove((ClientModel)obj);
                context.SaveChanges();
            }
        }
        //Commands
        public ICommand RemoveCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    Remove(SelectedItem);
                },
                obj => selectedItem != null);
            }
        }

        private void AddClient()
        {
            var addClientWindow = new AddClientWindowView(context);

            if (addClientWindow.ShowDialog() == true)
            {
                var client = addClientWindow.Client;
                context.Clients.Add(client);
                context.SaveChanges();
            }
        }

        public void AddOrganisation()
        {
            var addOrgWindow = new AddOrganisationWindow(context);

            if (addOrgWindow.ShowDialog() == true)
            {
                var org = addOrgWindow.Organisation;
                context.Organisations.Add(org);
                context.SaveChanges();
            }
        }

        public void Edit(AClient aClient)
        {
            if (aClient is OrganisationModel)
            {
                EditOrganisation(aClient as OrganisationModel);
            }
            else if (aClient is ClientModel)
            {
                EditClient(aClient as ClientModel);
            }
        }

        private void EditClient(ClientModel client)
        {
            var addClientWindow = new AddClientWindowView(context, client);

            if (addClientWindow.ShowDialog() == true)
            {
                //var editedClient = addClientWindow.Client;
                context.Clients.AddOrUpdate(client);
                context.SaveChanges();
            }
        }

        private void EditOrganisation(OrganisationModel organisation)
        {
            var addOrgWindow = new AddOrganisationWindow(context, organisation);

            if (addOrgWindow.ShowDialog() == true)
            {
                //var org = addOrgWindow.Organisation;
                context.Organisations.AddOrUpdate(organisation);
                context.SaveChanges();
            }
        }

        public ICommand AddClientCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    AddClient();
                });
            }
        }

        public ICommand AddOrganisationCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    AddOrganisation();
                });
            }
        }

        public ICommand EditCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    Edit(selectedItem);
                }, obj => selectedItem != null);
            }
        }

        public ICommand TransactCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    Transact(selectedItem);
                }, obg => SelectedItem != null);
            }
        }

        private void Transact(AClient aClient)
        {
            var transactWindow = new TransactWindowView(context, selectedItem);
            if (transactWindow.ShowDialog() == true)
            {
                context.SaveChanges();
            }
        }

        private void AddCredit()
        {
            var credit = new CreditModel();
            var addCreditWindow = new AddCreditWindowView(SelectedItem, credit);
            if (addCreditWindow.ShowDialog() == true)
            {
                context.Credits.Add(credit);
                context.SaveChanges();
                SelectedItem = SelectedItem;
            }
        }

        public ICommand AddCreditCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    AddCredit();
                }, obj => selectedItem!=null);
            }
        }

        private void AddDeposit()
        {
            var deposit = new DepositModel();
            var addDepositWindow = new AddDepositMindowView(SelectedItem, deposit);
            if (addDepositWindow.ShowDialog() == true)
            {
                context.Deposits.Add(deposit);
                context.SaveChanges();
                SelectedItem = SelectedItem;
            }
        }

        public ICommand AddDepositCommand
        {
            get
            {
                return new DelegateCommand(obj=>
                {
                    AddDeposit();
                }, obj => selectedItem != null);
            }
        }

        private void RemoveEcoItem()
        {
            if(SelectedEcoItem is CreditModel)
            {
                RemoveCredit();
            }
            else if(SelectedEcoItem is DepositModel)
            {
                RemoveDeposit();
            }
        }

        private void RemoveCredit()
        {
            context.Credits.Remove(SelectedEcoItem as CreditModel);
            context.SaveChanges();
        }

        private void RemoveDeposit()
        {
            context.Deposits.Remove(SelectedEcoItem as DepositModel);
            context.SaveChanges();
        }

        public ICommand RemoveAEcoItem
        {
            get
            {
                return new DelegateCommand(obj=>
                {
                    RemoveEcoItem();
                }, obj => SelectedEcoItem != null);
            }
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ICommand SimulateCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    Simulate();
                }, obj => Credits!= null || Deposits != null);
            }
        }

        private void Simulate()
        {
            foreach(var client in Clients)
            {
                client.MakePayment();
            }

            foreach(var org in Organisations)
            {
                org.MakePayment();
            }

            context.SaveChanges();
        }
    }
}
