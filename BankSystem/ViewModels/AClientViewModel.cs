using BankSystem.Models;
using BankSystem.Views;
using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Windows.Input;

namespace BankSystem.ViewModels
{
    /// <summary>
    /// Класс ViewModel для AClient
    /// </summary>
    class AClientViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие изменения данных
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Контекст БД
        /// </summary>
        private BankDbContext context;


        //Выбранный объект AClient(ClientModel or OrganisationModel)
        private AClient selectedItem;
        /// <summary>
        /// Выбранный объект AClient(ClientModel or OrganisationModel)
        /// </summary>
        public AClient SelectedItem { get => selectedItem; set { selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); } }


        // Выбранный объект AEcoItem(CreditModel or DepositModel)
        private AEcoItem selectedEcoItem;
        /// <summary>
        /// Выбранный объект AEcoItem(CreditModel or DepositModel)
        /// </summary>
        public AEcoItem SelectedEcoItem { get => selectedEcoItem; set { selectedEcoItem = value; OnPropertyChanged(nameof(SelectedEcoItem)); } }


        /// <summary>
        /// Коллекция клиентов
        /// </summary>
        public BindingList<ClientModel> Clients { get; set; }

        /// <summary>
        /// Коллекция организаций
        /// </summary>
        public BindingList<OrganisationModel> Organisations { get; set; }

        /// <summary>
        /// Коллекция кредитов
        /// </summary>
        public BindingList<CreditModel> Credits { get; set; }

        /// <summary>
        /// Коллекция депозитов
        /// </summary>
        public BindingList<DepositModel> Deposits { get; set; }



        /// <summary>
        /// Конструктор
        /// </summary>
        public AClientViewModel()
        {
            context = new BankDbContext();

            context.Clients.Load();
            context.Organisations.Load();
            context.Credits.Load();
            context.Deposits.Load();

            Clients = context.Clients.Local.ToBindingList();
            Organisations = context.Organisations.Local.ToBindingList();
            Credits = context.Credits.Local.ToBindingList();
            Deposits = context.Deposits.Local.ToBindingList();
        }


        //----- Methods -----
        #region Methods


        /// <summary>
        /// Удаление AClient (ClientModel or OrganisationModel)
        /// </summary>
        /// <param name="obj">Клиент</param>
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


        /// <summary>
        /// Добавление клиента
        /// </summary>
        private void AddClient()
        {
            var addClientWindow = new AddClientWindowView();

            if (addClientWindow.ShowDialog() == true)
            {
                context.Clients.Load();
            }
        }

        /// <summary>
        /// Добавление организации 
        /// </summary>
        private void AddOrganisation()
        {
            var addOrgWindow = new AddOrganisationWindow();

            if (addOrgWindow.ShowDialog() == true)
            {
                context.Organisations.Load();
            }
        }


        /// <summary>
        /// Редактирование AClient (ClientModel OrganisationModel)
        /// </summary>
        /// <param name="aClient">Клиент</param>
        private void Edit(AClient aClient)
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

        /// <summary>
        /// Редактирование клиента
        /// </summary>
        /// <param name="client">Клиент</param>
        private void EditClient(ClientModel client)
        {
            var editClientView = new EditClientView(client);
            editClientView.ShowDialog();
        }

        /// <summary>
        /// Редактирование организации
        /// </summary>
        /// <param name="organisation">Организация</param>
        private void EditOrganisation(OrganisationModel organisation)
        {
            var editOrganisationView = new EditOrganisationView(organisation);
            editOrganisationView.ShowDialog();
        }


        /// <summary>
        /// Первеод средств
        /// </summary>
        /// <param name="aClient">Клинет</param>
        private void Transact(AClient aClient)
        {
            var transactWindow = new TransactWindowView(selectedItem);
            transactWindow.ShowDialog();
        }


        /// <summary>
        /// Добавление кредита
        /// </summary>
        private void AddCredit()
        {
            var addCreditWindow = new AddCreditWindowView(SelectedItem);
            if (addCreditWindow.ShowDialog() == true)
            {
                context.Clients.Load();
                context.Organisations.Load();
                context.Credits.Load();
            }
        }

        /// <summary>
        /// Добавление депозита
        /// </summary>
        private void AddDeposit()
        {
            var addDepositWindow = new AddDepositMindowView(SelectedItem);

            if (addDepositWindow.ShowDialog() == true)
            {
                context.Clients.Load();
                context.Organisations.Load();
                context.Deposits.Load();
            }
        }


        /// <summary>
        /// Удаление AEcoItem (CreditModel or DepositMode)
        /// </summary>
        private void RemoveEcoItem()
        {
            if (SelectedEcoItem is CreditModel)
            {
                RemoveCredit();
            }
            else if (SelectedEcoItem is DepositModel)
            {
                RemoveDeposit();
            }
        }

        /// <summary>
        /// Ужадегте кредита
        /// </summary>
        private void RemoveCredit()
        {
            context.Credits.Remove(SelectedEcoItem as CreditModel);
            context.SaveChanges();
        }

        /// <summary>
        /// Удаление депозита
        /// </summary>
        private void RemoveDeposit()
        {
            context.Deposits.Remove(SelectedEcoItem as DepositModel);
            context.SaveChanges();
        }


        /// <summary>
        /// Вызывает событие изменения данных
        /// </summary>
        /// <param name="name">Имя измененных данных</param>
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        /// <summary>
        /// Симуляция ежемесячного платежа
        /// </summary>
        private void Simulate()
        {
            foreach (var client in Clients)
            {
                client.MakePayment();
            }

            foreach (var org in Organisations)
            {
                org.MakePayment();
            }

            context.SaveChanges();
        }

        #endregion

        //----- Commands -----
        #region Commands

        
        /// <summary>
        /// Команда удаления AClient (ClientModel or OrganisationModel)
        /// </summary>
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

        /// <summary>
        /// Команда добавления клиента
        /// </summary>
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

        /// <summary>
        /// Команда добавление организации
        /// </summary>
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

        /// <summary>
        /// Команда редактирование AClient (ClientModel or OrganisationModel)
        /// </summary>
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

        /// <summary>
        /// Команда перевода средств
        /// </summary>
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

        /// <summary>
        /// Команда добавления кредита
        /// </summary>
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

        /// <summary>
        /// Команда добавления депозита
        /// </summary>
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

        /// <summary>
        /// Команда удаления AEcoItem (CreditModel or DepositModel)
        /// </summary>
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

        /// <summary>
        /// Команда симуляции ежемесячного платежа
        /// </summary>        
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

        #endregion
    }
}
