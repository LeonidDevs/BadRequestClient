using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using VittaClient.Models;
using VittaClient.Services;
using VittaClient.Tools;
using VittaClient.Views;

namespace VittaClient.ViewModels
{
    public class CreateOrderViewModel : ViewModelBase
    {
        private readonly MainWindow _mainWindow;
        private readonly ApiService _apiService;
        private readonly UserDTO _user;
        private readonly int _userId;
        private OrderDTO _order;

        private ObservableCollection<OrderItemDTO> _selectedOrderItems;
        public ObservableCollection<OrderItemDTO> SelectedOrderItems
        {
            get => _selectedOrderItems;
            set
            {
                _selectedOrderItems = value;
                OnPropertyChanged(nameof(SelectedOrderItems));
            }
        }

        public decimal TotalAmount => _order.OrderItems.Sum(item => item.Quantity * item.Product.Price);
        private void CalculateTotalAmount()
        {
            _order.TotalAmount = _order.OrderItems.Sum(item => item.Product.Price * item.Quantity);
            OnPropertyChanged(nameof(TotalAmount));
        }

        public ICommand AddProductsCommand { get; set; }
        public ICommand BuyCommand { get; set; }


        public CreateOrderViewModel(MainWindow mainWindow, ApiService apiService, UserDTO user)
        {
            _mainWindow = mainWindow;

            _apiService = apiService;
            _user = user;

            _order = new OrderDTO { UserId = _user.UserId };
            SelectedOrderItems = new ObservableCollection<OrderItemDTO>(_order.OrderItems);

            AddProductsCommand = new RelayCommand(AddProductExecute);
        }

        public CreateOrderViewModel(MainWindow mainWindow, ApiService apiService, OrderDTO order, UserDTO user)
        {
            _mainWindow = mainWindow;
            _order = order;
            _order.User = user;

            _apiService = apiService;
            SelectedOrderItems = order.OrderItems.ToObservableCollection();
            foreach (var item in _order.OrderItems)
            {
                item.OrderId = _order.OrderId;
            }

            AddProductsCommand = new RelayCommand(AddProductExecute);
            BuyCommand = new RelayCommand(async _ => await CreateOrderAsync());
        }

        private void AddProductExecute(object parameter)
        {

            _mainWindow.ChangePage(new ProductsView(_apiService, _order, _user));

            OnPropertyChanged(nameof(SelectedOrderItems));
            OnPropertyChanged(nameof(TotalAmount));
        }

        private async Task CreateOrderAsync()
        {
            try
            {
                CalculateTotalAmount();

                var orderDto = new OrderDTO
                {
                    UserId = _order.UserId,
                    OrderDate = DateTime.Now,
                    TotalAmount = _order.TotalAmount,
                    OrderItems = _order.OrderItems.Select(oi => new OrderItemDTO
                    {
                        ProductId = oi.ProductId,
                        OrderId = oi.OrderId,
                        Quantity = oi.Quantity
                    }).ToList()
                };

                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                var jsonContent = JsonSerializer.Serialize(orderDto, jsonSerializerOptions);

                Console.WriteLine("Sending JSON: " + jsonContent);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _apiService.CreateOrderAsync(orderDto);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Order successfully created!");
                    _mainWindow.ChangePage(new OrdersView(_apiService, _user));

                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Failed to create order. Server response: {errorContent}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"HTTP Error occurred: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
