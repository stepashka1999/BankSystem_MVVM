﻿using BankSystem.Models;
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
    /// Логика взаимодействия для TransactWindowView.xaml
    /// </summary>
    public partial class TransactWindowView : Window
    {
        public TransactWindowView(BankDbContext context, AClient selectedItem)
        {
            InitializeComponent();
            DataContext = new TransactViewModel(context, selectedItem, this);
        }
    }
}