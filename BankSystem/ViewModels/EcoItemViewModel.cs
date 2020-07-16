using BankSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.ViewModels
{
    class EcoItemViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private BankDbContext context;


        private AClient currentClient;
        public AClient CurrentClient { get => currentClient; set { currentClient = value; OnPropertyChanged(nameof(CurrentClient)); } }


        private AEcoItem selectedItem;
        public AEcoItem SelectedItem { get => selectedItem; set { selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); } }


        public BindingList<CreditModel> Credits;
        public BindingList<DepositModel> Deposits;


        public EcoItemViewModel(BankDbContext context)
        {
            this.context = context;
            context.Credits.Load();
            context.Deposits.Load();

            //Credits = (context.Credits.Where(x => x.Id == currentClient.Id) as DbSet<CreditModel>).Local.ToBindingList();
            //Deposits = (context.Deposits.Where(x => x.Id == currentClient.Id) as DbSet<DepositModel>).Local.ToBindingList();
        }


        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
