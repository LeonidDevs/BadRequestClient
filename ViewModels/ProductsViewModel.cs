using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using VittaClient.Models;
using VittaClient.Services;
using VittaClient.Tools;
using VittaClient.Views;

namespace VittaClient.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        private readonly MainWindow _mainWindow;
        private readonly ApiService _apiService;
        private OrderDTO _currentOrder;

        private ObservableCollection<ProductViewModel> _products;
        public ObservableCollection<ProductViewModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public ICommand AddToOrderCommand { get; }
        public ICommand AcceptCommand { get; }

        public ProductsViewModel(MainWindow mainWindow, ApiService apiService, OrderDTO currentOrder, int userId)
        {
            _mainWindow = mainWindow;

            _apiService = apiService;
            _currentOrder = currentOrder;
            _currentOrder.UserId = userId;

            Products = new ObservableCollection<ProductViewModel>();
            LoadProductsAsync();

            AddToOrderCommand = new RelayCommand(AddToOrder);
            AcceptCommand = new RelayCommand(AcceptOrder);
        }

        private void AddToOrder(object obj)
        {
            var productVM = obj as ProductViewModel;
            if (productVM != null && productVM.Quantity > 0)
            {
                // Проверяем, существует ли уже такой продукт в заказе
                var existingOrderItem = _currentOrder.OrderItems
                    .FirstOrDefault(item => item.ProductId == productVM.Product.ProductId);

                if (existingOrderItem != null)
                {
                    // Если продукт уже существует, просто увеличиваем его количество
                    existingOrderItem.Quantity += productVM.Quantity;
                }
                else
                {
                    // Если продукт новый, добавляем его в заказ
                    var newOrderItem = new OrderItemDTO
                    {
                        ProductId = productVM.Product.ProductId,
                        Quantity = productVM.Quantity,
                        Product = productVM.Product
                    };
                    _currentOrder.OrderItems.Add(newOrderItem);
                }
            }
        }

        private void AcceptOrder(object obj)
        {
            _mainWindow.ChangePage(new CreateOrderView(_apiService, _currentOrder, _currentOrder.UserId));
        }

        private async void LoadProductsAsync()
        {
            try
            {
                var productsList = await _apiService.GetProductsAsync();

                Products.Clear();
                foreach (var product in productsList)
                {
                    Products.Add(new ProductViewModel(product));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке продуктов: {ex.Message}");
            }
        }
    }
}
