using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace BankSystem.Models
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(string connectionName) : base(connectionName)
        {
            Departaments.Load();

            if (Departaments == null || Departaments.Count() == 0)  BaseFill();

            Clients.Load();
            Credits.Load();
            Deposits.Load();
            Logs.Load();
            Organisations.Load();
            CreditHistories.Load();
        }

        public DbSet<CreditModel> Credits { get; set; }
        public DbSet<DepositModel> Deposits { get; set; }
        public DbSet<DepartamentModel> Departaments { get; set; }
        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<OrganisationModel> Organisations { get; set; }
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<LogModel> Logs { get; set; }
        public DbSet<CreditHistory> CreditHistories { get; set; }

        private void BaseFill()
        {
            FillCreditHistory();
            FillDepartaments();
        }

        private void FillCreditHistory()
        {
            var goodHistory = new CreditHistory()
            {
                Name = "Good",
                Percent = 4
            };

            var normalHistory = new CreditHistory()
            {
                Name = "Normal",
                Percent = 8
            };

            var badHistory = new CreditHistory()
            {
                Name = "Bad",
                Percent = 16
            };


            CreditHistories.Add(goodHistory);
            CreditHistories.Add(normalHistory);
            CreditHistories.Add(badHistory);

            SaveChanges();
        }
        private void FillClients()
        {
            var client1 = new ClientModel("Rick", "Prat", false, 1234567891234567, 500000, CreditHistories.First());
            var client2 = new ClientModel("Morty", "Vizl", false, 1234567891234567, 500000, CreditHistories.First());
            var client3 = new ClientModel("Vasya", "Jebs", false, 1234567891234567, 500000, CreditHistories.First());
            
            var client11 = new ClientModel("Katya", "Jobs", true, 1234567891234567, 500000, CreditHistories.First());
            var client21 = new ClientModel("Mer", "Grean", true, 1234567891234567, 500000, CreditHistories.First());
            var client31 = new ClientModel("Grick", "Lone", true, 1234567891234567, 500000, CreditHistories.First());

            
            Clients.Add(client1);
            Clients.Add(client2);
            Clients.Add(client3);

            Clients.Add(client11);
            Clients.Add(client21);
            Clients.Add(client31);

            SaveChanges();
        }

        private void FillOrganisations()
        {
            var org1 = new OrganisationModel("Valve", 2563145274589658, 500000000, CreditHistories.First());
            var org2 = new OrganisationModel("Forward", 2563145274589678, 100000, CreditHistories.First(x => x.Id == 3));
            var org3 = new OrganisationModel("Rubin", 8523698741255698, 50000000, CreditHistories.First(x => x.Id == 2));

            Organisations.Add(org1);
            Organisations.Add(org2);
            Organisations.Add(org3);
            SaveChanges();
        }

        private void FillDepartaments()
        {
            var dep1 = new DepartamentModel("Default Departament");
            var dep2 = new DepartamentModel("VIP Departament");
            var dep3 = new DepartamentModel("Organisation Departament");

            Departaments.Add(dep1);
            Departaments.Add(dep2);
            Departaments.Add(dep3);
            SaveChanges();
        }
    }
}
