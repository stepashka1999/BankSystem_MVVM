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
    class OrganisationsViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BankDbContext context;
        
        
        private OrganisationModel selectedOrganisation;
        public OrganisationModel SelectedOrganisation { get => selectedOrganisation; set { selectedOrganisation = value; OnPropertyChanged(nameof(SelectedOrganisation)); } }

        public BindingList<OrganisationModel> Organisations { get; set; }


        public OrganisationsViewModel(string connection)
        {
            context = new BankDbContext(connection);
            context.Organisations.Load();
            Organisations = context.Organisations.Local.ToBindingList();
        }


        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Remove(OrganisationModel org)
        {
            if (org == null) return;
            context.Organisations.Remove(org);
            context.SaveChanges();
        }
        //Commands
        public ICommand Delete
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    Remove(SelectedOrganisation);
                },
                obj => Organisations.Count > 0);
            }
        }

        public ICommand Add
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    throw new NotImplementedException();
                });
            }
        }
    }
}
