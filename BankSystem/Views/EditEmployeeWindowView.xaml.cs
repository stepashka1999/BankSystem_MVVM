using BankSystem.Models;
using BankSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BankSystem.Views
{
    /// <summary>
    /// Логика взаимодействия для EditEmployeeWindowView.xaml
    /// </summary>
    public partial class EditEmployeeWindowView : Window
    {
        public EditEmployeeWindowView(BankDbContext context, EmployeeModel empl)
        {
            InitializeComponent();
            DataContext = new EditEmployeeViewModel(context, empl, this);
        }
    }
}
