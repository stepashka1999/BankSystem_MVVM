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
    /// Логика взаимодействия для AddOrganisationWindow.xaml
    /// </summary>
    partial class AddOrganisationWindow : Window
    {
        public OrganisationModel Organisation { get; set; } = new OrganisationModel();
        public AddOrganisationWindow(BankDbContext context)
        {
            InitializeComponent();
            DataContext = new AddOrgViewModel(context, this);
        }

        public AddOrganisationWindow(BankDbContext context, OrganisationModel organisation)
        {
            InitializeComponent();
            Organisation = organisation;
            
            DataContext = new AddOrgViewModel(context, this);
        }
    }
}