﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VittaClient.Models;
using VittaClient.Services;
using VittaClient.ViewModels;

namespace VittaClient.Views
{
    /// <summary>
    /// Логика взаимодействия для ProductsView.xaml
    /// </summary>
    public partial class ProductsView : Page
    {
        public ProductsView(ApiService apiService, OrderDTO order, UserDTO user)
        {
            InitializeComponent();
            DataContext = new ProductsViewModel(Application.Current.MainWindow as MainWindow, apiService, order, user);
        }
    }
}
