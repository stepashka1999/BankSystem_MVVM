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
    /// Логика взаимодействия для AddDepositMindowView.xaml
    /// </summary>
    public partial class AddDepositMindowView : Window
    {
        public AddDepositMindowView(AClient holder, DepositModel credit)
        {
            InitializeComponent();
            DataContext = new AddDepositViewModel(holder, credit, this);
        }
    }
}
