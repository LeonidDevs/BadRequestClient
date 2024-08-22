using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using VittaClient.Services;
using VittaClient.Tools;
using VittaClient.Models;
using VittaClient.Views;

namespace VittaClient.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        private readonly MainWindow _mainWindow;

        private readonly ApiService _apiService;
        private readonly UserDTO _user;

        private ObservableCollection<OrderDTO> _orders;

        public ObservableCollection<OrderDTO> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        public ICommand CreateOrderCommand { get; }

        public OrdersViewModel(MainWindow mainWindow, ApiService apiService, UserDTO user)
        {
            _mainWindow = mainWindow;
            CreateOrderCommand = new RelayCommand(CreateOrderExecute);

            _apiService = apiService;
            _user = user;

            Orders = new ObservableCollection<OrderDTO>();
            LoadOrdersAsync();
        }

        private void CreateOrderExecute(object parameter)
        {
            _mainWindow.ChangePage(new CreateOrderView(_apiService, _user));
        }

        private async void LoadOrdersAsync()
        {
            try
            {
                var ordersList = await _apiService.GetOrdersAsync(_user);

                Orders.Clear();
                foreach (var order in ordersList)
                {
                    Orders.Add(order);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                MessageBox.Show($"Ошибка при загрузке заказов: {ex.Message}");
            }
        }
    }
}
