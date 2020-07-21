using System.Data.Entity;
using System.Linq;

namespace BankSystem.Models
{
    /// <summary>
    /// Класс контекста БД Банка
    /// </summary>
    public class BankDbContext : DbContext
    {
        /// <summary>
        /// Имя подключения
        /// </summary>
        private static readonly string conName = "BankDB";      //Имя подключения


        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public BankDbContext():base(conName)
        {
            Departaments.Load();

            if (Departaments == null || Departaments.Count() == 0) BaseFill();

            Clients.Load();
            Credits.Load();
            Deposits.Load();
            Logs.Load();
            Organisations.Load();
            CreditHistories.Load();
        }


        /// <summary>
        /// Коллекция кредитов
        /// </summary>
        public DbSet<CreditModel> Credits { get; set; }

        /// <summary>
        /// Коллекция депозитов
        /// </summary>
        public DbSet<DepositModel> Deposits { get; set; }

        /// <summary>
        /// Коллекция департаментов
        /// </summary>
        public DbSet<DepartamentModel> Departaments { get; set; }

        /// <summary>
        /// Коллекция клиентов
        /// </summary>
        public DbSet<ClientModel> Clients { get; set; }

        /// <summary>
        /// Коллекция организаций
        /// </summary>
        public DbSet<OrganisationModel> Organisations { get; set; }

        /// <summary>
        /// Коллекция сотрудников
        /// </summary>
        public DbSet<EmployeeModel> Employees { get; set; }

        /// <summary>
        /// Коллекция логов
        /// </summary>
        public DbSet<LogModel> Logs { get; set; }

        /// <summary>
        /// Коллекция типов кредитной истории
        /// </summary>
        public DbSet<CreditHistory> CreditHistories { get; set; }


        /// <summary>
        /// Заполнение БД базовыми значениями
        /// </summary>
        private void BaseFill()
        {
            FillCreditHistory();
            FillDepartaments();
        }


        /// <summary>
        /// Заполение типао кредитной исотрии базовыми значениями
        /// </summary>
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


        /// <summary>
        /// Заполнение Департаментов базовыми значениями
        /// </summary>
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
